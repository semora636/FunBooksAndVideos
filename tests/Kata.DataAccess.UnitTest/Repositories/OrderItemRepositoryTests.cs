using Kata.DataAccess.Repositories;
using Kata.Domain.Entities;
using Kata.Domain.Enums;
using Moq;
using System.Data;

namespace Kata.DataAccess.UnitTest.Repositories
{
    public class OrderItemRepositoryTests
    {
        private readonly Mock<ISqlDataAccess> _mockDataAccess;
        private readonly Mock<IDbConnection> _mockConnection;
        private readonly Mock<IDapperWrapper> _mockDapperWrapper;
        private readonly OrderItemRepository _orderItemRepository;

        public OrderItemRepositoryTests()
        {
            _mockDataAccess = new Mock<ISqlDataAccess>();
            _mockConnection = new Mock<IDbConnection>();
            _mockDapperWrapper = new Mock<IDapperWrapper>();
            _mockDataAccess.Setup(da => da.CreateConnection()).Returns(_mockConnection.Object);
            _orderItemRepository = new OrderItemRepository(_mockDataAccess.Object, _mockDapperWrapper.Object);
        }

        [Fact]
        public async Task AddOrderItemAsync_AddsOrderItem()
        {
            // Arrange
            var orderItem = new OrderItem
            {
                PurchaseOrderId = 1,
                ProductId = 1,
                ProductType = ProductType.Video,
                Quantity = 2,
                Price = 15.00m
            };
            int expectedOrderItemId = 3;

            _mockDapperWrapper.Setup(wrapper => wrapper.ExecuteScalarAsync<int>(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    orderItem,
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(expectedOrderItemId);

            var mockTransaction = new Mock<IDbTransaction>();

            // Act
            var result = await _orderItemRepository.AddOrderItemAsync(orderItem, mockTransaction.Object, _mockConnection.Object);

            // Assert
            Assert.Equal(expectedOrderItemId, result);
            _mockDapperWrapper.Verify(wrapper => wrapper.ExecuteScalarAsync<int>(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    orderItem,
                    mockTransaction.Object,
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()), Times.Once);
        }

        [Fact]
        public async Task DeleteOrderItemsByPurchaseOrderIdAsync_DeletesOrderItems()
        {
            // Arrange
            int purchaseOrderId = 1;
            _mockDapperWrapper.Setup(wrapper => wrapper.ExecuteAsync(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(1);

            var mockTransaction = new Mock<IDbTransaction>();

            // Act
            await _orderItemRepository.DeleteOrderItemsByPurchaseOrderIdAsync(purchaseOrderId, mockTransaction.Object, _mockConnection.Object);

            // Assert
            _mockDapperWrapper.Verify(wrapper => wrapper.ExecuteAsync(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    mockTransaction.Object,
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()), Times.Once);
        }

        [Fact]
        public async Task GetOrderItemsByPurchaseOrderIdAsync_ReturnsOrderItems()
        {
            // Arrange
            int purchaseOrderId = 1;
            var expectedOrderItems = new List<OrderItem>
            {
                new OrderItem { PurchaseOrderId = purchaseOrderId, ProductId = 1, ProductType = ProductType.Video, Quantity = 2, Price = 15.00m },
                new OrderItem { PurchaseOrderId = purchaseOrderId, ProductId = 2, ProductType = ProductType.Book, Quantity = 1, Price = 10.00m }
            };

            _mockDapperWrapper.Setup(wrapper => wrapper.QueryAsync<OrderItem>(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(expectedOrderItems);

            // Act
            var result = await _orderItemRepository.GetOrderItemsByPurchaseOrderIdAsync(purchaseOrderId);

            // Assert
            Assert.Equal(expectedOrderItems, result);
            _mockDapperWrapper.Verify(wrapper => wrapper.QueryAsync<OrderItem>(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()), Times.Once);
        }
    }
}
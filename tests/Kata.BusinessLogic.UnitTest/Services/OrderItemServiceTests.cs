using Kata.BusinessLogic.Services;
using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;
using Moq;
using System.Data;

namespace Kata.BusinessLogic.UnitTest.Services
{
    public class OrderItemServiceTests
    {
        private readonly Mock<IOrderItemRepository> _mockOrderItemRepository;
        private readonly OrderItemService _orderItemService;

        public OrderItemServiceTests()
        {
            _mockOrderItemRepository = new Mock<IOrderItemRepository>();
            _orderItemService = new OrderItemService(_mockOrderItemRepository.Object);
        }

        [Fact]
        public async Task GetOrderItemsByPurchaseOrderIdAsync_ReturnsOrderItems()
        {
            // Arrange
            int purchaseOrderId = 1;
            var expectedOrderItems = new List<OrderItem>
            {
                new OrderItem { PurchaseOrderId = purchaseOrderId, ProductId = 1 },
                new OrderItem { PurchaseOrderId = purchaseOrderId, ProductId = 2 }
            };
            _mockOrderItemRepository.Setup(repo => repo.GetOrderItemsByPurchaseOrderIdAsync(purchaseOrderId)).ReturnsAsync(expectedOrderItems);

            // Act
            var result = await _orderItemService.GetOrderItemsByPurchaseOrderIdAsync(purchaseOrderId);

            // Assert
            Assert.Equal(expectedOrderItems, result);
        }

        [Fact]
        public async Task AddOrderItemAsync_CallsRepositoryAddOrderItemAsync()
        {
            // Arrange
            var orderItem = new OrderItem { PurchaseOrderId = 1, ProductId = 1 };
            var mockTransaction = new Mock<IDbTransaction>().Object;
            var mockConnection = new Mock<IDbConnection>().Object;
            int expectedId = 5;

            _mockOrderItemRepository.Setup(repo => repo.AddOrderItemAsync(orderItem, mockTransaction, mockConnection)).ReturnsAsync(expectedId);

            // Act
            var result = await _orderItemService.AddOrderItemAsync(orderItem, mockTransaction, mockConnection);

            // Assert
            Assert.Equal(expectedId, result);
            _mockOrderItemRepository.Verify(repo => repo.AddOrderItemAsync(orderItem, mockTransaction, mockConnection), Times.Once);
        }

        [Fact]
        public async Task DeleteOrderItemsByPurchaseOrderIdAsync_CallsRepositoryDeleteOrderItemsByPurchaseOrderIdAsync()
        {
            // Arrange
            int purchaseOrderId = 1;
            var mockTransaction = new Mock<IDbTransaction>().Object;
            var mockConnection = new Mock<IDbConnection>().Object;

            // Act
            await _orderItemService.DeleteOrderItemsByPurchaseOrderIdAsync(purchaseOrderId, mockTransaction, mockConnection);

            // Assert
            _mockOrderItemRepository.Verify(repo => repo.DeleteOrderItemsByPurchaseOrderIdAsync(purchaseOrderId, mockTransaction, mockConnection), Times.Once);
        }
    }
}
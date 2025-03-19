using Kata.DataAccess.Repositories;
using Kata.Domain.Entities;
using Moq;
using System.Data;

namespace Kata.DataAccess.UnitTest.Repositories
{
    public class PurchaseOrderRepositoryTests
    {
        private readonly Mock<ISqlDataAccess> _mockDataAccess;
        private readonly Mock<IDbConnection> _mockConnection;
        private readonly Mock<IDapperWrapper> _mockDapperWrapper;
        private readonly PurchaseOrderRepository _purchaseOrderRepository;

        public PurchaseOrderRepositoryTests()
        {
            _mockDataAccess = new Mock<ISqlDataAccess>();
            _mockConnection = new Mock<IDbConnection>();
            _mockDapperWrapper = new Mock<IDapperWrapper>();
            _mockDataAccess.Setup(da => da.CreateConnection()).Returns(_mockConnection.Object);
            _purchaseOrderRepository = new PurchaseOrderRepository(_mockDataAccess.Object, _mockDapperWrapper.Object);
        }

        [Fact]
        public async Task GetPurchaseOrderByIdAsync_ReturnsPurchaseOrder()
        {
            // Arrange
            int purchaseOrderId = 1;
            var expectedPurchaseOrder = new PurchaseOrder { PurchaseOrderId = purchaseOrderId, CustomerId = 1, OrderDateTime = DateTime.Now, TotalPrice = 100.00m };
            _mockDapperWrapper.Setup(wrapper => wrapper.QueryFirstOrDefaultAsync<PurchaseOrder>(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(expectedPurchaseOrder);

            // Act
            var result = await _purchaseOrderRepository.GetPurchaseOrderByIdAsync(purchaseOrderId);

            // Assert
            Assert.Equal(expectedPurchaseOrder, result);
            _mockDapperWrapper.Verify(wrapper => wrapper.QueryFirstOrDefaultAsync<PurchaseOrder>(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()), Times.Once);
        }

        [Fact]
        public async Task GetAllPurchaseOrdersAsync_ReturnsAllPurchaseOrders()
        {
            // Arrange
            var expectedPurchaseOrders = new List<PurchaseOrder>
            {
                new PurchaseOrder { PurchaseOrderId = 1, CustomerId = 1, OrderDateTime = DateTime.Now, TotalPrice = 100.00m },
                new PurchaseOrder { PurchaseOrderId = 2, CustomerId = 2, OrderDateTime = DateTime.Now, TotalPrice = 200.00m }
            };
            _mockDapperWrapper.Setup(wrapper => wrapper.QueryAsync<PurchaseOrder>(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(expectedPurchaseOrders);

            // Act
            var result = await _purchaseOrderRepository.GetAllPurchaseOrdersAsync();

            // Assert
            Assert.Equal(expectedPurchaseOrders, result);
            _mockDapperWrapper.Verify(wrapper => wrapper.QueryAsync<PurchaseOrder>(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()), Times.Once);
        }

        [Fact]
        public async Task AddPurchaseOrderAsync_AddsPurchaseOrder()
        {
            // Arrange
            var purchaseOrder = new PurchaseOrder { CustomerId = 1, OrderDateTime = DateTime.Now, TotalPrice = 150.00m };
            int expectedPurchaseOrderId = 3;

            _mockDapperWrapper.Setup(wrapper => wrapper.ExecuteScalarAsync<int>(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    purchaseOrder,
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(expectedPurchaseOrderId);

            var mockTransaction = new Mock<IDbTransaction>();

            // Act
            var result = await _purchaseOrderRepository.AddPurchaseOrderAsync(purchaseOrder, mockTransaction.Object, _mockConnection.Object);

            // Assert
            Assert.Equal(expectedPurchaseOrderId, result);
            _mockDapperWrapper.Verify(wrapper => wrapper.ExecuteScalarAsync<int>(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    purchaseOrder,
                    mockTransaction.Object,
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()), Times.Once);
        }

        [Fact]
        public async Task UpdatePurchaseOrderAsync_UpdatesPurchaseOrder()
        {
            // Arrange
            var purchaseOrder = new PurchaseOrder { PurchaseOrderId = 1, CustomerId = 2, OrderDateTime = DateTime.Now.AddDays(1), TotalPrice = 250.00m };
            _mockDapperWrapper.Setup(wrapper => wrapper.ExecuteAsync(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    purchaseOrder,
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(1);

            var mockTransaction = new Mock<IDbTransaction>();

            // Act
            await _purchaseOrderRepository.UpdatePurchaseOrderAsync(purchaseOrder, mockTransaction.Object, _mockConnection.Object);

            // Assert
            _mockDapperWrapper.Verify(wrapper => wrapper.ExecuteAsync(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    purchaseOrder,
                    mockTransaction.Object,
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()), Times.Once);
        }

        [Fact]
        public async Task DeletePurchaseOrderAsync_DeletesPurchaseOrder()
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
            await _purchaseOrderRepository.DeletePurchaseOrderAsync(purchaseOrderId, mockTransaction.Object, _mockConnection.Object);

            // Assert
            _mockDapperWrapper.Verify(wrapper => wrapper.ExecuteAsync(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    mockTransaction.Object,
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()), Times.Once);
        }
    }
}
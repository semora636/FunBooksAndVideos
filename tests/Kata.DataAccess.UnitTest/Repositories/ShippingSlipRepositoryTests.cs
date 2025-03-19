using Kata.DataAccess.Repositories;
using Kata.Domain.Entities;
using Moq;
using System.Data;

namespace Kata.DataAccess.UnitTest.Repositories
{
    public class ShippingSlipRepositoryTests
    {
        private readonly Mock<ISqlDataAccess> _mockDataAccess;
        private readonly Mock<IDbConnection> _mockConnection;
        private readonly Mock<IDapperWrapper> _mockDapperWrapper;
        private readonly ShippingSlipRepository _shippingSlipRepository;

        public ShippingSlipRepositoryTests()
        {
            _mockDataAccess = new Mock<ISqlDataAccess>();
            _mockConnection = new Mock<IDbConnection>();
            _mockDapperWrapper = new Mock<IDapperWrapper>();
            _mockDataAccess.Setup(da => da.CreateConnection()).Returns(_mockConnection.Object);
            _shippingSlipRepository = new ShippingSlipRepository(_mockDataAccess.Object, _mockDapperWrapper.Object);
        }

        [Fact]
        public async Task GetShippingSlipsByPurchaseOrderIdAsync_ReturnsShippingSlips()
        {
            // Arrange
            int purchaseOrderId = 1;
            var expectedShippingSlips = new List<ShippingSlip>
            {
                new ShippingSlip { ShippingSlipId = 1, PurchaseOrderId = purchaseOrderId, RecipientAddress = "Address 1" },
                new ShippingSlip { ShippingSlipId = 2, PurchaseOrderId = purchaseOrderId, RecipientAddress = "Address 2" }
            };

            _mockDapperWrapper.Setup(wrapper => wrapper.QueryAsync<ShippingSlip>(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(expectedShippingSlips);

            // Act
            var result = await _shippingSlipRepository.GetShippingSlipsByPurchaseOrderIdAsync(purchaseOrderId);

            // Assert
            Assert.Equal(expectedShippingSlips, result);
            _mockDapperWrapper.Verify(wrapper => wrapper.QueryAsync<ShippingSlip>(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()), Times.Once);
        }

        [Fact]
        public async Task AddShippingSlipAsync_AddsShippingSlip()
        {
            // Arrange
            var shippingSlip = new ShippingSlip { PurchaseOrderId = 1, RecipientAddress = "New Address" };
            int expectedShippingSlipId = 3;

            _mockDapperWrapper.Setup(wrapper => wrapper.ExecuteScalarAsync<int>(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    shippingSlip,
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(expectedShippingSlipId);

            var mockTransaction = new Mock<IDbTransaction>();

            // Act
            await _shippingSlipRepository.AddShippingSlipAsync(shippingSlip, mockTransaction.Object, _mockConnection.Object);

            // Assert
            Assert.Equal(expectedShippingSlipId, shippingSlip.ShippingSlipId);
            _mockDapperWrapper.Verify(wrapper => wrapper.ExecuteScalarAsync<int>(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    shippingSlip,
                    mockTransaction.Object,
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()), Times.Once);
        }
    }
}
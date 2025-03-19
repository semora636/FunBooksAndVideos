using Kata.BusinessLogic.Services;
using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;
using Moq;
using System.Data;

namespace Kata.BusinessLogic.UnitTest.Services
{
    public class ShippingSlipServiceTests
    {
        private readonly Mock<IShippingSlipRepository> _mockShippingSlipRepository;
        private readonly Mock<ICustomerRepository> _mockCustomerRepository;
        private readonly ShippingSlipService _shippingSlipService;
        private readonly Mock<IDbTransaction> _mockTransaction;
        private readonly Mock<IDbConnection> _mockConnection;

        public ShippingSlipServiceTests()
        {
            _mockShippingSlipRepository = new Mock<IShippingSlipRepository>();
            _mockCustomerRepository = new Mock<ICustomerRepository>();
            _mockTransaction = new Mock<IDbTransaction>();
            _mockConnection = new Mock<IDbConnection>();
            _shippingSlipService = new ShippingSlipService(_mockShippingSlipRepository.Object, _mockCustomerRepository.Object);
        }

        [Fact]
        public async Task GetShippingSlipsByPurchaseOrderIdAsync_ReturnsShippingSlips()
        {
            // Arrange
            int purchaseOrderId = 1;
            var expectedShippingSlips = new List<ShippingSlip>
            {
                new ShippingSlip { PurchaseOrderId = purchaseOrderId, RecipientAddress = "101 Elm St" },
                new ShippingSlip { PurchaseOrderId = purchaseOrderId, RecipientAddress = "123 Oak Ave" }
            };
            _mockShippingSlipRepository.Setup(repo => repo.GetShippingSlipsByPurchaseOrderIdAsync(purchaseOrderId)).ReturnsAsync(expectedShippingSlips);

            // Act
            var result = await _shippingSlipService.GetShippingSlipsByPurchaseOrderIdAsync(purchaseOrderId);

            // Assert
            Assert.Equal(expectedShippingSlips, result);
        }

        [Fact]
        public async Task GenerateShippingSlipAsync_AddsShippingSlipAndUpdatesPurchaseOrder()
        {
            // Arrange
            var purchaseOrder = new PurchaseOrder { PurchaseOrderId = 1, CustomerId = 1 };
            var customer = new Customer { CustomerId = 1, Address = "456 Pine Ln", EmailAddress = "test@example.com" };

            _mockCustomerRepository.Setup(repo => repo.GetCustomerByIdAsync(purchaseOrder.CustomerId)).ReturnsAsync(customer);
            _mockShippingSlipRepository.Setup(repo => repo.AddShippingSlipAsync(It.IsAny<ShippingSlip>(), _mockTransaction.Object, _mockConnection.Object)).Returns(Task.CompletedTask);

            // Act
            await _shippingSlipService.GenerateShippingSlipAsync(purchaseOrder, _mockConnection.Object, _mockTransaction.Object);

            // Assert
            _mockCustomerRepository.Verify(repo => repo.GetCustomerByIdAsync(purchaseOrder.CustomerId), Times.Once);
            _mockShippingSlipRepository.Verify(repo => repo.AddShippingSlipAsync(It.Is<ShippingSlip>(s => s.PurchaseOrderId == purchaseOrder.PurchaseOrderId && s.RecipientAddress == customer.Address), _mockTransaction.Object, _mockConnection.Object), Times.Once);
            Assert.NotNull(purchaseOrder.ShippingSlips);
            Assert.Single(purchaseOrder.ShippingSlips);
            Assert.Equal(customer.Address, purchaseOrder.ShippingSlips[0].RecipientAddress);
            Assert.Equal(purchaseOrder.PurchaseOrderId, purchaseOrder.ShippingSlips[0].PurchaseOrderId);
        }

        [Fact]
        public async Task GenerateShippingSlipAsync_DoesNotAddShippingSlip_WhenCustomerNotFound()
        {
            // Arrange
            var purchaseOrder = new PurchaseOrder { PurchaseOrderId = 1, CustomerId = 1 };

            _mockCustomerRepository.Setup(repo => repo.GetCustomerByIdAsync(purchaseOrder.CustomerId)).ReturnsAsync(default(Customer));

            // Act
            await _shippingSlipService.GenerateShippingSlipAsync(purchaseOrder, _mockConnection.Object, _mockTransaction.Object);

            // Assert
            _mockCustomerRepository.Verify(repo => repo.GetCustomerByIdAsync(purchaseOrder.CustomerId), Times.Once);
            _mockShippingSlipRepository.Verify(repo => repo.AddShippingSlipAsync(It.IsAny<ShippingSlip>(), _mockTransaction.Object, _mockConnection.Object), Times.Never);
            Assert.Null(purchaseOrder.ShippingSlips);
        }

        [Fact]
        public async Task GenerateShippingSlipAsync_CreatesShippingSlipListIfNull()
        {
            // Arrange
            var purchaseOrder = new PurchaseOrder { PurchaseOrderId = 1, CustomerId = 1, ShippingSlips = null };
            var customer = new Customer { CustomerId = 1, Address = "456 Pine Ln", EmailAddress = "test@example.com" };

            _mockCustomerRepository.Setup(repo => repo.GetCustomerByIdAsync(purchaseOrder.CustomerId)).ReturnsAsync(customer);
            _mockShippingSlipRepository.Setup(repo => repo.AddShippingSlipAsync(It.IsAny<ShippingSlip>(), _mockTransaction.Object, _mockConnection.Object)).Returns(Task.CompletedTask);

            // Act
            await _shippingSlipService.GenerateShippingSlipAsync(purchaseOrder, _mockConnection.Object, _mockTransaction.Object);

            // Assert
            Assert.NotNull(purchaseOrder.ShippingSlips);
            Assert.Single(purchaseOrder.ShippingSlips);
        }
    }
}
using Kata.BusinessLogic.Interfaces;
using Kata.BusinessLogic.Services;
using Kata.DataAccess;
using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;
using Kata.Domain.Enums;
using Moq;
using System.Data;

namespace Kata.BusinessLogic.UnitTest.Services
{
    public class PurchaseOrderServiceTests
    {
        private readonly Mock<IPurchaseOrderRepository> _mockPurchaseOrderRepository;
        private readonly Mock<IOrderItemRepository> _mockOrderItemRepository;
        private readonly Mock<IShippingSlipService> _mockShippingSlipService;
        private readonly Mock<ISqlDataAccess> _mockDataAccess;
        private readonly PurchaseOrderService _purchaseOrderService;
        private readonly Mock<IDbTransaction> _mockTransaction;
        private readonly Mock<IDbConnection> _mockConnection;
        private readonly Mock<IProductProcessor> _mockMembershipProductProcessor;
        private readonly Mock<IProductProcessor> _mockShippableProductProcessor;
        private readonly IEnumerable<IProductProcessor> _mockProductProcessors;
        private readonly Mock<ITransactionHandler> _mockTransactionHandler;

        public PurchaseOrderServiceTests()
        {
            _mockPurchaseOrderRepository = new Mock<IPurchaseOrderRepository>();
            _mockOrderItemRepository = new Mock<IOrderItemRepository>();
            _mockShippingSlipService = new Mock<IShippingSlipService>();
            
            _mockTransaction = new Mock<IDbTransaction>();
            _mockConnection = new Mock<IDbConnection>();

            _mockConnection.Setup(conn => conn.BeginTransaction()).Returns(_mockTransaction.Object);

            _mockDataAccess = new Mock<ISqlDataAccess>();
            _mockDataAccess.Setup(da => da.CreateConnection()).Returns(_mockConnection.Object);

            _mockMembershipProductProcessor = new Mock<IProductProcessor>();
            _mockShippableProductProcessor = new Mock<IProductProcessor>();

            _mockMembershipProductProcessor.Setup(p => p.ProductType).Returns(ProductType.Membership);
            _mockShippableProductProcessor.Setup(p => p.ProductType).Returns(ProductType.Book);

            _mockProductProcessors = new List<IProductProcessor>
            {
                _mockMembershipProductProcessor.Object,
                _mockShippableProductProcessor.Object
            };

            _mockTransactionHandler = new Mock<ITransactionHandler>();
            _mockTransactionHandler.Setup(th => th.ExecuteTransactionAsync(It.IsAny<Func<IDbTransaction, IDbConnection, Task>>()))
                .Returns((Func<IDbTransaction, IDbConnection, Task> operation) => operation(_mockTransaction.Object, _mockConnection.Object));

            _purchaseOrderService = new PurchaseOrderService(
                _mockPurchaseOrderRepository.Object,
                _mockOrderItemRepository.Object,
                _mockShippingSlipService.Object,
                _mockProductProcessors,
                _mockTransactionHandler.Object);
        }

        [Fact]
        public async Task GetPurchaseOrderByIdAsync_ReturnsPurchaseOrderWithItemsAndShippingSlips()
        {
            // Arrange
            int purchaseOrderId = 1;
            var purchaseOrder = new PurchaseOrder { PurchaseOrderId = purchaseOrderId };
            var orderItems = new List<OrderItem> { new OrderItem { PurchaseOrderId = purchaseOrderId } };
            var shippingSlips = new List<ShippingSlip> { new ShippingSlip { PurchaseOrderId = purchaseOrderId, RecipientAddress = "101 Elm Rd" } };

            _mockPurchaseOrderRepository.Setup(repo => repo.GetPurchaseOrderByIdAsync(purchaseOrderId)).ReturnsAsync(purchaseOrder);
            _mockOrderItemRepository.Setup(service => service.GetOrderItemsByPurchaseOrderIdAsync(purchaseOrderId)).ReturnsAsync(orderItems);
            _mockShippingSlipService.Setup(service => service.GetShippingSlipsByPurchaseOrderIdAsync(purchaseOrderId)).ReturnsAsync(shippingSlips);

            // Act
            var result = await _purchaseOrderService.GetPurchaseOrderByIdAsync(purchaseOrderId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(purchaseOrderId, result.PurchaseOrderId);
            Assert.Equal(orderItems, result.Items);
            Assert.Equal(shippingSlips, result.ShippingSlips);
        }

        [Fact]
        public async Task GetAllPurchaseOrdersAsync_ReturnsAllPurchaseOrdersWithItemsAndShippingSlips()
        {
            // Arrange
            var purchaseOrders = new List<PurchaseOrder> { new PurchaseOrder { PurchaseOrderId = 1 }, new PurchaseOrder { PurchaseOrderId = 2 } };
            var orderItems1 = new List<OrderItem> { new OrderItem { PurchaseOrderId = 1 } };
            var shippingSlips1 = new List<ShippingSlip> { new ShippingSlip { PurchaseOrderId = 1, RecipientAddress = "101 Elm Rd" } };
            var orderItems2 = new List<OrderItem> { new OrderItem { PurchaseOrderId = 2 } };
            var shippingSlips2 = new List<ShippingSlip> { new ShippingSlip { PurchaseOrderId = 2, RecipientAddress = "101 Elm Rd" } };

            _mockPurchaseOrderRepository.Setup(repo => repo.GetAllPurchaseOrdersAsync()).ReturnsAsync(purchaseOrders);
            _mockOrderItemRepository.Setup(service => service.GetOrderItemsByPurchaseOrderIdAsync(1)).ReturnsAsync(orderItems1);
            _mockShippingSlipService.Setup(service => service.GetShippingSlipsByPurchaseOrderIdAsync(1)).ReturnsAsync(shippingSlips1);
            _mockOrderItemRepository.Setup(service => service.GetOrderItemsByPurchaseOrderIdAsync(2)).ReturnsAsync(orderItems2);
            _mockShippingSlipService.Setup(service => service.GetShippingSlipsByPurchaseOrderIdAsync(2)).ReturnsAsync(shippingSlips2);

            // Act
            var result = await _purchaseOrderService.GetAllPurchaseOrdersAsync();

            // Assert
            Assert.Equal(purchaseOrders.Count, result.Count());
            Assert.Equal(orderItems1, result.First().Items);
            Assert.Equal(shippingSlips1, result.First().ShippingSlips);
            Assert.Equal(orderItems2, result.Last().Items);
            Assert.Equal(shippingSlips2, result.Last().ShippingSlips);
        }
        [Fact]
        public async Task AddPurchaseOrderAsync_AddsPurchaseOrderWithItemsAndMembership()
        {
            // Arrange
            var purchaseOrder = new PurchaseOrder
            {
                Items = new List<OrderItem>
                {
                    new OrderItem { ProductType = ProductType.Membership, ProductId = 1, Price = 10, Quantity = 1 }
                }
            };

            _mockPurchaseOrderRepository.Setup(repo => repo.AddPurchaseOrderAsync(It.IsAny<PurchaseOrder>(), _mockTransaction.Object, _mockConnection.Object)).ReturnsAsync(1);
            _mockOrderItemRepository.Setup(service => service.AddOrderItemAsync(It.IsAny<OrderItem>(), _mockTransaction.Object, _mockConnection.Object)).ReturnsAsync(1);
            _mockMembershipProductProcessor.Setup(p => p.ProcessProductAsync(It.IsAny<PurchaseOrder>(), It.IsAny<OrderItem>(), _mockConnection.Object, _mockTransaction.Object)).Returns(Task.CompletedTask);

            // Act
            await _purchaseOrderService.AddPurchaseOrderAsync(purchaseOrder);

            // Assert
            _mockPurchaseOrderRepository.Verify(repo => repo.AddPurchaseOrderAsync(It.IsAny<PurchaseOrder>(), _mockTransaction.Object, _mockConnection.Object), Times.Once);
            _mockOrderItemRepository.Verify(service => service.AddOrderItemAsync(It.IsAny<OrderItem>(), _mockTransaction.Object, _mockConnection.Object), Times.Once);
            _mockMembershipProductProcessor.Verify(p => p.ProcessProductAsync(It.IsAny<PurchaseOrder>(), It.IsAny<OrderItem>(), _mockConnection.Object, _mockTransaction.Object), Times.Once);
            _mockShippableProductProcessor.Verify(p => p.ProcessProductAsync(It.IsAny<PurchaseOrder>(), It.IsAny<OrderItem>(), _mockConnection.Object, _mockTransaction.Object), Times.Never);
        }

        [Fact]
        public async Task AddPurchaseOrderAsync_AddsPurchaseOrderWithItemsAndShippingSlip()
        {
            // Arrange
            var purchaseOrder = new PurchaseOrder
            {
                Items = new List<OrderItem>
                {
                    new OrderItem { ProductType = ProductType.Book, ProductId = 1, Price = 10, Quantity = 1 }
                }
            };

            _mockPurchaseOrderRepository.Setup(repo => repo.AddPurchaseOrderAsync(It.IsAny<PurchaseOrder>(), _mockTransaction.Object, _mockConnection.Object)).ReturnsAsync(1);
            _mockOrderItemRepository.Setup(service => service.AddOrderItemAsync(It.IsAny<OrderItem>(), _mockTransaction.Object, _mockConnection.Object)).ReturnsAsync(1);
            _mockShippableProductProcessor.Setup(p => p.ProcessProductAsync(It.IsAny<PurchaseOrder>(), It.IsAny<OrderItem>(), _mockConnection.Object, _mockTransaction.Object)).Returns(Task.CompletedTask);

            // Act
            await _purchaseOrderService.AddPurchaseOrderAsync(purchaseOrder);

            // Assert
            _mockPurchaseOrderRepository.Verify(repo => repo.AddPurchaseOrderAsync(It.IsAny<PurchaseOrder>(), _mockTransaction.Object, _mockConnection.Object), Times.Once);
            _mockOrderItemRepository.Verify(service => service.AddOrderItemAsync(It.IsAny<OrderItem>(), _mockTransaction.Object, _mockConnection.Object), Times.Once);
            _mockShippableProductProcessor.Verify(p => p.ProcessProductAsync(It.IsAny<PurchaseOrder>(), It.IsAny<OrderItem>(), _mockConnection.Object, _mockTransaction.Object), Times.Once);
            _mockMembershipProductProcessor.Verify(p => p.ProcessProductAsync(It.IsAny<PurchaseOrder>(), It.IsAny<OrderItem>(), _mockConnection.Object, _mockTransaction.Object), Times.Never);
        }

        [Fact]
        public async Task UpdatePurchaseOrderAsync_UpdatesPurchaseOrderAndOrderItems()
        {
            // Arrange
            var purchaseOrder = new PurchaseOrder
            {
                PurchaseOrderId = 1,
                Items = new List<OrderItem> { new OrderItem { PurchaseOrderId = 1 } }
            };

            // Act
            await _purchaseOrderService.UpdatePurchaseOrderAsync(purchaseOrder);

            // Assert
            _mockPurchaseOrderRepository.Verify(repo => repo.UpdatePurchaseOrderAsync(purchaseOrder, _mockTransaction.Object, _mockConnection.Object), Times.Once);
            _mockOrderItemRepository.Verify(service => service.DeleteOrderItemsByPurchaseOrderIdAsync(1, _mockTransaction.Object, _mockConnection.Object), Times.Once);
            _mockOrderItemRepository.Verify(service => service.AddOrderItemAsync(It.IsAny<OrderItem>(), _mockTransaction.Object, _mockConnection.Object), Times.Once);
        }

        [Fact]
        public async Task DeletePurchaseOrderAsync_DeletesPurchaseOrderAndOrderItems()
        {
            // Arrange
            int purchaseOrderId = 1;

            // Act
            await _purchaseOrderService.DeletePurchaseOrderAsync(purchaseOrderId);

            // Assert
            _mockOrderItemRepository.Verify(service => service.DeleteOrderItemsByPurchaseOrderIdAsync(purchaseOrderId, _mockTransaction.Object, _mockConnection.Object), Times.Once);
            _mockPurchaseOrderRepository.Verify(repo => repo.DeletePurchaseOrderAsync(purchaseOrderId, _mockTransaction.Object, _mockConnection.Object), Times.Once);
        }
    }
}
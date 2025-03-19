using Kata.BusinessLogic.Interfaces;
using Kata.Domain.Entities;
using Kata.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Kata.Presentation.UnitTest.Controllers
{
    public class PurchaseOrderControllerTests
    {
        private readonly Mock<IPurchaseOrderService> _mockPurchaseOrderService;
        private readonly Mock<ILogger<PurchaseOrderController>> _mockLogger;
        private readonly PurchaseOrderController _controller;

        public PurchaseOrderControllerTests()
        {
            _mockPurchaseOrderService = new Mock<IPurchaseOrderService>();
            _mockLogger = new Mock<ILogger<PurchaseOrderController>>();
            _controller = new PurchaseOrderController(_mockPurchaseOrderService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllPurchaseOrdersAsync_ReturnsOkWithPurchaseOrders()
        {
            // Arrange
            var purchaseOrders = new List<PurchaseOrder> { new PurchaseOrder { PurchaseOrderId = 1, CustomerId = 1, OrderDateTime = DateTime.Now, TotalPrice = 100.00m } };
            _mockPurchaseOrderService.Setup(service => service.GetAllPurchaseOrdersAsync()).ReturnsAsync(purchaseOrders);

            // Act
            var result = await _controller.GetAllPurchaseOrdersAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedPurchaseOrders = Assert.IsAssignableFrom<IEnumerable<PurchaseOrder>>(okResult.Value);
            Assert.Single(returnedPurchaseOrders);
        }

        [Fact]
        public async Task GetPurchaseOrderByIdAsync_ReturnsOkWithPurchaseOrder_WhenPurchaseOrderExists()
        {
            // Arrange
            var purchaseOrderId = 1;
            var purchaseOrder = new PurchaseOrder { PurchaseOrderId = purchaseOrderId, CustomerId = 1, OrderDateTime = DateTime.Now, TotalPrice = 100.00m };
            _mockPurchaseOrderService.Setup(service => service.GetPurchaseOrderByIdAsync(purchaseOrderId)).ReturnsAsync(purchaseOrder);

            // Act
            var result = await _controller.GetPurchaseOrderByIdAsync(purchaseOrderId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedPurchaseOrder = Assert.IsType<PurchaseOrder>(okResult.Value);
            Assert.Equal(purchaseOrderId, returnedPurchaseOrder.PurchaseOrderId);
        }

        [Fact]
        public async Task GetPurchaseOrderByIdAsync_ReturnsNotFound_WhenPurchaseOrderDoesNotExist()
        {
            // Arrange
            var purchaseOrderId = 1;
            _mockPurchaseOrderService.Setup(service => service.GetPurchaseOrderByIdAsync(purchaseOrderId)).ReturnsAsync(default(PurchaseOrder));

            // Act
            var result = await _controller.GetPurchaseOrderByIdAsync(purchaseOrderId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task AddPurchaseOrderAsync_ReturnsCreatedAtAction()
        {
            // Arrange
            var purchaseOrder = new PurchaseOrder { CustomerId = 1, OrderDateTime = DateTime.Now, TotalPrice = 50.00m };
            _mockPurchaseOrderService.Setup(service => service.AddPurchaseOrderAsync(purchaseOrder)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddPurchaseOrderAsync(purchaseOrder);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(PurchaseOrderController.GetPurchaseOrderByIdAsync), createdAtActionResult.ActionName);

            if (createdAtActionResult.RouteValues != null && createdAtActionResult.RouteValues.ContainsKey("id"))
            {
                if (createdAtActionResult.RouteValues["id"] is int id)
                {
                    Assert.Equal(purchaseOrder.PurchaseOrderId, id);
                }
                else
                {
                    Assert.Fail("Route Value 'id' was not an integer.");
                }
            }
            else
            {
                Assert.Fail("Route value 'id' was null or missing.");
            }

            Assert.Equal(purchaseOrder, createdAtActionResult.Value);
        }

        [Fact]
        public async Task UpdatePurchaseOrderAsync_ReturnsNoContent_WhenUpdateSuccessful()
        {
            // Arrange
            var purchaseOrderId = 1;
            var purchaseOrder = new PurchaseOrder { PurchaseOrderId = purchaseOrderId, CustomerId = 2, OrderDateTime = DateTime.Now.AddDays(1), TotalPrice = 120.00m };
            _mockPurchaseOrderService.Setup(service => service.UpdatePurchaseOrderAsync(purchaseOrder)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdatePurchaseOrderAsync(purchaseOrderId, purchaseOrder);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdatePurchaseOrderAsync_ReturnsBadRequest_WhenIdsDoNotMatch()
        {
            // Arrange
            var purchaseOrderId = 1;
            var purchaseOrder = new PurchaseOrder { PurchaseOrderId = 2, CustomerId = 2, OrderDateTime = DateTime.Now.AddDays(1), TotalPrice = 120.00m };

            // Act
            var result = await _controller.UpdatePurchaseOrderAsync(purchaseOrderId, purchaseOrder);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DeletePurchaseOrderAsync_ReturnsNoContent_WhenDeleteSuccessful()
        {
            // Arrange
            var purchaseOrderId = 1;
            _mockPurchaseOrderService.Setup(service => service.DeletePurchaseOrderAsync(purchaseOrderId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeletePurchaseOrderAsync(purchaseOrderId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
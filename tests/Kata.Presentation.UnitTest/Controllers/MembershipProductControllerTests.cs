﻿using Kata.Domain.Entities;
using Kata.Domain.Enums;
using Kata.Presentation.Controllers;
using Kata.Presentation.Handlers.MembershipProducts;
using Kata.Presentation.Requests.MembershipProducts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Kata.Presentation.UnitTest.Controllers
{
    public class MembershipProductControllerTests
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly Mock<ILogger<MembershipProductController>> _mockLogger;
        private readonly MembershipProductController _controller;

        public MembershipProductControllerTests()
        {
            _mockMediator = new Mock<IMediator>();
            _mockLogger = new Mock<ILogger<MembershipProductController>>();
            _controller = new MembershipProductController(_mockMediator.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllMembershipProductsAsync_ReturnsOkWithMembershipProducts()
        {
            // Arrange
            var membershipProducts = new List<MembershipProduct> { new MembershipProduct { MembershipProductId = 1, Name = "Premium Membership", MembershipType = MembershipType.Premium, Price = 99.99m, DurationMonths = 12 } };
            _mockMediator.Setup(service => service.Send(It.IsAny<GetAllMembershipProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(membershipProducts);

            // Act
            var result = await _controller.GetAllMembershipProductsAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedMembershipProducts = Assert.IsAssignableFrom<IEnumerable<MembershipProduct>>(okResult.Value);
            Assert.Single(returnedMembershipProducts);
        }

        [Fact]
        public async Task GetMembershipProductByIdAsync_ReturnsOkWithMembershipProduct_WhenMembershipProductExists()
        {
            // Arrange
            var membershipProductId = 1;
            var membershipProduct = new MembershipProduct { MembershipProductId = membershipProductId, Name = "Premium Membership", MembershipType = MembershipType.Premium, Price = 99.99m, DurationMonths = 12 };
            _mockMediator.Setup(service => service.Send(It.IsAny<GetMembershipProductByIdRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(membershipProduct);

            // Act
            var result = await _controller.GetMembershipProductByIdAsync(membershipProductId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedMembershipProduct = Assert.IsType<MembershipProduct>(okResult.Value);
            Assert.Equal(membershipProductId, returnedMembershipProduct.MembershipProductId);
        }

        [Fact]
        public async Task GetMembershipProductByIdAsync_ReturnsNotFound_WhenMembershipProductDoesNotExist()
        {
            // Arrange
            var membershipProductId = 1;
            _mockMediator.Setup(service => service.Send(It.IsAny<GetMembershipProductByIdRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(default(MembershipProduct));

            // Act
            var result = await _controller.GetMembershipProductByIdAsync(membershipProductId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task AddMembershipProductAsync_ReturnsCreatedAtAction()
        {
            // Arrange
            var membershipProduct = new MembershipProduct { Name = "Book Membership", MembershipType = MembershipType.BookClub, Price = 49.99m, DurationMonths = 6 };
            _mockMediator.Setup(service => service.Send(It.IsAny<AddMembershipProductHandler>(), It.IsAny<CancellationToken>())).ReturnsAsync(membershipProduct);

            // Act
            var result = await _controller.AddMembershipProductAsync(membershipProduct);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(MembershipProductController.GetMembershipProductByIdAsync), createdAtActionResult.ActionName);

            if (createdAtActionResult.RouteValues != null && createdAtActionResult.RouteValues.ContainsKey("id"))
            {
                if (createdAtActionResult.RouteValues["id"] is int id)
                {
                    Assert.Equal(membershipProduct.MembershipProductId, id);
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

            Assert.Equal(membershipProduct, createdAtActionResult.Value);
        }

        [Fact]
        public async Task UpdateMembershipProductAsync_ReturnsNoContent_WhenUpdateSuccessful()
        {
            // Arrange
            var membershipProductId = 1;
            var membershipProduct = new MembershipProduct { MembershipProductId = membershipProductId, Name = "Updated Premium Membership", MembershipType = MembershipType.Premium, Price = 109.99m, DurationMonths = 12 };
            _mockMediator.Setup(service => service.Send(It.IsAny<UpdateMembershipProductRequest>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateMembershipProductAsync(membershipProductId, membershipProduct);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateMembershipProductAsync_ReturnsBadRequest_WhenIdsDoNotMatch()
        {
            // Arrange
            var membershipProductId = 1;
            var membershipProduct = new MembershipProduct { MembershipProductId = 2, Name = "Updated Premium Membership", MembershipType = MembershipType.Premium, Price = 109.99m, DurationMonths = 12 };

            // Act
            var result = await _controller.UpdateMembershipProductAsync(membershipProductId, membershipProduct);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DeleteMembershipProductAsync_ReturnsNoContent_WhenDeleteSuccessful()
        {
            // Arrange
            var membershipProductId = 1;
            _mockMediator.Setup(service => service.Send(It.IsAny<DeleteMembershipProductRequest>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteMembershipProductAsync(membershipProductId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}

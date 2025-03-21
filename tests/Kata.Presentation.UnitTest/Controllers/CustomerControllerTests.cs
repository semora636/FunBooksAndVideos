using Kata.BusinessLogic.Interfaces;
using Kata.Domain.Entities;
using Kata.Domain.Enums;
using Kata.Presentation.Controllers;
using Kata.Presentation.Requests.Customers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Kata.Presentation.UnitTest.Controllers
{
    public class CustomerControllerTests
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly Mock<ILogger<CustomerController>> _mockLogger;
        private readonly CustomerController _controller;

        public CustomerControllerTests()
        {
            _mockMediator = new Mock<IMediator>();
            _mockLogger = new Mock<ILogger<CustomerController>>();
            _controller = new CustomerController(_mockMediator.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllCustomersAsync_ReturnsOkWithCustomers()
        {
            // Arrange
            var customers = new List<Customer> { new Customer { CustomerId = 1, FirstName = "John", LastName = "Doe", EmailAddress = "john.doe@example.com", Address = "123 Main St" } };
            _mockMediator.Setup(service => service.Send(It.IsAny<GetAllCustomersRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(customers);

            // Act
            var result = await _controller.GetAllCustomersAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCustomers = Assert.IsAssignableFrom<IEnumerable<Customer>>(okResult.Value);
            Assert.Single(returnedCustomers);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ReturnsOkWithCustomer_WhenCustomerExists()
        {
            // Arrange
            var customerId = 1;
            var customer = new Customer { CustomerId = customerId, FirstName = "John", LastName = "Doe", EmailAddress = "john.doe@example.com", Address = "123 Main St" };
            _mockMediator.Setup(service => service.Send(It.IsAny<GetCustomerByIdRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(customer);

            // Act
            var result = await _controller.GetCustomerByIdAsync(customerId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCustomer = Assert.IsType<Customer>(okResult.Value);
            Assert.Equal(customerId, returnedCustomer.CustomerId);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ReturnsNotFound_WhenCustomerDoesNotExist()
        {
            // Arrange
            var customerId = 1;
            _mockMediator.Setup(service => service.Send(It.IsAny<GetCustomerByIdRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(default(Customer));

            // Act
            var result = await _controller.GetCustomerByIdAsync(customerId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetMembershipsByCustomerIdAsync_ReturnsOkWithMemberships()
        {
            // Arrange
            var customerId = 1;
            var memberships = new List<Membership> { new Membership { MembershipId = 1, CustomerId = customerId, MembershipType = MembershipType.Premium, ActivationDateTime = DateTime.Now, ExpirationDateTime = DateTime.Now.AddMonths(1) } };
            _mockMediator.Setup(service => service.Send(It.IsAny<GetMembershipsByCustomerIdRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(memberships);

            // Act
            var result = await _controller.GetMembershipsByCustomerIdAsync(customerId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedMemberships = Assert.IsAssignableFrom<IEnumerable<Membership>>(okResult.Value);
            Assert.Single(returnedMemberships);
        }

        [Fact]
        public async Task AddCustomerAsync_ReturnsCreatedAtAction()
        {
            // Arrange
            var customer = new Customer { FirstName = "Jane", LastName = "Smith", EmailAddress = "jane.smith@example.com", Address = "456 Oak Ave" };
            _mockMediator.Setup(service => service.Send(It.IsAny<AddCustomerRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(customer);

            // Act
            var result = await _controller.AddCustomerAsync(customer);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(CustomerController.GetCustomerByIdAsync), createdAtActionResult.ActionName);

            if (createdAtActionResult.RouteValues != null && createdAtActionResult.RouteValues.ContainsKey("id"))
            {
                if (createdAtActionResult.RouteValues["id"] is int id)
                {
                    Assert.Equal(customer.CustomerId, id);
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

            Assert.Equal(customer, createdAtActionResult.Value);
        }

        [Fact]
        public async Task UpdateCustomerAsync_ReturnsNoContent_WhenUpdateSuccessful()
        {
            // Arrange
            var customerId = 1;
            var customer = new Customer { CustomerId = customerId, FirstName = "Updated John", LastName = "Updated Doe", EmailAddress = "updated.john.doe@example.com", Address = "Updated 123 Main St" };
            _mockMediator.Setup(service => service.Send(It.IsAny<UpdateCustomerRequest>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateCustomerAsync(customerId, customer);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateCustomerAsync_ReturnsBadRequest_WhenIdsDoNotMatch()
        {
            // Arrange
            var customerId = 1;
            var customer = new Customer { CustomerId = 2, FirstName = "Updated John", LastName = "Updated Doe", EmailAddress = "updated.john.doe@example.com", Address = "Updated 123 Main St" };

            // Act
            var result = await _controller.UpdateCustomerAsync(customerId, customer);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DeleteCustomerAsync_ReturnsNoContent_WhenDeleteSuccessful()
        {
            // Arrange
            var customerId = 1;
            _mockMediator.Setup(service => service.Send(It.IsAny<DeleteCustomerRequest>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteCustomer(customerId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}

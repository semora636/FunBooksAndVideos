using Kata.BusinessLogic.Services;
using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;
using Moq;

namespace Kata.BusinessLogic.UnitTest.Services
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _mockCustomerRepository;
        private readonly CustomerService _customerService;

        public CustomerServiceTests()
        {
            _mockCustomerRepository = new Mock<ICustomerRepository>();
            _customerService = new CustomerService(_mockCustomerRepository.Object);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ReturnsCustomer_WhenCustomerExists()
        {
            // Arrange
            int customerId = 1;
            var expectedCustomer = new Customer
            {
                CustomerId = customerId,
                FirstName = "John",
                LastName = "Doe",
                EmailAddress = "john.doe@example.com",
                Address = "123 Main St"
            };
            _mockCustomerRepository.Setup(repo => repo.GetCustomerByIdAsync(customerId)).ReturnsAsync(expectedCustomer);

            // Act
            var result = await _customerService.GetCustomerByIdAsync(customerId);

            // Assert
            Assert.Equal(expectedCustomer, result);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ReturnsNull_WhenCustomerDoesNotExist()
        {
            // Arrange
            int customerId = 1;
            _mockCustomerRepository.Setup(repo => repo.GetCustomerByIdAsync(customerId)).ReturnsAsync(default(Customer));

            // Act
            var result = await _customerService.GetCustomerByIdAsync(customerId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllCustomersAsync_ReturnsAllCustomers()
        {
            // Arrange
            var expectedCustomers = new List<Customer>
            {
                new Customer
                {
                    CustomerId = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    EmailAddress = "john.doe@example.com",
                    Address = "123 Main St"
                },
                new Customer
                {
                    CustomerId = 2,
                    FirstName = "Jane",
                    LastName = "Smith",
                    EmailAddress = "jane.smith@example.com",
                    Address = "456 Oak Ave"
                }
            };
            _mockCustomerRepository.Setup(repo => repo.GetAllCustomersAsync()).ReturnsAsync(expectedCustomers);

            // Act
            var result = await _customerService.GetAllCustomersAsync();

            // Assert
            Assert.Equal(expectedCustomers, result);
        }

        [Fact]
        public async Task AddCustomerAsync_CallsRepositoryAddCustomerAsync()
        {
            // Arrange
            var customer = new Customer
            {
                CustomerId = 1,
                FirstName = "New",
                LastName = "Customer",
                EmailAddress = "new.customer@example.com",
                Address = "789 Pine Ln"
            };

            // Act
            await _customerService.AddCustomerAsync(customer);

            // Assert
            _mockCustomerRepository.Verify(repo => repo.AddCustomerAsync(customer), Times.Once);
        }

        [Fact]
        public async Task UpdateCustomerAsync_CallsRepositoryUpdateCustomerAsync()
        {
            // Arrange
            var customer = new Customer
            {
                CustomerId = 1,
                FirstName = "Updated",
                LastName = "Customer",
                EmailAddress = "updated.customer@example.com",
                Address = "101 Elm Rd"
            };

            // Act
            await _customerService.UpdateCustomerAsync(customer);

            // Assert
            _mockCustomerRepository.Verify(repo => repo.UpdateCustomerAsync(customer), Times.Once);
        }

        [Fact]
        public async Task DeleteCustomerAsync_CallsRepositoryDeleteCustomerAsync()
        {
            // Arrange
            int customerId = 1;

            // Act
            await _customerService.DeleteCustomerAsync(customerId);

            // Assert
            _mockCustomerRepository.Verify(repo => repo.DeleteCustomerAsync(customerId), Times.Once);
        }
    }
}
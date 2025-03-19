using Kata.DataAccess.Repositories;
using Kata.Domain.Entities;
using Moq;
using System.Data;

namespace Kata.DataAccess.UnitTest.Repositories
{
    public class CustomerRepositoryTests
    {
        private readonly Mock<ISqlDataAccess> _mockDataAccess;
        private readonly Mock<IDbConnection> _mockConnection;
        private readonly Mock<IDapperWrapper> _mockDapperWrapper;
        private readonly CustomerRepository _customerRepository;

        public CustomerRepositoryTests()
        {
            _mockDataAccess = new Mock<ISqlDataAccess>();
            _mockConnection = new Mock<IDbConnection>();
            _mockDapperWrapper = new Mock<IDapperWrapper>();
            _mockDataAccess.Setup(da => da.CreateConnection()).Returns(_mockConnection.Object);
            _customerRepository = new CustomerRepository(_mockDataAccess.Object, _mockDapperWrapper.Object);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ReturnsCustomer()
        {
            // Arrange
            int customerId = 1;
            var expectedCustomer = new Customer { CustomerId = customerId, FirstName = "John", LastName = "Doe", EmailAddress = "john.doe@example.com", Address = "123 Main St" };
            _mockDapperWrapper.Setup(wrapper => wrapper.QueryFirstOrDefaultAsync<Customer>(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(expectedCustomer);

            // Act
            var result = await _customerRepository.GetCustomerByIdAsync(customerId);

            // Assert
            Assert.Equal(expectedCustomer, result);
            _mockDapperWrapper.Verify(wrapper => wrapper.QueryFirstOrDefaultAsync<Customer>(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()), Times.Once);
        }

        [Fact]
        public async Task GetAllCustomersAsync_ReturnsAllCustomers()
        {
            // Arrange
            var expectedCustomers = new List<Customer>
            {
                new Customer { CustomerId = 1, FirstName = "John", LastName = "Doe", EmailAddress = "john.doe@example.com", Address = "123 Main St" },
                new Customer { CustomerId = 2, FirstName = "Jane", LastName = "Smith", EmailAddress = "jane.smith@example.com", Address = "456 Oak Ave" }
            };
            _mockDapperWrapper.Setup(wrapper => wrapper.QueryAsync<Customer>(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(expectedCustomers);

            // Act
            var result = await _customerRepository.GetAllCustomersAsync();

            // Assert
            Assert.Equal(expectedCustomers, result);
            _mockDapperWrapper.Verify(wrapper => wrapper.QueryAsync<Customer>(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()), Times.Once);
        }

        [Fact]
        public async Task AddCustomerAsync_AddsCustomer()
        {
            // Arrange
            var customer = new Customer { FirstName = "New", LastName = "Customer", EmailAddress = "new.customer@example.com", Address = "789 Pine Ln" };
            int expectedCustomerId = 3;

            _mockDapperWrapper.Setup(wrapper => wrapper.ExecuteScalarAsync<int>(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    customer,
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(expectedCustomerId);

            // Act
            await _customerRepository.AddCustomerAsync(customer);

            // Assert
            Assert.Equal(expectedCustomerId, customer.CustomerId);
            _mockDapperWrapper.Verify(wrapper => wrapper.ExecuteScalarAsync<int>(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    customer,
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()), Times.Once);
        }

        [Fact]
        public async Task UpdateCustomerAsync_UpdatesCustomer()
        {
            // Arrange
            var customer = new Customer { CustomerId = 1, FirstName = "Updated", LastName = "Customer", EmailAddress = "updated.customer@example.com", Address = "1011 Elm St" };
            _mockDapperWrapper.Setup(wrapper => wrapper.ExecuteAsync(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    customer,
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(1);

            // Act
            await _customerRepository.UpdateCustomerAsync(customer);

            // Assert
            _mockDapperWrapper.Verify(wrapper => wrapper.ExecuteAsync(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    customer,
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()), Times.Once);
        }

        [Fact]
        public async Task DeleteCustomerAsync_DeletesCustomer()
        {
            // Arrange
            int customerId = 1;
            _mockDapperWrapper.Setup(wrapper => wrapper.ExecuteAsync(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()))
                .ReturnsAsync(1);

            // Act
            await _customerRepository.DeleteCustomerAsync(customerId);

            // Assert
            _mockDapperWrapper.Verify(wrapper => wrapper.ExecuteAsync(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(),
                    It.IsAny<int?>(),
                    It.IsAny<CommandType?>()), Times.Once);
        }
    }
}
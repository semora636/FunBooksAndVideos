using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;

namespace Kata.DataAccess.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ISqlDataAccess _dataAccess;
        private readonly IDapperWrapper _dapperWrapper;

        public CustomerRepository(ISqlDataAccess dataAccess, IDapperWrapper dapperWrapper)
        {
            _dataAccess = dataAccess;
            _dapperWrapper = dapperWrapper;
        }

        public async Task<Customer?> GetCustomerByIdAsync(int customerId)
        {
            using var connection = _dataAccess.CreateConnection();
            return await _dapperWrapper.QueryFirstOrDefaultAsync<Customer>(connection, "SELECT * FROM Customers WHERE CustomerId = @CustomerId", new { CustomerId = customerId });
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            using var connection = _dataAccess.CreateConnection();
            return await _dapperWrapper.QueryAsync<Customer>(connection, "SELECT * FROM Customers");
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            using var connection = _dataAccess.CreateConnection();
            customer.CustomerId = await _dapperWrapper.ExecuteScalarAsync<int>(connection, "INSERT INTO Customers (FirstName, LastName, EmailAddress, Address) VALUES (@FirstName, @LastName, @EmailAddress, @Address); SELECT SCOPE_IDENTITY();", customer);
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            using var connection = _dataAccess.CreateConnection();
            await _dapperWrapper.ExecuteAsync(connection, "UPDATE Customers SET FirstName = @FirstName, LastName = @LastName, EmailAddress = @EmailAddress, Address = @Address WHERE CustomerId = @CustomerId", customer);
        }

        public async Task DeleteCustomerAsync(int customerId)
        {
            using var connection = _dataAccess.CreateConnection();
            await _dapperWrapper.ExecuteAsync(connection, "DELETE FROM Customers WHERE CustomerId = @CustomerId", new { CustomerId = customerId });
        }
    }
}

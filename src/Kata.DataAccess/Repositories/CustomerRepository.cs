using Dapper;
using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;

namespace Kata.DataAccess.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ISqlDataAccess _dataAccess;

        public CustomerRepository(ISqlDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<Customer?> GetCustomerByIdAsync(int customerId)
        {
            using var connection = _dataAccess.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Customer>("SELECT * FROM Customers WHERE CustomerId = @CustomerId", new { CustomerId = customerId });
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            using var connection = _dataAccess.CreateConnection();
            return await connection.QueryAsync<Customer>("SELECT * FROM Customers");
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            using var connection = _dataAccess.CreateConnection();
            customer.CustomerId = await connection.ExecuteScalarAsync<int>("INSERT INTO Customers (FirstName, LastName, EmailAddress, Address) VALUES (@FirstName, @LastName, @EmailAddress, @Address); SELECT SCOPE_IDENTITY();", customer);
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            using var connection = _dataAccess.CreateConnection();
            await connection.ExecuteAsync("UPDATE Customers SET FirstName = @FirstName, LastName = @LastName, EmailAddress = @EmailAddress, Address = @Address WHERE CustomerId = @CustomerId", customer);
        }

        public async Task DeleteCustomerAsync(int customerId)
        {
            using var connection = _dataAccess.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM Customers WHERE CustomerId = @CustomerId", new { CustomerId = customerId });
        }
    }
}

using Dapper;
using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;

namespace Kata.DataAccess.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly SqlDataAccess _dataAccess;

        public CustomerRepository(SqlDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public Customer? GetCustomerById(int customerId)
        {
            using var connection = _dataAccess.CreateConnection();
            return connection.QueryFirstOrDefault<Customer>("SELECT * FROM Customers WHERE CustomerId = @CustomerId", new { CustomerId = customerId });
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            using var connection = _dataAccess.CreateConnection();
            return connection.Query<Customer>("SELECT * FROM Customers");
        }

        public void AddCustomer(Customer customer)
        {
            using var connection = _dataAccess.CreateConnection();
            customer.CustomerId = connection.ExecuteScalar<int>("INSERT INTO Customers (FirstName, LastName, EmailAddress, Address) VALUES (@FirstName, @LastName, @EmailAddress, @Address); SELECT SCOPE_IDENTITY();", customer);
        }

        public void UpdateCustomer(Customer customer)
        {
            using var connection = _dataAccess.CreateConnection();
            connection.Execute("UPDATE Customers SET FirstName = @FirstName, LastName = @LastName, EmailAddress = @EmailAddress, Address = @Address WHERE CustomerId = @CustomerId", customer);
        }

        public void DeleteCustomer(int customerId)
        {
            using var connection = _dataAccess.CreateConnection();
            connection.Execute("DELETE FROM Customers WHERE CustomerId = @CustomerId", new { CustomerId = customerId });
        }
    }
}

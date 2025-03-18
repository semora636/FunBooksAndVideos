using Kata.Domain.Entities;

namespace Kata.DataAccess.Interfaces
{
    public interface ICustomerRepository
    {
        Customer? GetCustomerById(int customerId);
        IEnumerable<Customer> GetAllCustomers();
        void AddCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(int customerId);
    }
}

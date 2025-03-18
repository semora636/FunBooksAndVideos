using Kata.Domain.Entities;

namespace Kata.BusinessLogic.Interfaces
{
    public interface ICustomerService
    {
        Customer? GetCustomerById(int customerId);
        IEnumerable<Customer> GetAllCustomers();
        void AddCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(int customerId);
    }
}

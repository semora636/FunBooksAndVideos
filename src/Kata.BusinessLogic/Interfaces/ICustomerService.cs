using Kata.Domain.Entities;

namespace Kata.BusinessLogic.Interfaces
{
    public interface ICustomerService
    {
        Task<Customer?> GetCustomerByIdAsync(int customerId);
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task AddCustomerAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(int customerId);
    }
}

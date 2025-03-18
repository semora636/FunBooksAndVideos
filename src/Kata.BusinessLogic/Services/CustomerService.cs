using Kata.BusinessLogic.Interfaces;
using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;

namespace Kata.BusinessLogic.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public Customer? GetCustomerById(int customerId)
        {
            return _customerRepository.GetCustomerById(customerId);
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return _customerRepository.GetAllCustomers();
        }

        public void AddCustomer(Customer customer)
        {
            _customerRepository.AddCustomer(customer);
        }

        public void UpdateCustomer(Customer customer)
        {
            _customerRepository.UpdateCustomer(customer);
        }

        public void DeleteCustomer(int customerId)
        {
            _customerRepository.DeleteCustomer(customerId);
        }
    }
}

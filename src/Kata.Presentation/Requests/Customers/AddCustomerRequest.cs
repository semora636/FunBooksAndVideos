using Kata.Domain.Entities;
using MediatR;

namespace Kata.Presentation.Requests.Customers
{
    public class AddCustomerRequest : IRequest<Customer>
    {
        public Customer Customer { get; set; }

        public AddCustomerRequest(Customer customer)
        {
            Customer = customer;
        }
    }
}

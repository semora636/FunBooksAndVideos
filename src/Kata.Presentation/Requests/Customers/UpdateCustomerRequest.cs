using Kata.Domain.Entities;
using MediatR;

namespace Kata.Presentation.Requests.Customers
{
    public class UpdateCustomerRequest : IRequest
    {
        public Customer Customer { get; set; }

        public UpdateCustomerRequest(Customer customer)
        {
            Customer = customer;
        }
    }
}

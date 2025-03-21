using Kata.Domain.Entities;
using MediatR;

namespace Kata.Presentation.Requests.Customers
{
    public class UpdateCustomerRequest : IRequest
    {
        public int Id { get; set; }
        public Customer Customer { get; set; }
    }
}

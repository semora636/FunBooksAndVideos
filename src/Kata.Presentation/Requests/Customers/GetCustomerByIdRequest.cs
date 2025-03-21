using Kata.Domain.Entities;
using MediatR;

namespace Kata.Presentation.Requests.Customers
{
    public class GetCustomerByIdRequest : IRequest<Customer?>
    {
        public int Id { get; set; }
    }
}

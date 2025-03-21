using Kata.Domain.Entities;
using MediatR;

namespace Kata.Presentation.Requests.Customers
{
    public class GetAllCustomersRequest : IRequest<IEnumerable<Customer>>
    {
    }
}

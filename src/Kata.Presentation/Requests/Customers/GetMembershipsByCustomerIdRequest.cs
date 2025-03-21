using Kata.Domain.Entities;
using MediatR;

namespace Kata.Presentation.Requests.Customers
{
    public class GetMembershipsByCustomerIdRequest : IRequest<IEnumerable<Membership>>
    {
        public int Id { get; set; }
    }
}

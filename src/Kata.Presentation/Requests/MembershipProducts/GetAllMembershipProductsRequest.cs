using Kata.Domain.Entities;
using MediatR;

namespace Kata.Presentation.Requests.MembershipProducts
{
    public class GetAllMembershipProductsRequest : IRequest<IEnumerable<MembershipProduct>>
    {
    }
}

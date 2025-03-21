using Kata.Domain.Entities;
using MediatR;

namespace Kata.Presentation.Requests.MembershipProducts
{
    public class GetMembershipProductByIdRequest : IRequest<MembershipProduct?>
    {
        public int Id { get; set; }
    }
}

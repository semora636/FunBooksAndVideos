using Kata.Domain.Entities;
using MediatR;

namespace Kata.Presentation.Requests.MembershipProducts
{
    public class AddMembershipProductRequest : IRequest<MembershipProduct>
    {
        public MembershipProduct MembershipProduct { get; set; }
    }
}

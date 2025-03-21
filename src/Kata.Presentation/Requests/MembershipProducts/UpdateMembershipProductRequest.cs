using Kata.Domain.Entities;
using MediatR;

namespace Kata.Presentation.Requests.MembershipProducts
{
    public class UpdateMembershipProductRequest : IRequest
    {
        public int Id { get; set; }
        public MembershipProduct MembershipProduct { get; set; }
    }
}

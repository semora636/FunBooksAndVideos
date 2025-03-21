using MediatR;

namespace Kata.Presentation.Requests.MembershipProducts
{
    public class DeleteMembershipProductRequest : IRequest
    {
        public int Id { get; set; }
    }
}

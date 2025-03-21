using Kata.Domain.Entities;
using MediatR;

namespace Kata.Presentation.Requests.MembershipProducts
{
    public class UpdateMembershipProductRequest : IRequest
    {
        public MembershipProduct MembershipProduct { get; set; }

        public UpdateMembershipProductRequest(MembershipProduct membershipProduct)
        {
            MembershipProduct = membershipProduct;
        }
    }
}

using Kata.BusinessLogic.Interfaces;
using Kata.Domain.Entities;
using Kata.Presentation.Requests.MembershipProducts;
using MediatR;

namespace Kata.Presentation.Handlers.MembershipProducts
{
    public class AddMembershipProductHandler : IRequestHandler<AddMembershipProductRequest, MembershipProduct>
    {
        private readonly IMembershipProductService _membershipProductService;

        public AddMembershipProductHandler(IMembershipProductService membershipProductService)
        {
            _membershipProductService = membershipProductService;
        }

        public async Task<MembershipProduct> Handle(AddMembershipProductRequest request, CancellationToken cancellationToken)
        {
            await _membershipProductService.AddMembershipProductAsync(request.MembershipProduct);
            return request.MembershipProduct;
        }
    }
}

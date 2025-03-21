using Kata.BusinessLogic.Interfaces;
using Kata.Domain.Entities;
using Kata.Presentation.Requests.MembershipProducts;
using MediatR;

namespace Kata.Presentation.Handlers.MembershipProducts
{
    public class GetAllMembershipProductsHandler : IRequestHandler<GetAllMembershipProductsRequest, IEnumerable<MembershipProduct>>
    {
        private readonly IMembershipProductService _membershipProductService;

        public GetAllMembershipProductsHandler(IMembershipProductService membershipProductService)
        {
            _membershipProductService = membershipProductService;
        }

        public async Task<IEnumerable<MembershipProduct>> Handle(GetAllMembershipProductsRequest request, CancellationToken cancellationToken)
        {
            return await _membershipProductService.GetAllMembershipProductsAsync();
        }
    }
}

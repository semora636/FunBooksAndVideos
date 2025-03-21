using Kata.BusinessLogic.Interfaces;
using Kata.Domain.Entities;
using Kata.Presentation.Requests.MembershipProducts;
using MediatR;

namespace Kata.Presentation.Handlers.MembershipProducts
{
    public class GetMembershipProductByIdHandler : IRequestHandler<GetMembershipProductByIdRequest, MembershipProduct?>
    {
        private readonly IMembershipProductService _membershipProductService;

        public GetMembershipProductByIdHandler(IMembershipProductService membershipProductService)
        {
            _membershipProductService = membershipProductService;
        }

        public async Task<MembershipProduct?> Handle(GetMembershipProductByIdRequest request, CancellationToken cancellationToken)
        {
            return await _membershipProductService.GetMembershipProductByIdAsync(request.Id);
        }
    }
}

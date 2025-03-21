using Kata.BusinessLogic.Interfaces;
using Kata.Presentation.Requests.MembershipProducts;
using MediatR;

namespace Kata.Presentation.Handlers.MembershipProducts
{
    public class UpdateMembershipProductHandler : IRequestHandler<UpdateMembershipProductRequest>
    {
        private readonly IMembershipProductService _membershipProductService;

        public UpdateMembershipProductHandler(IMembershipProductService membershipProductService)
        {
            _membershipProductService = membershipProductService;
        }

        public async Task Handle(UpdateMembershipProductRequest request, CancellationToken cancellationToken)
        {
            await _membershipProductService.UpdateMembershipProductAsync(request.MembershipProduct);
        }
    }
}

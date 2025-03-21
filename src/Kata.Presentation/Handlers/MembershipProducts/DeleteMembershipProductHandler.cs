using Kata.BusinessLogic.Interfaces;
using Kata.Presentation.Requests.MembershipProducts;
using MediatR;

namespace Kata.Presentation.Handlers.MembershipProducts
{
    public class DeleteMembershipProductHandler : IRequestHandler<DeleteMembershipProductRequest>
    {
        private readonly IMembershipProductService _membershipProductService;

        public DeleteMembershipProductHandler(IMembershipProductService membershipProductService)
        {
            _membershipProductService = membershipProductService;
        }

        public async Task Handle(DeleteMembershipProductRequest request, CancellationToken cancellationToken)
        {
            await _membershipProductService.DeleteMembershipProductAsync(request.Id);
        }
    }
}

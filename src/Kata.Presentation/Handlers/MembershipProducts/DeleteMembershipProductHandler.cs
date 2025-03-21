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
            _ = await _membershipProductService.GetMembershipProductByIdAsync(request.Id) ?? throw new KeyNotFoundException($"MembershipProduct with ID {request.Id} not found.");
            await _membershipProductService.DeleteMembershipProductAsync(request.Id);
        }
    }
}

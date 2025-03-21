using Kata.BusinessLogic.Interfaces;
using Kata.Domain.Entities;
using Kata.Presentation.Requests.Customers;
using MediatR;

namespace Kata.Presentation.Handlers.Customers
{
    public class GetMembershipsByCustomerIdHandler : IRequestHandler<GetMembershipsByCustomerIdRequest, IEnumerable<Membership>>
    {
        private readonly IMembershipService _membershipService;

        public GetMembershipsByCustomerIdHandler(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        public async Task<IEnumerable<Membership>> Handle(GetMembershipsByCustomerIdRequest request, CancellationToken cancellationToken)
        {
            return await _membershipService.GetMembershipsByCustomerIdAsync(request.Id);
        }
    }
}

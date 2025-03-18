using Kata.BusinessLogic.Interfaces;
using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;

namespace Kata.BusinessLogic.Services
{
    internal class MembershipService : IMembershipService
    {
        private readonly IMembershipRepository _membershipRepository;

        public MembershipService(IMembershipRepository membershipRepository)
        {
            _membershipRepository = membershipRepository;
        }

        public IEnumerable<Membership> GetMembershipsByCustomerId(int customerId)
        {
            return _membershipRepository.GetMembershipsByCustomer(customerId);
        }
    }
}

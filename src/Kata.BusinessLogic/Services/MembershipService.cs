using Kata.BusinessLogic.Interfaces;
using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;
using Microsoft.Data.SqlClient;

namespace Kata.BusinessLogic.Services
{
    internal class MembershipService : IMembershipService
    {
        private readonly IMembershipRepository _membershipRepository;
        private readonly IMembershipProductRepository _membershipProductRepository;

        public MembershipService(IMembershipRepository membershipRepository, IMembershipProductRepository membershipProductRepository)
        {
            _membershipRepository = membershipRepository;
            _membershipProductRepository = membershipProductRepository;
        }

        public IEnumerable<Membership> GetMembershipsByCustomerId(int customerId)
        {
            return _membershipRepository.GetMembershipsByCustomer(customerId);
        }

        public void ActivateMembership(PurchaseOrder purchaseOrder, SqlConnection connection, SqlTransaction transaction, OrderItem item)
        {
            // TODO: We can check if the user already has an active memvbership and in this case just increase the
            // existing expiration, or add the duration on top of the existing one
            var membershipProduct = _membershipProductRepository.GetMembershipProductById(item.ProductId);
            if (membershipProduct != null)
            {
                var membership = new Membership()
                {
                    ActivationDateTime = DateTime.UtcNow,
                    ExpirationDateTime = DateTime.UtcNow.AddMonths(membershipProduct.DurationMonths),
                    CustomerId = purchaseOrder.CustomerId,
                    MembershipType = membershipProduct.MembershipType
                };
                _membershipRepository.AddMembership(membership, transaction, connection);
            }
        }
    }
}

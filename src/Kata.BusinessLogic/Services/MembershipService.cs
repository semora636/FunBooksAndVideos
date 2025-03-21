﻿using Kata.BusinessLogic.Interfaces;
using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;
using System.Data;

namespace Kata.BusinessLogic.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly IMembershipRepository _membershipRepository;
        private readonly IMembershipProductRepository _membershipProductRepository;

        public MembershipService(IMembershipRepository membershipRepository, IMembershipProductRepository membershipProductRepository)
        {
            _membershipRepository = membershipRepository;
            _membershipProductRepository = membershipProductRepository;
        }

        public async Task<IEnumerable<Membership>> GetMembershipsByCustomerIdAsync(int customerId)
        {
            return await _membershipRepository.GetMembershipsByCustomerAsync(customerId);
        }

        public async Task ActivateMembershipAsync(PurchaseOrder purchaseOrder, IDbConnection connection, IDbTransaction transaction, OrderItem item)
        {
            // TODO: We can check if the user already has an active memvbership and in this case just increase the
            // existing expiration, or add the duration on top of the existing one
            var membershipProduct = await _membershipProductRepository.GetMembershipProductByIdAsync(item.ProductId);
            if (membershipProduct != null)
            {
                var membership = new Membership()
                {
                    ActivationDateTime = DateTime.UtcNow,
                    ExpirationDateTime = DateTime.UtcNow.AddMonths(membershipProduct.DurationMonths),
                    CustomerId = purchaseOrder.CustomerId,
                    MembershipType = membershipProduct.MembershipType
                };
                await _membershipRepository.AddMembershipAsync(membership, transaction, connection);
            }
        }
    }
}

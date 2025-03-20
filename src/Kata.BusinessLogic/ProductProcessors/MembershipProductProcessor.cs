using Kata.BusinessLogic.Interfaces;
using Kata.Domain.Entities;
using Kata.Domain.Enums;
using System.Data;

namespace Kata.BusinessLogic.ProductProcessors
{
    public class MembershipProductProcessor : IProductProcessor
    {
        private readonly IMembershipService _membershipService;

        public MembershipProductProcessor(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        public async Task ProcessProductAsync(PurchaseOrder purchaseOrder, OrderItem item, IDbConnection connection, IDbTransaction transaction)
        {
            await _membershipService.ActivateMembershipAsync(purchaseOrder, connection, transaction, item);
        }

        public ProductType ProductType => ProductType.Membership;
    }
}

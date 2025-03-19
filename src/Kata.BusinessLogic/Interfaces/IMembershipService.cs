using Kata.Domain.Entities;
using System.Data;

namespace Kata.BusinessLogic.Interfaces
{
    public interface IMembershipService
    {
        Task<IEnumerable<Membership>> GetMembershipsByCustomerIdAsync(int customerId);
        Task ActivateMembershipAsync(PurchaseOrder purchaseOrder, IDbConnection connection, IDbTransaction transaction, OrderItem item);
    }
}

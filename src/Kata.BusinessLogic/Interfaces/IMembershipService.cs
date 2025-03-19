using Kata.Domain.Entities;
using Microsoft.Data.SqlClient;

namespace Kata.BusinessLogic.Interfaces
{
    public interface IMembershipService
    {
        Task<IEnumerable<Membership>> GetMembershipsByCustomerIdAsync(int customerId);
        Task ActivateMembershipAsync(PurchaseOrder purchaseOrder, SqlConnection connection, SqlTransaction transaction, OrderItem item);
    }
}

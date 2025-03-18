using Kata.Domain.Entities;
using Microsoft.Data.SqlClient;

namespace Kata.BusinessLogic.Interfaces
{
    public interface IMembershipService
    {
        IEnumerable<Membership> GetMembershipsByCustomerId(int customerId);
        void ActivateMembership(PurchaseOrder purchaseOrder, SqlConnection connection, SqlTransaction transaction, OrderItem item);
    }
}

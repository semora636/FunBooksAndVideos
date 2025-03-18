using Kata.Domain.Entities;
using Microsoft.Data.SqlClient;

namespace Kata.DataAccess.Interfaces
{
    public interface IMembershipRepository
    {
        IEnumerable<Membership> GetMembershipsByCustomer(int customerId);
        void AddMembership(Membership membership, SqlTransaction transaction, SqlConnection connection);
    }
}

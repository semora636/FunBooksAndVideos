using Kata.Domain.Entities;
using Microsoft.Data.SqlClient;

namespace Kata.DataAccess.Interfaces
{
    public interface IMembershipRepository
    {
        Task<IEnumerable<Membership>> GetMembershipsByCustomerAsync(int customerId);
        Task AddMembershipAsync(Membership membership, SqlTransaction transaction, SqlConnection connection);
    }
}

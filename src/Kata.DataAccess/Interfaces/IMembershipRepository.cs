using Kata.Domain.Entities;
using System.Data;

namespace Kata.DataAccess.Interfaces
{
    public interface IMembershipRepository
    {
        Task<IEnumerable<Membership>> GetMembershipsByCustomerAsync(int customerId);
        Task AddMembershipAsync(Membership membership, IDbTransaction transaction, IDbConnection connection);
    }
}

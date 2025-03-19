using Dapper;
using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;
using System.Data;

namespace Kata.DataAccess.Repositories
{
    public class MembershipRepository : IMembershipRepository
    {
        private readonly ISqlDataAccess _dataAccess;

        public MembershipRepository(ISqlDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<IEnumerable<Membership>> GetMembershipsByCustomerAsync(int customerId)
        {
            using var connection = _dataAccess.CreateConnection();
            return await connection.QueryAsync<Membership>("SELECT * FROM Memberships WHERE CustomerId = @CustomerId", new { CustomerId = customerId });
        }

        public async Task AddMembershipAsync(Membership membership, IDbTransaction transaction, IDbConnection connection)
        {
            membership.MembershipId = await connection.ExecuteScalarAsync<int>("INSERT INTO Memberships (MembershipType, ActivationDateTime, ExpirationDateTime, CustomerId) VALUES (@MembershipType, @ActivationDateTime, @ExpirationDateTime, @CustomerId); SELECT SCOPE_IDENTITY();", membership, transaction);
        }
    }
}

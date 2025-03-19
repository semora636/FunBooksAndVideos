using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;
using System.Data;

namespace Kata.DataAccess.Repositories
{
    public class MembershipRepository : IMembershipRepository
    {
        private readonly ISqlDataAccess _dataAccess;
        private readonly IDapperWrapper _dapperWrapper;

        public MembershipRepository(ISqlDataAccess dataAccess, IDapperWrapper dapperWrapper)
        {
            _dataAccess = dataAccess;
            _dapperWrapper = dapperWrapper;
        }

        public async Task<IEnumerable<Membership>> GetMembershipsByCustomerAsync(int customerId)
        {
            using var connection = _dataAccess.CreateConnection();
            return await _dapperWrapper.QueryAsync<Membership>(connection, "SELECT * FROM Memberships WHERE CustomerId = @CustomerId", new { CustomerId = customerId });
        }

        public async Task AddMembershipAsync(Membership membership, IDbTransaction transaction, IDbConnection connection)
        {
            membership.MembershipId = await _dapperWrapper.ExecuteScalarAsync<int>(connection, "INSERT INTO Memberships (MembershipType, ActivationDateTime, ExpirationDateTime, CustomerId) VALUES (@MembershipType, @ActivationDateTime, @ExpirationDateTime, @CustomerId); SELECT SCOPE_IDENTITY();", membership, transaction);
        }
    }
}

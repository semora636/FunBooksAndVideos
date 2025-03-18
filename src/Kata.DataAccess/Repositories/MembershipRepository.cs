using Dapper;
using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;
using Microsoft.Data.SqlClient;

namespace Kata.DataAccess.Repositories
{
    public class MembershipRepository : IMembershipRepository
    {
        private readonly SqlDataAccess _dataAccess;

        public MembershipRepository(SqlDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public IEnumerable<Membership> GetMembershipsByCustomer(int customerId)
        {
            using var connection = _dataAccess.CreateConnection();
            return connection.Query<Membership>("SELECT * FROM Memberships WHERE CustomerId = @CustomerId", new { CustomerId = customerId }).ToList();
        }

        public void AddMembership(Membership membership, SqlTransaction transaction, SqlConnection connection)
        {
            membership.MembershipId = connection.ExecuteScalar<int>("INSERT INTO Memberships (MembershipType, ActivationDateTime, ExpirationDateTime, CustomerId) VALUES (@MembershipType, @ActivationDateTime, @ExpirationDateTime, @CustomerId); SELECT SCOPE_IDENTITY();", membership, transaction);
        }
    }
}

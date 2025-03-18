using Dapper;
using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;

namespace Kata.DataAccess.Repositories
{
    public class MembershipProductRepository : IMembershipProductRepository
    {
        private readonly SqlDataAccess _dataAccess;

        public MembershipProductRepository(SqlDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public MembershipProduct? GetMembershipProductById(int membershipProductId)
        {
            using var connection = _dataAccess.CreateConnection();
            return connection.QueryFirstOrDefault<MembershipProduct>("SELECT * FROM MembershipProducts WHERE MembershipProductId = @MembershipProductId", new { MembershipProductId = membershipProductId });
        }

        public IEnumerable<MembershipProduct> GetAllMembershipProducts()
        {
            using var connection = _dataAccess.CreateConnection();
            return connection.Query<MembershipProduct>("SELECT * FROM MembershipProducts");
        }

        public void AddMembershipProduct(MembershipProduct membershipProduct)
        {
            using var connection = _dataAccess.CreateConnection();
            connection.Execute("INSERT INTO MembershipProducts (Name, MembershipType, Price, DurationMonths) VALUES (@Name, @MembershipType, @Price, @DurationMonths)", membershipProduct);
        }

        public void UpdateMembershipProduct(MembershipProduct membershipProduct)
        {
            using var connection = _dataAccess.CreateConnection();
            connection.Execute("UPDATE MembershipProducts SET Name = @Name, MembershipType = @MembershipType, Price = @Price, DurationMonths = @DurationMonths WHERE MembershipProductId = @MembershipProductId", membershipProduct);
        }

        public void DeleteMembershipProduct(int membershipProductId)
        {
            using var connection = _dataAccess.CreateConnection();
            connection.Execute("DELETE FROM MembershipProducts WHERE MembershipProductId = @MembershipProductId", new { MembershipProductId = membershipProductId });
        }
    }
}

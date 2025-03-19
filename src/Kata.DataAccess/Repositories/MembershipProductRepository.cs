using Dapper;
using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;

namespace Kata.DataAccess.Repositories
{
    public class MembershipProductRepository : IMembershipProductRepository
    {
        private readonly ISqlDataAccess _dataAccess;

        public MembershipProductRepository(ISqlDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<MembershipProduct?> GetMembershipProductByIdAsync(int membershipProductId)
        {
            using var connection = _dataAccess.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<MembershipProduct>("SELECT * FROM MembershipProducts WHERE MembershipProductId = @MembershipProductId", new { MembershipProductId = membershipProductId });
        }

        public async Task<IEnumerable<MembershipProduct>> GetAllMembershipProductsAsync()
        {
            using var connection = _dataAccess.CreateConnection();
            return await connection.QueryAsync<MembershipProduct>("SELECT * FROM MembershipProducts");
        }

        public async Task AddMembershipProductAsync(MembershipProduct membershipProduct)
        {
            using var connection = _dataAccess.CreateConnection();
            membershipProduct.MembershipProductId = await connection.ExecuteScalarAsync<int>("INSERT INTO MembershipProducts (Name, MembershipType, Price, DurationMonths) VALUES (@Name, @MembershipType, @Price, @DurationMonths); SELECT SCOPE_IDENTITY();", membershipProduct);
        }

        public async Task UpdateMembershipProductAsync(MembershipProduct membershipProduct)
        {
            using var connection = _dataAccess.CreateConnection();
            await connection.ExecuteAsync("UPDATE MembershipProducts SET Name = @Name, MembershipType = @MembershipType, Price = @Price, DurationMonths = @DurationMonths WHERE MembershipProductId = @MembershipProductId", membershipProduct);
        }

        public async Task DeleteMembershipProductAsync(int membershipProductId)
        {
            using var connection = _dataAccess.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM MembershipProducts WHERE MembershipProductId = @MembershipProductId", new { MembershipProductId = membershipProductId });
        }
    }
}

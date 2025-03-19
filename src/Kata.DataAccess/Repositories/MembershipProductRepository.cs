using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;

namespace Kata.DataAccess.Repositories
{
    public class MembershipProductRepository : IMembershipProductRepository
    {
        private readonly ISqlDataAccess _dataAccess;
        private readonly IDapperWrapper _dapperWrapper;

        public MembershipProductRepository(ISqlDataAccess dataAccess, IDapperWrapper dapperWrapper)
        {
            _dataAccess = dataAccess;
            _dapperWrapper = dapperWrapper;
        }

        public async Task<MembershipProduct?> GetMembershipProductByIdAsync(int membershipProductId)
        {
            using var connection = _dataAccess.CreateConnection();
            return await _dapperWrapper.QueryFirstOrDefaultAsync<MembershipProduct>(connection, "SELECT * FROM MembershipProducts WHERE MembershipProductId = @MembershipProductId", new { MembershipProductId = membershipProductId });
        }

        public async Task<IEnumerable<MembershipProduct>> GetAllMembershipProductsAsync()
        {
            using var connection = _dataAccess.CreateConnection();
            return await _dapperWrapper.QueryAsync<MembershipProduct>(connection, "SELECT * FROM MembershipProducts");
        }

        public async Task AddMembershipProductAsync(MembershipProduct membershipProduct)
        {
            using var connection = _dataAccess.CreateConnection();
            membershipProduct.MembershipProductId = await _dapperWrapper.ExecuteScalarAsync<int>(connection, "INSERT INTO MembershipProducts (Name, MembershipType, Price, DurationMonths) VALUES (@Name, @MembershipType, @Price, @DurationMonths); SELECT SCOPE_IDENTITY();", membershipProduct);
        }

        public async Task UpdateMembershipProductAsync(MembershipProduct membershipProduct)
        {
            using var connection = _dataAccess.CreateConnection();
            await _dapperWrapper.ExecuteAsync(connection, "UPDATE MembershipProducts SET Name = @Name, MembershipType = @MembershipType, Price = @Price, DurationMonths = @DurationMonths WHERE MembershipProductId = @MembershipProductId", membershipProduct);
        }

        public async Task DeleteMembershipProductAsync(int membershipProductId)
        {
            using var connection = _dataAccess.CreateConnection();
            await _dapperWrapper.ExecuteAsync(connection, "DELETE FROM MembershipProducts WHERE MembershipProductId = @MembershipProductId", new { MembershipProductId = membershipProductId });
        }
    }
}

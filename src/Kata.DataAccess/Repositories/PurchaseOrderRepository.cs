using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;
using System.Data;

namespace Kata.DataAccess.Repositories
{
    public class PurchaseOrderRepository : IPurchaseOrderRepository
    {
        private readonly ISqlDataAccess _dataAccess;
        private readonly IDapperWrapper _dapperWrapper;

        public PurchaseOrderRepository(ISqlDataAccess dataAccess, IDapperWrapper dapperWrapper)
        {
            _dataAccess = dataAccess;
            _dapperWrapper = dapperWrapper;
        }

        public async Task<PurchaseOrder?> GetPurchaseOrderByIdAsync(int purchaseOrderId)
        {
            using var connection = _dataAccess.CreateConnection();
            return await _dapperWrapper.QueryFirstOrDefaultAsync<PurchaseOrder>(connection, "SELECT * FROM PurchaseOrders WHERE PurchaseOrderId = @PurchaseOrderId", new { PurchaseOrderId = purchaseOrderId });
        }

        public async Task<IEnumerable<PurchaseOrder>> GetAllPurchaseOrdersAsync()
        {
            using var connection = _dataAccess.CreateConnection();
            return await _dapperWrapper.QueryAsync<PurchaseOrder>(connection, "SELECT * FROM PurchaseOrders");
        }

        public async Task<int> AddPurchaseOrderAsync(PurchaseOrder purchaseOrder, IDbTransaction transaction, IDbConnection connection)
        {
            return await _dapperWrapper.ExecuteScalarAsync<int>(connection, "INSERT INTO PurchaseOrders (CustomerId, OrderDateTime, TotalPrice) VALUES (@CustomerId, @OrderDateTime, @TotalPrice); SELECT SCOPE_IDENTITY();", purchaseOrder, transaction);
        }

        public async Task UpdatePurchaseOrderAsync(PurchaseOrder purchaseOrder, IDbTransaction transaction, IDbConnection connection)
        {
            await _dapperWrapper.ExecuteAsync(connection, "UPDATE PurchaseOrders SET CustomerId = @CustomerId, OrderDateTime = @OrderDateTime, TotalPrice = @TotalPrice WHERE PurchaseOrderId = @PurchaseOrderId", purchaseOrder, transaction);
        }

        public async Task DeletePurchaseOrderAsync(int purchaseOrderId, IDbTransaction transaction, IDbConnection connection)
        {
            await _dapperWrapper.ExecuteAsync(connection, "DELETE FROM OrderItems WHERE PurchaseOrderId = @PurchaseOrderId", new { PurchaseOrderId = purchaseOrderId }, transaction);
        }
    }
}

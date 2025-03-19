using Dapper;
using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;
using Microsoft.Data.SqlClient;

namespace Kata.DataAccess.Repositories
{
    public class PurchaseOrderRepository : IPurchaseOrderRepository
    {
        private readonly SqlDataAccess _dataAccess;

        public PurchaseOrderRepository(SqlDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<PurchaseOrder?> GetPurchaseOrderByIdAsync(int purchaseOrderId)
        {
            using var connection = _dataAccess.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<PurchaseOrder>("SELECT * FROM PurchaseOrders WHERE PurchaseOrderId = @PurchaseOrderId", new { PurchaseOrderId = purchaseOrderId });
        }

        public async Task<IEnumerable<PurchaseOrder>> GetAllPurchaseOrdersAsync()
        {
            using var connection = _dataAccess.CreateConnection();
            return await connection.QueryAsync<PurchaseOrder>("SELECT * FROM PurchaseOrders");
        }

        public async Task<int> AddPurchaseOrderAsync(PurchaseOrder purchaseOrder, SqlTransaction transaction, SqlConnection connection)
        {
            return await connection.ExecuteScalarAsync<int>("INSERT INTO PurchaseOrders (CustomerId, OrderDateTime, TotalPrice) VALUES (@CustomerId, @OrderDateTime, @TotalPrice); SELECT SCOPE_IDENTITY();", purchaseOrder, transaction);
        }

        public async Task UpdatePurchaseOrderAsync(PurchaseOrder purchaseOrder, SqlTransaction transaction, SqlConnection connection)
        {
            await connection.ExecuteAsync("UPDATE PurchaseOrders SET CustomerId = @CustomerId, OrderDateTime = @OrderDateTime, TotalPrice = @TotalPrice WHERE PurchaseOrderId = @PurchaseOrderId", purchaseOrder, transaction);
        }

        public async Task DeletePurchaseOrderAsync(int purchaseOrderId, SqlTransaction transaction, SqlConnection connection)
        {
            await connection.ExecuteAsync("DELETE FROM OrderItems WHERE PurchaseOrderId = @PurchaseOrderId", new { PurchaseOrderId = purchaseOrderId }, transaction);
        }
    }
}

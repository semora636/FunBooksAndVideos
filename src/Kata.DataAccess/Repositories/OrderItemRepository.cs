using Dapper;
using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;
using Microsoft.Data.SqlClient;

namespace Kata.DataAccess.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly SqlDataAccess _dataAccess;

        public OrderItemRepository(SqlDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<int> AddOrderItemAsync(OrderItem orderItem, SqlTransaction transaction, SqlConnection connection)
        {
            return await connection.ExecuteScalarAsync<int>("INSERT INTO OrderItems (PurchaseOrderId, ProductId, ProductType, Quantity, Price) VALUES (@PurchaseOrderId, @ProductId, @ProductType, @Quantity, @Price); SELECT SCOPE_IDENTITY();", orderItem, transaction);
        }

        public async Task DeleteOrderItemsByPurchaseOrderIdAsync(int purchaseOrderId, SqlTransaction transaction, SqlConnection connection)
        {
            await connection.ExecuteAsync("DELETE FROM OrderItems WHERE PurchaseOrderId = @PurchaseOrderId", new { PurchaseOrderId = purchaseOrderId }, transaction);
        }

        public async Task<IEnumerable<OrderItem>> GetOrderItemsByPurchaseOrderIdAsync(int purchaseOrderId)
        {
            using var connection = _dataAccess.CreateConnection();
            return await connection.QueryAsync<OrderItem>("SELECT * FROM OrderItems WHERE PurchaseOrderId = @PurchaseOrderId", new { PurchaseOrderId = purchaseOrderId });
        }
    }
}

using Dapper;
using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;
using System.Data;

namespace Kata.DataAccess.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly ISqlDataAccess _dataAccess;

        public OrderItemRepository(ISqlDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<int> AddOrderItemAsync(OrderItem orderItem, IDbTransaction transaction, IDbConnection connection)
        {
            return await connection.ExecuteScalarAsync<int>("INSERT INTO OrderItems (PurchaseOrderId, ProductId, ProductType, Quantity, Price) VALUES (@PurchaseOrderId, @ProductId, @ProductType, @Quantity, @Price); SELECT SCOPE_IDENTITY();", orderItem, transaction);
        }

        public async Task DeleteOrderItemsByPurchaseOrderIdAsync(int purchaseOrderId, IDbTransaction transaction, IDbConnection connection)
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

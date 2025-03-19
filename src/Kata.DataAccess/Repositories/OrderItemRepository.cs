using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;
using System.Data;

namespace Kata.DataAccess.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly ISqlDataAccess _dataAccess;
        private readonly IDapperWrapper _dapperWrapper;

        public OrderItemRepository(ISqlDataAccess dataAccess, IDapperWrapper dapperWrapper)
        {
            _dataAccess = dataAccess;
            _dapperWrapper = dapperWrapper;
        }

        public async Task<int> AddOrderItemAsync(OrderItem orderItem, IDbTransaction transaction, IDbConnection connection)
        {
            return await _dapperWrapper.ExecuteScalarAsync<int>(connection, "INSERT INTO OrderItems (PurchaseOrderId, ProductId, ProductType, Quantity, Price) VALUES (@PurchaseOrderId, @ProductId, @ProductType, @Quantity, @Price); SELECT SCOPE_IDENTITY();", orderItem, transaction);
        }

        public async Task DeleteOrderItemsByPurchaseOrderIdAsync(int purchaseOrderId, IDbTransaction transaction, IDbConnection connection)
        {
            await _dapperWrapper.ExecuteAsync(connection, "DELETE FROM OrderItems WHERE PurchaseOrderId = @PurchaseOrderId", new { PurchaseOrderId = purchaseOrderId }, transaction);
        }

        public async Task<IEnumerable<OrderItem>> GetOrderItemsByPurchaseOrderIdAsync(int purchaseOrderId)
        {
            using var connection = _dataAccess.CreateConnection();
            return await _dapperWrapper.QueryAsync<OrderItem>(connection, "SELECT * FROM OrderItems WHERE PurchaseOrderId = @PurchaseOrderId", new { PurchaseOrderId = purchaseOrderId });
        }
    }
}

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

        public int AddOrderItem(OrderItem orderItem, SqlTransaction transaction, SqlConnection connection)
        {
            return connection.ExecuteScalar<int>("INSERT INTO OrderItems (PurchaseOrderId, ProductId, ProductType, Quantity, Price) VALUES (@PurchaseOrderId, @ProductId, @ProductType, @Quantity, @Price)", orderItem, transaction);
        }

        public void DeleteOrderItemsByPurchaseOrderId(int purchaseOrderId, SqlTransaction transaction, SqlConnection connection)
        {
            connection.Execute("DELETE FROM OrderItems WHERE PurchaseOrderId = @PurchaseOrderId", new { PurchaseOrderId = purchaseOrderId }, transaction);
        }

        public List<OrderItem> GetOrderItemsByPurchaseOrderId(int purchaseOrderId)
        {
            using var connection = _dataAccess.CreateConnection();
            return connection.Query<OrderItem>("SELECT * FROM OrderItems WHERE PurchaseOrderId = @PurchaseOrderId", new { PurchaseOrderId = purchaseOrderId }).ToList();
        }
    }
}

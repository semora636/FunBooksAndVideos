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

        public PurchaseOrder? GetPurchaseOrderById(int purchaseOrderId)
        {
            using var connection = _dataAccess.CreateConnection();
            return connection.QueryFirstOrDefault<PurchaseOrder>("SELECT * FROM PurchaseOrders WHERE PurchaseOrderId = @PurchaseOrderId", new { PurchaseOrderId = purchaseOrderId });
        }

        public IEnumerable<PurchaseOrder> GetAllPurchaseOrders()
        {
            using var connection = _dataAccess.CreateConnection();
            return connection.Query<PurchaseOrder>("SELECT * FROM PurchaseOrders").ToList();
        }

        public int AddPurchaseOrder(PurchaseOrder purchaseOrder, SqlTransaction transaction, SqlConnection connection)
        {
            return connection.ExecuteScalar<int>("INSERT INTO PurchaseOrders (CustomerId, OrderDateTime, TotalPrice) VALUES (@CustomerId, @OrderDateTime, @TotalPrice); SELECT SCOPE_IDENTITY();", purchaseOrder, transaction);
        }

        public void UpdatePurchaseOrder(PurchaseOrder purchaseOrder, SqlTransaction transaction, SqlConnection connection)
        {
            connection.Execute("UPDATE PurchaseOrders SET CustomerId = @CustomerId, OrderDateTime = @OrderDateTime, TotalPrice = @TotalPrice WHERE PurchaseOrderId = @PurchaseOrderId", purchaseOrder, transaction);
        }

        public void DeletePurchaseOrder(int purchaseOrderId, SqlTransaction transaction, SqlConnection connection)
        {
            connection.Execute("DELETE FROM OrderItems WHERE PurchaseOrderId = @PurchaseOrderId", new { PurchaseOrderId = purchaseOrderId }, transaction);
        }
    }
}

using Dapper;
using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;

namespace Kata.DataAccess.Repositories
{
    // In this repository we are also dealing with OrderItem as it is tightly coupled with PurchaseOrder. The
    // alternative is to create a new repository for OrderItem, but since the OrderItems are not going to be changed
    // independently of the PurchaseOrder, this approach was chosen
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
            var purchaseOrder = connection.QueryFirstOrDefault<PurchaseOrder>("SELECT * FROM PurchaseOrders WHERE PurchaseOrderId = @PurchaseOrderId", new { PurchaseOrderId = purchaseOrderId });

            if (purchaseOrder != null)
            {
                purchaseOrder.Items = connection.Query<OrderItem>("SELECT * FROM OrderItems WHERE PurchaseOrderId = @PurchaseOrderId", new { PurchaseOrderId = purchaseOrderId }).ToList();
            }

            return purchaseOrder;
        }

        public IEnumerable<PurchaseOrder> GetAllPurchaseOrders()
        {
            using var connection = _dataAccess.CreateConnection();
            var purchaseOrders = connection.Query<PurchaseOrder>("SELECT * FROM PurchaseOrders").ToList();

            foreach (var purchaseOrder in purchaseOrders)
            {
                purchaseOrder.Items = connection.Query<OrderItem>("SELECT * FROM OrderItems WHERE PurchaseOrderId = @PurchaseOrderId", new { PurchaseOrderId = purchaseOrder.PurchaseOrderId }).ToList();
            }
            return purchaseOrders;
        }

        public void AddPurchaseOrder(PurchaseOrder purchaseOrder)
        {
            using var connection = _dataAccess.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                purchaseOrder.PurchaseOrderId = connection.ExecuteScalar<int>("INSERT INTO PurchaseOrders (CustomerId, OrderDateTime, TotalPrice) VALUES (@CustomerId, @OrderDateTime, @TotalPrice); SELECT SCOPE_IDENTITY();", purchaseOrder, transaction);

                if (purchaseOrder.Items != null)
                {
                    foreach (var item in purchaseOrder.Items)
                    {
                        item.PurchaseOrderId = purchaseOrder.PurchaseOrderId;
                        item.OrderItemId = connection.ExecuteScalar<int>("INSERT INTO OrderItems (PurchaseOrderId, ProductId, ProductType, Quantity, Price) VALUES (@PurchaseOrderId, @ProductId, @ProductType, @Quantity, @Price); SELECT SCOPE_IDENTITY();", item, transaction);
                    }
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        public void UpdatePurchaseOrder(PurchaseOrder purchaseOrder)
        {
            using var connection = _dataAccess.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                connection.Execute("UPDATE PurchaseOrders SET CustomerId = @CustomerId, OrderDateTime = @OrderDateTime, TotalPrice = @TotalPrice WHERE PurchaseOrderId = @PurchaseOrderId", purchaseOrder, transaction);

                connection.Execute("DELETE FROM OrderItems WHERE PurchaseOrderId = @PurchaseOrderId", new { purchaseOrder.PurchaseOrderId }, transaction);

                if (purchaseOrder.Items != null)
                {
                    foreach (var item in purchaseOrder.Items)
                    {
                        item.PurchaseOrderId = purchaseOrder.PurchaseOrderId;
                        connection.Execute("INSERT INTO OrderItems (PurchaseOrderId, ProductId, ProductType, Quantity, Price) VALUES (@PurchaseOrderId, @ProductId, @ProductType, @Quantity, @Price)", item, transaction);
                    }
                }

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        public void DeletePurchaseOrder(int purchaseOrderId)
        {
            using var connection = _dataAccess.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                connection.Execute("DELETE FROM OrderItems WHERE PurchaseOrderId = @PurchaseOrderId", new { PurchaseOrderId = purchaseOrderId }, transaction);
                connection.Execute("DELETE FROM PurchaseOrders WHERE PurchaseOrderId = @PurchaseOrderId", new { PurchaseOrderId = purchaseOrderId }, transaction);

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}

using Kata.BusinessLogic.Interfaces;
using Kata.DataAccess;
using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;

namespace Kata.BusinessLogic.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly SqlDataAccess _dataAccess;

        public PurchaseOrderService(IPurchaseOrderRepository purchaseOrderRepository, IOrderItemRepository orderItemRepository, SqlDataAccess dataAccess)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _orderItemRepository = orderItemRepository;
            _dataAccess = dataAccess;
        }

        public PurchaseOrder? GetPurchaseOrderById(int purchaseOrderId)
        {
            var purchaseOrder = _purchaseOrderRepository.GetPurchaseOrderById(purchaseOrderId);

            if (purchaseOrder != null)
            {
                purchaseOrder.Items = _orderItemRepository.GetOrderItemsByPurchaseOrderId(purchaseOrderId);
            }

            return purchaseOrder;
        }

        public IEnumerable<PurchaseOrder> GetAllPurchaseOrders()
        {
            var purchaseOrders = _purchaseOrderRepository.GetAllPurchaseOrders();

            foreach (var purchaseOrder in purchaseOrders)
            {
                purchaseOrder.Items = _orderItemRepository.GetOrderItemsByPurchaseOrderId(purchaseOrder.PurchaseOrderId);
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
                purchaseOrder.PurchaseOrderId = _purchaseOrderRepository.AddPurchaseOrder(purchaseOrder, transaction, connection);

                if (purchaseOrder.Items != null)
                {
                    foreach (var item in purchaseOrder.Items)
                    {
                        item.PurchaseOrderId = purchaseOrder.PurchaseOrderId;
                        item.OrderItemId = _orderItemRepository.AddOrderItem(item, transaction, connection);
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
                _purchaseOrderRepository.UpdatePurchaseOrder(purchaseOrder, transaction, connection);
                _orderItemRepository.DeleteOrderItemsByPurchaseOrderId(purchaseOrder.PurchaseOrderId, transaction, connection);

                if (purchaseOrder.Items != null)
                {
                    foreach (var item in purchaseOrder.Items)
                    {
                        item.PurchaseOrderId = purchaseOrder.PurchaseOrderId;
                        item.OrderItemId = _orderItemRepository.AddOrderItem(item, transaction, connection);
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
                _orderItemRepository.DeleteOrderItemsByPurchaseOrderId(purchaseOrderId, transaction, connection);
                _purchaseOrderRepository.DeletePurchaseOrder(purchaseOrderId, transaction, connection);

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

using Kata.BusinessLogic.Interfaces;
using Kata.DataAccess;
using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;

namespace Kata.BusinessLogic.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly IOrderItemService _orderItemService;
        private readonly IMembershipService _membershipService;
        private readonly IShippingSlipService _shippingSlipService;
        private readonly SqlDataAccess _dataAccess;

        public PurchaseOrderService(
            SqlDataAccess dataAccess,
            IPurchaseOrderRepository purchaseOrderRepository,
            IOrderItemService orderItemService,
            IMembershipService membershipService,
            IShippingSlipService shippingSlipService)
        {
            _dataAccess = dataAccess;
            _purchaseOrderRepository = purchaseOrderRepository;
            _orderItemService = orderItemService;
            _membershipService = membershipService;
            _shippingSlipService = shippingSlipService;
        }

        public PurchaseOrder? GetPurchaseOrderById(int purchaseOrderId)
        {
            var purchaseOrder = _purchaseOrderRepository.GetPurchaseOrderById(purchaseOrderId);

            if (purchaseOrder != null)
            {
                purchaseOrder.Items = _orderItemService.GetOrderItemsByPurchaseOrderId(purchaseOrderId);
                purchaseOrder.ShippingSlips = _shippingSlipService.GetShippingSlipsByPurchaseOrderId(purchaseOrderId);
            }

            return purchaseOrder;
        }

        public IEnumerable<PurchaseOrder> GetAllPurchaseOrders()
        {
            var purchaseOrders = _purchaseOrderRepository.GetAllPurchaseOrders();

            foreach (var purchaseOrder in purchaseOrders)
            {
                purchaseOrder.Items = _orderItemService.GetOrderItemsByPurchaseOrderId(purchaseOrder.PurchaseOrderId);
                purchaseOrder.ShippingSlips = _shippingSlipService.GetShippingSlipsByPurchaseOrderId(purchaseOrder.PurchaseOrderId);
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
                if (purchaseOrder.Items != null)
                {
                    purchaseOrder.OrderDateTime = DateTime.UtcNow;
                    purchaseOrder.TotalPrice = purchaseOrder.Items.Sum(item => item.Price * item.Quantity);
                    purchaseOrder.PurchaseOrderId = _purchaseOrderRepository.AddPurchaseOrder(purchaseOrder, transaction, connection);

                    foreach (var item in purchaseOrder.Items)
                    {
                        item.PurchaseOrderId = purchaseOrder.PurchaseOrderId;
                        item.OrderItemId = _orderItemService.AddOrderItem(item, transaction, connection);

                        if (item.ProductType == Domain.Enums.ProductType.Membership)
                        {
                            // If the item is a membership product, we need to add it to Membership table
                            _membershipService.ActivateMembership(purchaseOrder, connection, transaction, item);
                        }
                        else if (item.ProductType == Domain.Enums.ProductType.Book)
                        {
                            // If the item is a phisical product, so we need to generate a shipping slip
                            _shippingSlipService.GenerateShippingSlip(purchaseOrder, connection, transaction);
                        }
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
                _orderItemService.DeleteOrderItemsByPurchaseOrderId(purchaseOrder.PurchaseOrderId, transaction, connection);

                if (purchaseOrder.Items != null)
                {
                    foreach (var item in purchaseOrder.Items)
                    {
                        item.PurchaseOrderId = purchaseOrder.PurchaseOrderId;
                        item.OrderItemId = _orderItemService.AddOrderItem(item, transaction, connection);
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
                _orderItemService.DeleteOrderItemsByPurchaseOrderId(purchaseOrderId, transaction, connection);
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

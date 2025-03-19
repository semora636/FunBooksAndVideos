using Kata.BusinessLogic.Interfaces;
using Kata.DataAccess;
using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;
using Microsoft.Data.SqlClient;

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

        public async Task<PurchaseOrder?> GetPurchaseOrderByIdAsync(int purchaseOrderId)
        {
            var purchaseOrder = await _purchaseOrderRepository.GetPurchaseOrderByIdAsync(purchaseOrderId);

            if (purchaseOrder != null)
            {
                purchaseOrder.Items = (await _orderItemService.GetOrderItemsByPurchaseOrderIdAsync(purchaseOrderId)).ToList();
                purchaseOrder.ShippingSlips = (await _shippingSlipService.GetShippingSlipsByPurchaseOrderIdAsync(purchaseOrderId)).ToList();
            }

            return purchaseOrder;
        }

        public async Task<IEnumerable<PurchaseOrder>> GetAllPurchaseOrdersAsync()
        {
            var purchaseOrders = await _purchaseOrderRepository.GetAllPurchaseOrdersAsync();

            foreach (var purchaseOrder in purchaseOrders)
            {
                purchaseOrder.Items = (await _orderItemService.GetOrderItemsByPurchaseOrderIdAsync(purchaseOrder.PurchaseOrderId)).ToList();
                purchaseOrder.ShippingSlips = (await _shippingSlipService.GetShippingSlipsByPurchaseOrderIdAsync(purchaseOrder.PurchaseOrderId)).ToList();
            }

            return purchaseOrders;
        }

        public async Task AddPurchaseOrderAsync(PurchaseOrder purchaseOrder)
        {
            await ExecuteTransactionAsync(async (transaction, connection) =>
            {
                if (purchaseOrder.Items != null)
                {
                    purchaseOrder.OrderDateTime = DateTime.UtcNow;
                    purchaseOrder.TotalPrice = purchaseOrder.Items.Sum(item => item.Price * item.Quantity);
                    purchaseOrder.PurchaseOrderId = await _purchaseOrderRepository.AddPurchaseOrderAsync(purchaseOrder, transaction, connection);
                    purchaseOrder.ShippingSlips = new List<ShippingSlip>();

                    foreach (var item in purchaseOrder.Items)
                    {
                        item.PurchaseOrderId = purchaseOrder.PurchaseOrderId;

                        // TODO: Add validation to make sure the product exists
                        item.OrderItemId = await _orderItemService.AddOrderItemAsync(item, transaction, connection);

                        if (item.ProductType == Domain.Enums.ProductType.Membership)
                        {
                            // If the item is a membership product, we need to add it to Membership table
                            await _membershipService.ActivateMembershipAsync(purchaseOrder, connection, transaction, item);
                        }
                        else if (item.ProductType == Domain.Enums.ProductType.Book)
                        {
                            // If the item is a phisical product, so we need to generate a shipping slip
                            await _shippingSlipService.GenerateShippingSlipAsync(purchaseOrder, connection, transaction);
                        }
                    }
                }
            });
        }

        public async Task UpdatePurchaseOrderAsync(PurchaseOrder purchaseOrder)
        {
            await ExecuteTransactionAsync(async (transaction, connection) =>
            {
                await _purchaseOrderRepository.UpdatePurchaseOrderAsync(purchaseOrder, transaction, connection);
                await _orderItemService.DeleteOrderItemsByPurchaseOrderIdAsync(purchaseOrder.PurchaseOrderId, transaction, connection);

                if (purchaseOrder.Items != null)
                {
                    foreach (var item in purchaseOrder.Items)
                    {
                        item.PurchaseOrderId = purchaseOrder.PurchaseOrderId;
                        item.OrderItemId = await _orderItemService.AddOrderItemAsync(item, transaction, connection);
                    }
                }
            });
        }

        public async Task DeletePurchaseOrderAsync(int purchaseOrderId)
        {
            await ExecuteTransactionAsync(async (transaction, connection) =>
            {
                await _orderItemService.DeleteOrderItemsByPurchaseOrderIdAsync(purchaseOrderId, transaction, connection);
                await _purchaseOrderRepository.DeletePurchaseOrderAsync(purchaseOrderId, transaction, connection);

            });
        }

        private async Task ExecuteTransactionAsync(Func<SqlTransaction, SqlConnection, Task> operation)
        {
            using var connection = _dataAccess.CreateConnection();
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
                await operation(transaction, connection);
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

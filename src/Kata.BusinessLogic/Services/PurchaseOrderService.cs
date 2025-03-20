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
        private readonly IShippingSlipService _shippingSlipService;
        private readonly IEnumerable<IProductProcessor> _productProcessors;
        private readonly ITransactionHandler _transactionHandler;

        public PurchaseOrderService(
            IPurchaseOrderRepository purchaseOrderRepository,
            IOrderItemRepository orderItemService,
            IShippingSlipService shippingSlipService,
            IEnumerable<IProductProcessor> productProcessors,
            ITransactionHandler transactionHandler)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _orderItemRepository = orderItemService;
            _shippingSlipService = shippingSlipService;
            _productProcessors = productProcessors;
            _transactionHandler = transactionHandler;
        }

        public async Task<PurchaseOrder?> GetPurchaseOrderByIdAsync(int purchaseOrderId)
        {
            var purchaseOrder = await _purchaseOrderRepository.GetPurchaseOrderWithItemsAndSlipByIdAsync(purchaseOrderId);

            //if (purchaseOrder != null)
            //{
            //    purchaseOrder.Items = (await _orderItemRepository.GetOrderItemsByPurchaseOrderIdAsync(purchaseOrderId)).ToList();
            //    purchaseOrder.ShippingSlips = (await _shippingSlipService.GetShippingSlipsByPurchaseOrderIdAsync(purchaseOrderId)).ToList();
            //}

            return purchaseOrder;
        }

        public async Task<IEnumerable<PurchaseOrder>> GetAllPurchaseOrdersAsync()
        {
            var purchaseOrders = await _purchaseOrderRepository.GetAllPurchaseOrdersWithItemsAndSlipsAsync();
            return purchaseOrders;
        }

        public async Task AddPurchaseOrderAsync(PurchaseOrder purchaseOrder)
        {
            await _transactionHandler.ExecuteTransactionAsync(async (transaction, connection) =>
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
                        item.OrderItemId = await _orderItemRepository.AddOrderItemAsync(item, transaction, connection);

                        var processor = _productProcessors.FirstOrDefault(p => p.ProductType == item.ProductType);
                        if (processor != null)
                        {
                            await processor.ProcessProductAsync(purchaseOrder, item, connection, transaction);
                        }
                    }
                }
            });
        }

        public async Task UpdatePurchaseOrderAsync(PurchaseOrder purchaseOrder)
        {
            await _transactionHandler.ExecuteTransactionAsync(async (transaction, connection) =>
            {
                await _purchaseOrderRepository.UpdatePurchaseOrderAsync(purchaseOrder, transaction, connection);
                await _orderItemRepository.DeleteOrderItemsByPurchaseOrderIdAsync(purchaseOrder.PurchaseOrderId, transaction, connection);

                if (purchaseOrder.Items != null)
                {
                    foreach (var item in purchaseOrder.Items)
                    {
                        item.PurchaseOrderId = purchaseOrder.PurchaseOrderId;
                        item.OrderItemId = await _orderItemRepository.AddOrderItemAsync(item, transaction, connection);
                    }
                }
            });
        }

        public async Task DeletePurchaseOrderAsync(int purchaseOrderId)
        {
            await _transactionHandler.ExecuteTransactionAsync(async (transaction, connection) =>
            {
                await _orderItemRepository.DeleteOrderItemsByPurchaseOrderIdAsync(purchaseOrderId, transaction, connection);
                await _purchaseOrderRepository.DeletePurchaseOrderAsync(purchaseOrderId, transaction, connection);

            });
        }
    }
}

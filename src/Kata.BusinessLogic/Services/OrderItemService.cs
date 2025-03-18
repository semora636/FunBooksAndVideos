using Kata.BusinessLogic.Interfaces;
using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;
using Microsoft.Data.SqlClient;

namespace Kata.BusinessLogic.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;

        public OrderItemService(IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }

        public IEnumerable<OrderItem> GetOrderItemsByPurchaseOrderId(int purchaseOrderId)
        {
            return _orderItemRepository.GetOrderItemsByPurchaseOrderId(purchaseOrderId);
        }

        public int AddOrderItem(OrderItem orderItem, SqlTransaction transaction, SqlConnection connection)
        {
            return _orderItemRepository.AddOrderItem(orderItem, transaction, connection);
        }

        public void DeleteOrderItemsByPurchaseOrderId(int purchaseOrderId, SqlTransaction transaction, SqlConnection connection)
        {
            _orderItemRepository.DeleteOrderItemsByPurchaseOrderId(purchaseOrderId, transaction, connection);
        }
    }
}

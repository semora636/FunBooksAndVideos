using Kata.Domain.Entities;
using Microsoft.Data.SqlClient;

namespace Kata.BusinessLogic.Interfaces
{
    public interface IOrderItemService
    {
        IEnumerable<OrderItem> GetOrderItemsByPurchaseOrderId(int purchaseOrderId);
        int AddOrderItem(OrderItem orderItem, SqlTransaction transaction, SqlConnection connection);
        void DeleteOrderItemsByPurchaseOrderId(int purchaseOrderId, SqlTransaction transaction, SqlConnection connection);
    }
}

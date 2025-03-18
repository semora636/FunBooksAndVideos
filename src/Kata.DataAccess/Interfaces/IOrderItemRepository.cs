using Kata.Domain.Entities;
using Microsoft.Data.SqlClient;

namespace Kata.DataAccess.Interfaces
{
    public interface IOrderItemRepository
    {
        int AddOrderItem(OrderItem orderItem, SqlTransaction transaction, SqlConnection connection);
        void DeleteOrderItemsByPurchaseOrderId(int purchaseOrderId, SqlTransaction transaction, SqlConnection connection);
        IEnumerable<OrderItem> GetOrderItemsByPurchaseOrderId(int purchaseOrderId);
    }
}

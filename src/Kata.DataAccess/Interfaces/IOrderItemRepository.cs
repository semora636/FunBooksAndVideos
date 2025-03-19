using Kata.Domain.Entities;
using Microsoft.Data.SqlClient;

namespace Kata.DataAccess.Interfaces
{
    public interface IOrderItemRepository
    {
        Task<int> AddOrderItemAsync(OrderItem orderItem, SqlTransaction transaction, SqlConnection connection);
        Task DeleteOrderItemsByPurchaseOrderIdAsync(int purchaseOrderId, SqlTransaction transaction, SqlConnection connection);
        Task<IEnumerable<OrderItem>> GetOrderItemsByPurchaseOrderIdAsync(int purchaseOrderId);
    }
}

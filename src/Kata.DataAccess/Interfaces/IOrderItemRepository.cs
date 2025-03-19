using Kata.Domain.Entities;
using System.Data;

namespace Kata.DataAccess.Interfaces
{
    public interface IOrderItemRepository
    {
        Task<int> AddOrderItemAsync(OrderItem orderItem, IDbTransaction transaction, IDbConnection connection);
        Task DeleteOrderItemsByPurchaseOrderIdAsync(int purchaseOrderId, IDbTransaction transaction, IDbConnection connection);
        Task<IEnumerable<OrderItem>> GetOrderItemsByPurchaseOrderIdAsync(int purchaseOrderId);
    }
}

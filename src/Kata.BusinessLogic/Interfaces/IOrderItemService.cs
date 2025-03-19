using Kata.Domain.Entities;
using System.Data;

namespace Kata.BusinessLogic.Interfaces
{
    public interface IOrderItemService
    {
        Task<IEnumerable<OrderItem>> GetOrderItemsByPurchaseOrderIdAsync(int purchaseOrderId);
        Task<int> AddOrderItemAsync(OrderItem orderItem, IDbTransaction transaction, IDbConnection connection);
        Task DeleteOrderItemsByPurchaseOrderIdAsync(int purchaseOrderId, IDbTransaction transaction, IDbConnection connection);
    }
}

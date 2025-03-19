using Kata.Domain.Entities;
using System.Data;

namespace Kata.DataAccess.Interfaces
{
    public interface IPurchaseOrderRepository
    {
        Task<PurchaseOrder?> GetPurchaseOrderByIdAsync(int purchaseOrderId);
        Task<IEnumerable<PurchaseOrder>> GetAllPurchaseOrdersAsync();
        Task<int> AddPurchaseOrderAsync(PurchaseOrder purchaseOrder, IDbTransaction transaction, IDbConnection connection);
        Task UpdatePurchaseOrderAsync(PurchaseOrder purchaseOrder, IDbTransaction transaction, IDbConnection connection);
        Task DeletePurchaseOrderAsync(int purchaseOrderId, IDbTransaction transaction, IDbConnection connection);
    }
}

using Kata.Domain.Entities;
using Microsoft.Data.SqlClient;

namespace Kata.DataAccess.Interfaces
{
    public interface IPurchaseOrderRepository
    {
        Task<PurchaseOrder?> GetPurchaseOrderByIdAsync(int purchaseOrderId);
        Task<IEnumerable<PurchaseOrder>> GetAllPurchaseOrdersAsync();
        Task<int> AddPurchaseOrderAsync(PurchaseOrder purchaseOrder, SqlTransaction transaction, SqlConnection connection);
        Task UpdatePurchaseOrderAsync(PurchaseOrder purchaseOrder, SqlTransaction transaction, SqlConnection connection);
        Task DeletePurchaseOrderAsync(int purchaseOrderId, SqlTransaction transaction, SqlConnection connection);
    }
}

using Kata.Domain.Entities;

namespace Kata.BusinessLogic.Interfaces
{
    public interface IPurchaseOrderService
    {
        Task<PurchaseOrder?> GetPurchaseOrderByIdAsync(int purchaseOrderId);
        Task<IEnumerable<PurchaseOrder>> GetAllPurchaseOrdersAsync();
        Task AddPurchaseOrderAsync(PurchaseOrder purchaseOrder);
        Task UpdatePurchaseOrderAsync(PurchaseOrder purchaseOrder);
        Task DeletePurchaseOrderAsync(int purchaseOrderId);
    }
}

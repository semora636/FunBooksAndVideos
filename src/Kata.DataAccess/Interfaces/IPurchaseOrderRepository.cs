using Kata.Domain.Entities;

namespace Kata.DataAccess.Interfaces
{
    public interface IPurchaseOrderRepository
    {
        PurchaseOrder? GetPurchaseOrderById(int purchaseOrderId);
        IEnumerable<PurchaseOrder> GetAllPurchaseOrders();
        void AddPurchaseOrder(PurchaseOrder purchaseOrder);
        void UpdatePurchaseOrder(PurchaseOrder purchaseOrder);
        void DeletePurchaseOrder(int purchaseOrderId);
    }
}

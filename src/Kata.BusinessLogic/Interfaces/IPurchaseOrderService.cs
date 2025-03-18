using Kata.Domain.Entities;

namespace Kata.BusinessLogic.Interfaces
{
    public interface IPurchaseOrderService
    {
        PurchaseOrder? GetPurchaseOrderById(int purchaseOrderId);
        IEnumerable<PurchaseOrder> GetAllPurchaseOrders();
        void AddPurchaseOrder(PurchaseOrder purchaseOrder);
        void UpdatePurchaseOrder(PurchaseOrder purchaseOrder);
        void DeletePurchaseOrder(int purchaseOrderId);
    }
}

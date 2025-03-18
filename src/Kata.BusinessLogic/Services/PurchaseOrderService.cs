using Kata.BusinessLogic.Interfaces;
using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;

namespace Kata.BusinessLogic.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;

        public PurchaseOrderService(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public PurchaseOrder? GetPurchaseOrderById(int purchaseOrderId)
        {
            return _purchaseOrderRepository.GetPurchaseOrderById(purchaseOrderId);
        }

        public IEnumerable<PurchaseOrder> GetAllPurchaseOrders()
        {
            return _purchaseOrderRepository.GetAllPurchaseOrders();
        }

        public void AddPurchaseOrder(PurchaseOrder purchaseOrder)
        {
            _purchaseOrderRepository.AddPurchaseOrder(purchaseOrder);
        }

        public void UpdatePurchaseOrder(PurchaseOrder purchaseOrder)
        {
            _purchaseOrderRepository.UpdatePurchaseOrder(purchaseOrder);
        }

        public void DeletePurchaseOrder(int purchaseOrderId)
        {
            _purchaseOrderRepository.DeletePurchaseOrder(purchaseOrderId);
        }
    }
}

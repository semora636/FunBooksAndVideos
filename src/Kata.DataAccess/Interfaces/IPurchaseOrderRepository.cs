using Kata.Domain.Entities;
using Microsoft.Data.SqlClient;

namespace Kata.DataAccess.Interfaces
{
    public interface IPurchaseOrderRepository
    {
        PurchaseOrder? GetPurchaseOrderById(int purchaseOrderId);
        IEnumerable<PurchaseOrder> GetAllPurchaseOrders();
        int AddPurchaseOrder(PurchaseOrder purchaseOrder, SqlTransaction transaction, SqlConnection connection);
        void UpdatePurchaseOrder(PurchaseOrder purchaseOrder, SqlTransaction transaction, SqlConnection connection);
        void DeletePurchaseOrder(int purchaseOrderId, SqlTransaction transaction, SqlConnection connection);
    }
}

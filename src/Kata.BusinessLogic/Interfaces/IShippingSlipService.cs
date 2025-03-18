using Kata.Domain.Entities;
using Microsoft.Data.SqlClient;

namespace Kata.BusinessLogic.Interfaces
{
    public interface IShippingSlipService
    {
        IEnumerable<ShippingSlip> GetShippingSlipsByPurchaseOrderId(int purchaseOrderId);
        void GenerateShippingSlip(PurchaseOrder purchaseOrder, SqlConnection connection, SqlTransaction transaction);
    }
}

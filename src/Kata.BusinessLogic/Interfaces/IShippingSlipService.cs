using Kata.Domain.Entities;
using Microsoft.Data.SqlClient;

namespace Kata.BusinessLogic.Interfaces
{
    public interface IShippingSlipService
    {
        Task<IEnumerable<ShippingSlip>> GetShippingSlipsByPurchaseOrderIdAsync(int purchaseOrderId);
        Task GenerateShippingSlipAsync(PurchaseOrder purchaseOrder, SqlConnection connection, SqlTransaction transaction);
    }
}

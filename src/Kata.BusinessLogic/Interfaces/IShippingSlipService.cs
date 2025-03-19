using Kata.Domain.Entities;
using System.Data;

namespace Kata.BusinessLogic.Interfaces
{
    public interface IShippingSlipService
    {
        Task<IEnumerable<ShippingSlip>> GetShippingSlipsByPurchaseOrderIdAsync(int purchaseOrderId);
        Task GenerateShippingSlipAsync(PurchaseOrder purchaseOrder, IDbConnection connection, IDbTransaction transaction);
    }
}

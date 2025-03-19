using Kata.Domain.Entities;
using System.Data;

namespace Kata.DataAccess.Interfaces
{
    public interface IShippingSlipRepository
    {
        Task<IEnumerable<ShippingSlip>> GetShippingSlipsByPurchaseOrderIdAsync(int purchaseOrderId);
        Task AddShippingSlipAsync(ShippingSlip shippingSlip, IDbTransaction transaction, IDbConnection connection);
    }
}

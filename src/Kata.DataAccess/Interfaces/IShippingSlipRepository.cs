using Kata.Domain.Entities;
using Microsoft.Data.SqlClient;

namespace Kata.DataAccess.Interfaces
{
    public interface IShippingSlipRepository
    {
        Task<IEnumerable<ShippingSlip>> GetShippingSlipsByPurchaseOrderIdAsync(int purchaseOrderId);
        Task AddShippingSlipAsync(ShippingSlip shippingSlip, SqlTransaction transaction, SqlConnection connection);
    }
}

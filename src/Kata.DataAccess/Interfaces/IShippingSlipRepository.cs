using Kata.Domain.Entities;
using Microsoft.Data.SqlClient;

namespace Kata.DataAccess.Interfaces
{
    public interface IShippingSlipRepository
    {
        IEnumerable<ShippingSlip> GetShippingSlipsByPurchaseOrderId(int purchaseOrderId);
        void AddShippingSlip(ShippingSlip shippingSlip, SqlTransaction transaction, SqlConnection connection);
    }
}

using Dapper;
using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;
using Microsoft.Data.SqlClient;

namespace Kata.DataAccess.Repositories
{
    public class ShippingSlipRepository : IShippingSlipRepository
    {
        private readonly SqlDataAccess _dataAccess;

        public ShippingSlipRepository(SqlDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public IEnumerable<ShippingSlip> GetShippingSlipsByPurchaseOrderId(int purchaseOrderId)
        {
            using var connection = _dataAccess.CreateConnection();
            return connection.Query<ShippingSlip>("SELECT * FROM ShippingSlips WHERE PurchaseOrderId = @PurchaseOrderId", new { PurchaseOrderId = purchaseOrderId }).ToList();
        }

        public void AddShippingSlip(ShippingSlip shippingSlip, SqlTransaction transaction, SqlConnection connection)
        {
            shippingSlip.ShippingSlipId = connection.ExecuteScalar<int>("INSERT INTO ShippingSlips (PurchaseOrderId, RecipientAddress) VALUES (@PurchaseOrderId, @RecipientAddress); SELECT SCOPE_IDENTITY();", shippingSlip, transaction);
        }
    }
}

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

        public async Task<IEnumerable<ShippingSlip>> GetShippingSlipsByPurchaseOrderIdAsync(int purchaseOrderId)
        {
            using var connection = _dataAccess.CreateConnection();
            return await connection.QueryAsync<ShippingSlip>("SELECT * FROM ShippingSlips WHERE PurchaseOrderId = @PurchaseOrderId", new { PurchaseOrderId = purchaseOrderId });
        }

        public async Task AddShippingSlipAsync(ShippingSlip shippingSlip, SqlTransaction transaction, SqlConnection connection)
        {
            shippingSlip.ShippingSlipId = await connection.ExecuteScalarAsync<int>("INSERT INTO ShippingSlips (PurchaseOrderId, RecipientAddress) VALUES (@PurchaseOrderId, @RecipientAddress); SELECT SCOPE_IDENTITY();", shippingSlip, transaction);
        }
    }
}

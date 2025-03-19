using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;
using System.Data;

namespace Kata.DataAccess.Repositories
{
    public class ShippingSlipRepository : IShippingSlipRepository
    {
        private readonly ISqlDataAccess _dataAccess;
        private readonly IDapperWrapper _dapperWrapper;

        public ShippingSlipRepository(ISqlDataAccess dataAccess, IDapperWrapper dapperWrapper)
        {
            _dataAccess = dataAccess;
            _dapperWrapper = dapperWrapper;
        }

        public async Task<IEnumerable<ShippingSlip>> GetShippingSlipsByPurchaseOrderIdAsync(int purchaseOrderId)
        {
            using var connection = _dataAccess.CreateConnection();
            return await _dapperWrapper.QueryAsync<ShippingSlip>(connection, "SELECT * FROM ShippingSlips WHERE PurchaseOrderId = @PurchaseOrderId", new { PurchaseOrderId = purchaseOrderId });
        }

        public async Task AddShippingSlipAsync(ShippingSlip shippingSlip, IDbTransaction transaction, IDbConnection connection)
        {
            shippingSlip.ShippingSlipId = await _dapperWrapper.ExecuteScalarAsync<int>(connection, "INSERT INTO ShippingSlips (PurchaseOrderId, RecipientAddress) VALUES (@PurchaseOrderId, @RecipientAddress); SELECT SCOPE_IDENTITY();", shippingSlip, transaction);
        }
    }
}

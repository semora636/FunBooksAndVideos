using Kata.BusinessLogic.Interfaces;
using Kata.Domain.Entities;
using Kata.Domain.Enums;
using System.Data;

namespace Kata.BusinessLogic.Services
{
    public class ShippableProductProcessor : IProductProcessor
    {
        private readonly IShippingSlipService _shippingSlipService;

        public ShippableProductProcessor(IShippingSlipService shippingSlipService)
        {
            _shippingSlipService = shippingSlipService;
        }

        public async Task ProcessProductAsync(PurchaseOrder purchaseOrder, OrderItem item, IDbConnection connection, IDbTransaction transaction)
        {
            await _shippingSlipService.GenerateShippingSlipAsync(purchaseOrder, connection, transaction);
        }

        public ProductType ProductType => ProductType.Book;
    }
}

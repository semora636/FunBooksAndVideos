using Kata.BusinessLogic.Interfaces;
using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;
using System.Data;

namespace Kata.BusinessLogic.Services
{
    public class ShippingSlipService : IShippingSlipService
    {
        private readonly IShippingSlipRepository _shippingSlipRepository;
        private readonly ICustomerRepository _customerRepository;

        public ShippingSlipService(IShippingSlipRepository shippingSlipRepository, ICustomerRepository customerRepository)
        {
            _shippingSlipRepository = shippingSlipRepository;
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<ShippingSlip>> GetShippingSlipsByPurchaseOrderIdAsync(int purchaseOrderId)
        {
            return await _shippingSlipRepository.GetShippingSlipsByPurchaseOrderIdAsync(purchaseOrderId);
        }

        public async Task GenerateShippingSlipAsync(PurchaseOrder purchaseOrder, IDbConnection connection, IDbTransaction transaction)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(purchaseOrder.CustomerId);
            if (customer != null)
            {
                var shippingSlip = new ShippingSlip()
                {
                    PurchaseOrderId = purchaseOrder.PurchaseOrderId,
                    RecipientAddress = customer.Address,
                };
                await _shippingSlipRepository.AddShippingSlipAsync(shippingSlip, transaction, connection);

                if (purchaseOrder.ShippingSlips == null)
                {
                    purchaseOrder.ShippingSlips = [];
                }
                purchaseOrder.ShippingSlips.Add(shippingSlip);
            }
        }
    }
}

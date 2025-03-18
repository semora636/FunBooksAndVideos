using Kata.BusinessLogic.Interfaces;
using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;
using Microsoft.Data.SqlClient;

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

        public IEnumerable<ShippingSlip> GetShippingSlipsByPurchaseOrderId(int purchaseOrderId)
        {
            return _shippingSlipRepository.GetShippingSlipsByPurchaseOrderId(purchaseOrderId);
        }

        public void GenerateShippingSlip(PurchaseOrder purchaseOrder, SqlConnection connection, SqlTransaction transaction)
        {
            var customer = _customerRepository.GetCustomerById(purchaseOrder.CustomerId);
            if (customer != null)
            {
                var shippingSlip = new ShippingSlip()
                {
                    PurchaseOrderId = purchaseOrder.PurchaseOrderId,
                    RecipientAddress = customer.Address,
                };
                _shippingSlipRepository.AddShippingSlip(shippingSlip, transaction, connection);

                if (purchaseOrder.ShippingSlips == null)
                {
                    purchaseOrder.ShippingSlips = [];
                }
                purchaseOrder.ShippingSlips.Add(shippingSlip);
            }
        }
    }
}

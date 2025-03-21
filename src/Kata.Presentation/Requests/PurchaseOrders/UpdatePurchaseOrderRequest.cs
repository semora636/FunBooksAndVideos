using Kata.Domain.Entities;
using MediatR;

namespace Kata.Presentation.Requests.PurchaseOrders
{
    public class UpdatePurchaseOrderRequest : IRequest
    {
        public PurchaseOrder PurchaseOrder { get; set; }

        public UpdatePurchaseOrderRequest(PurchaseOrder purchaseOrder)
        {
            PurchaseOrder = purchaseOrder;
        }
    }
}

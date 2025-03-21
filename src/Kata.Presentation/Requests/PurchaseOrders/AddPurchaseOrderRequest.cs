using Kata.Domain.Entities;
using MediatR;

namespace Kata.Presentation.Requests.PurchaseOrders
{
    public class AddPurchaseOrderRequest : IRequest<PurchaseOrder>
    {
        public PurchaseOrder PurchaseOrder { get; set; }
    }
}

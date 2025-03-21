using Kata.Domain.Entities;
using MediatR;

namespace Kata.Presentation.Requests.PurchaseOrders
{
    public class UpdatePurchaseOrderRequest : IRequest
    {
        public int Id { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }
    }
}

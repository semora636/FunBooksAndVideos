using MediatR;

namespace Kata.Presentation.Requests.PurchaseOrders
{
    public class DeletePurchaseOrderRequest : IRequest
    {
        public int Id { get; set; }
    }
}

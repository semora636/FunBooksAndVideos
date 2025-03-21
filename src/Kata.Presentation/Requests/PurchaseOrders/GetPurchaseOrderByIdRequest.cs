using Kata.Domain.Entities;
using MediatR;

namespace Kata.Presentation.Requests.PurchaseOrders
{
    public class GetPurchaseOrderByIdRequest : IRequest<PurchaseOrder?>
    {
        public int Id { get; set; }
    }
}

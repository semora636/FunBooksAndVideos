using Kata.Domain.Entities;
using MediatR;

namespace Kata.Presentation.Requests.PurchaseOrders
{
    public class GetAllPurchaseOrdersRequest : IRequest<IEnumerable<PurchaseOrder>>
    {
    }
}

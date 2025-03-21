using Kata.BusinessLogic.Interfaces;
using Kata.Domain.Entities;
using Kata.Presentation.Requests.PurchaseOrders;
using MediatR;

namespace Kata.Presentation.Handlers.PurchaseOrders
{
    public class GetAllPurchaseOrdersHandler : IRequestHandler<GetAllPurchaseOrdersRequest, IEnumerable<PurchaseOrder>>
    {
        private readonly IPurchaseOrderService _purchaseOrderService;

        public GetAllPurchaseOrdersHandler(IPurchaseOrderService purchaseOrderService)
        {
            _purchaseOrderService = purchaseOrderService;
        }

        public async Task<IEnumerable<PurchaseOrder>> Handle(GetAllPurchaseOrdersRequest request, CancellationToken cancellationToken)
        {
            return await _purchaseOrderService.GetAllPurchaseOrdersAsync();
        }
    }
}

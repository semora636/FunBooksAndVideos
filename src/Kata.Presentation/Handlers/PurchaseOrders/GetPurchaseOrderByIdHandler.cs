using Kata.BusinessLogic.Interfaces;
using Kata.Domain.Entities;
using Kata.Presentation.Requests.PurchaseOrders;
using MediatR;

namespace Kata.Presentation.Handlers.PurchaseOrders
{
    public class GetPurchaseOrderByIdHandler : IRequestHandler<GetPurchaseOrderByIdRequest, PurchaseOrder?>
    {
        private readonly IPurchaseOrderService _purchaseOrderService;

        public GetPurchaseOrderByIdHandler(IPurchaseOrderService purchaseOrderService)
        {
            _purchaseOrderService = purchaseOrderService;
        }

        public async Task<PurchaseOrder?> Handle(GetPurchaseOrderByIdRequest request, CancellationToken cancellationToken)
        {
            return await _purchaseOrderService.GetPurchaseOrderByIdAsync(request.Id);
        }
    }
}

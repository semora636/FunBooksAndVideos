using Kata.BusinessLogic.Interfaces;
using Kata.Presentation.Requests.PurchaseOrders;
using MediatR;

namespace Kata.Presentation.Handlers.PurchaseOrders
{
    public class UpdatePurchaseOrderHandler : IRequestHandler<UpdatePurchaseOrderRequest>
    {
        private readonly IPurchaseOrderService _purchaseOrderService;

        public UpdatePurchaseOrderHandler(IPurchaseOrderService purchaseOrderService)
        {
            _purchaseOrderService = purchaseOrderService;
        }

        public async Task Handle(UpdatePurchaseOrderRequest request, CancellationToken cancellationToken)
        {
            await _purchaseOrderService.UpdatePurchaseOrderAsync(request.PurchaseOrder);
        }
    }
}

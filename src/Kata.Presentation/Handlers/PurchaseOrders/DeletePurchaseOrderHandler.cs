using Kata.BusinessLogic.Interfaces;
using Kata.Presentation.Requests.PurchaseOrders;
using MediatR;

namespace Kata.Presentation.Handlers.PurchaseOrders
{
    public class DeletePurchaseOrderHandler : IRequestHandler<DeletePurchaseOrderRequest>
    {
        private readonly IPurchaseOrderService _purchaseOrderService;

        public DeletePurchaseOrderHandler(IPurchaseOrderService purchaseOrderService)
        {
            _purchaseOrderService = purchaseOrderService;
        }

        public async Task Handle(DeletePurchaseOrderRequest request, CancellationToken cancellationToken)
        {
            _ = await _purchaseOrderService.GetPurchaseOrderByIdAsync(request.Id) ?? throw new KeyNotFoundException($"PurchaseOrder with ID {request.Id} not found.");
            await _purchaseOrderService.DeletePurchaseOrderAsync(request.Id);
        }
    }
}

using Kata.BusinessLogic.Interfaces;
using Kata.Domain.Entities;
using Kata.Presentation.Requests.PurchaseOrders;
using MediatR;

namespace Kata.Presentation.Handlers.PurchaseOrders
{
    public class AddPurchaseOrderHandler : IRequestHandler<AddPurchaseOrderRequest, PurchaseOrder>
    {
        private readonly IPurchaseOrderService _purchaseOrderService;

        public AddPurchaseOrderHandler(IPurchaseOrderService purchaseOrderService)
        {
            _purchaseOrderService = purchaseOrderService;
        }

        public async Task<PurchaseOrder> Handle(AddPurchaseOrderRequest request, CancellationToken cancellationToken)
        {
            await _purchaseOrderService.AddPurchaseOrderAsync(request.PurchaseOrder);
            return request.PurchaseOrder;
        }
    }
}

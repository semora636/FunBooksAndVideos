using Kata.BusinessLogic.Interfaces;
using Kata.Domain.Entities;
using Kata.Presentation.Requests.PurchaseOrders;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kata.Presentation.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseOrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PurchaseOrderController> _logger;

        public PurchaseOrderController(IMediator mediator, ILogger<PurchaseOrderController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseOrder>>> GetAllPurchaseOrdersAsync()
        {
            var purchaseOrders = await _mediator.Send(new GetAllPurchaseOrdersRequest());
            return Ok(purchaseOrders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PurchaseOrder>> GetPurchaseOrderByIdAsync(int id)
        {
            var purchaseOrder = await _mediator.Send(new GetPurchaseOrderByIdRequest { Id = id });

            if (purchaseOrder == null)
            {
                return NotFound();
            }

            return Ok(purchaseOrder);
        }

        [HttpPost]
        public async Task<ActionResult<PurchaseOrder>> AddPurchaseOrderAsync([FromBody] PurchaseOrder purchaseOrder)
        {
            await _mediator.Send(new AddPurchaseOrderRequest(purchaseOrder));
            return CreatedAtAction(nameof(GetPurchaseOrderByIdAsync), new { id = purchaseOrder.PurchaseOrderId }, purchaseOrder);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePurchaseOrderAsync(int id, [FromBody] PurchaseOrder purchaseOrder)
        {
            if (id != purchaseOrder.PurchaseOrderId)
            {
                return BadRequest("PurchaseOrderId in the request body must match the ID in the URL.");
            }

            await _mediator.Send(new UpdatePurchaseOrderRequest(purchaseOrder));
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurchaseOrderAsync(int id)
        {
            await _mediator.Send(new DeletePurchaseOrderRequest { Id = id });
            return NoContent();
        }
    }
}

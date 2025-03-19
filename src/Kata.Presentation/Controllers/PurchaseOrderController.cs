using Kata.BusinessLogic.Interfaces;
using Kata.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Kata.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseOrderController : ControllerBase
    {
        private readonly IPurchaseOrderService _purchaseOrderService;
        private readonly ILogger<PurchaseOrderController> _logger;

        public PurchaseOrderController(IPurchaseOrderService purchaseOrderService, ILogger<PurchaseOrderController> logger)
        {
            _purchaseOrderService = purchaseOrderService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseOrder>>> GetAllPurchaseOrdersAsync()
        {
            var purchaseOrders = await _purchaseOrderService.GetAllPurchaseOrdersAsync();
            return Ok(purchaseOrders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PurchaseOrder>> GetPurchaseOrderByIdAsync(int id)
        {
            var purchaseOrder = await _purchaseOrderService.GetPurchaseOrderByIdAsync(id);

            if (purchaseOrder == null)
            {
                return NotFound();
            }

            return Ok(purchaseOrder);
        }

        [HttpPost]
        public async Task<ActionResult<PurchaseOrder>> AddPurchaseOrderAsync([FromBody] PurchaseOrder purchaseOrder)
        {
            await _purchaseOrderService.AddPurchaseOrderAsync(purchaseOrder);
            return CreatedAtAction(nameof(GetPurchaseOrderByIdAsync), new { id = purchaseOrder.PurchaseOrderId }, purchaseOrder);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePurchaseOrderAsync(int id, [FromBody] PurchaseOrder purchaseOrder)
        {
            if (id != purchaseOrder.PurchaseOrderId)
            {
                return BadRequest("PurchaseOrderId in the request body must match the ID in the URL.");
            }

            await _purchaseOrderService.UpdatePurchaseOrderAsync(purchaseOrder);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurchaseOrderAsync(int id)
        {
            await _purchaseOrderService.DeletePurchaseOrderAsync(id);
            return NoContent();
        }
    }
}

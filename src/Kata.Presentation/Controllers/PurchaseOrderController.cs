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
        public ActionResult<IEnumerable<PurchaseOrder>> GetAllPurchaseOrders()
        {
            var purchaseOrders = _purchaseOrderService.GetAllPurchaseOrders();
            return Ok(purchaseOrders);
        }

        [HttpGet("{id}")]
        public ActionResult<PurchaseOrder> GetPurchaseOrderById(int id)
        {
            var purchaseOrder = _purchaseOrderService.GetPurchaseOrderById(id);

            if (purchaseOrder == null)
            {
                return NotFound();
            }

            return Ok(purchaseOrder);
        }

        [HttpPost]
        public ActionResult<PurchaseOrder> AddPurchaseOrder([FromBody] PurchaseOrder purchaseOrder)
        {
            _purchaseOrderService.AddPurchaseOrder(purchaseOrder);
            return CreatedAtAction(nameof(GetPurchaseOrderById), new { id = purchaseOrder.PurchaseOrderId }, purchaseOrder);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePurchaseOrder(int id, [FromBody] PurchaseOrder purchaseOrder)
        {
            if (id != purchaseOrder.PurchaseOrderId)
            {
                return BadRequest("PurchaseOrderId in the request body must match the ID in the URL.");
            }

            _purchaseOrderService.UpdatePurchaseOrder(purchaseOrder);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePurchaseOrder(int id)
        {
            _purchaseOrderService.DeletePurchaseOrder(id);
            return NoContent();
        }
    }
}

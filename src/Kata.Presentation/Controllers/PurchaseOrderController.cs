﻿using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Kata.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseOrderController : ControllerBase
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly ILogger<PurchaseOrderController> _logger;

        public PurchaseOrderController(IPurchaseOrderRepository purchaseOrderRepository, ILogger<PurchaseOrderController> logger)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PurchaseOrder>> GetAllPurchaseOrders()
        {
            try
            {
                var purchaseOrders = _purchaseOrderRepository.GetAllPurchaseOrders();
                return Ok(purchaseOrders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all purchase orders.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<PurchaseOrder> GetPurchaseOrderById(int id)
        {
            try
            {
                var purchaseOrder = _purchaseOrderRepository.GetPurchaseOrderById(id);

                if (purchaseOrder == null)
                {
                    return NotFound();
                }

                return Ok(purchaseOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving purchase order with ID {id}.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        public ActionResult<PurchaseOrder> AddPurchaseOrder([FromBody] PurchaseOrder purchaseOrder)
        {
            try
            {
                _purchaseOrderRepository.AddPurchaseOrder(purchaseOrder);
                return CreatedAtAction(nameof(GetPurchaseOrderById), new { id = purchaseOrder.PurchaseOrderId }, purchaseOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding a new purchase order.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePurchaseOrder(int id, [FromBody] PurchaseOrder purchaseOrder)
        {
            try
            {
                if (id != purchaseOrder.PurchaseOrderId)
                {
                    return BadRequest("PurchaseOrderId in the request body must match the ID in the URL.");
                }

                _purchaseOrderRepository.UpdatePurchaseOrder(purchaseOrder);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating purchase order with ID {id}.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePurchaseOrder(int id)
        {
            try
            {
                _purchaseOrderRepository.DeletePurchaseOrder(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting purchase order with ID {id}.");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}

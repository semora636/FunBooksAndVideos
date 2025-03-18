using Kata.BusinessLogic.Interfaces;
using Kata.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Kata.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipProductController : ControllerBase
    {
        private readonly IMembershipProductService _membershipProductService;
        private readonly ILogger<MembershipProductController> _logger;

        public MembershipProductController(IMembershipProductService membershipProductService, ILogger<MembershipProductController> logger)
        {
            _membershipProductService = membershipProductService;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<MembershipProduct>> GetAllMembershipProducts()
        {
            try
            {
                var membershipProducts = _membershipProductService.GetAllMembershipProducts();
                return Ok(membershipProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all membershipProducts.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<MembershipProduct> GetMembershipProductById(int id)
        {
            try
            {
                var membershipProduct = _membershipProductService.GetMembershipProductById(id);

                if (membershipProduct == null)
                {
                    return NotFound();
                }

                return Ok(membershipProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving membershipProduct with ID {id}.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        public ActionResult<MembershipProduct> AddMembershipProduct([FromBody] MembershipProduct membershipProduct)
        {
            try
            {
                _membershipProductService.AddMembershipProduct(membershipProduct);
                return CreatedAtAction(nameof(GetMembershipProductById), new { id = membershipProduct.MembershipProductId }, membershipProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding a new membershipProduct.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateMembershipProduct(int id, [FromBody] MembershipProduct membershipProduct)
        {
            try
            {
                if (id != membershipProduct.MembershipProductId)
                {
                    return BadRequest("MembershipProductId in the request body must match the id in the URL.");
                }

                _membershipProductService.UpdateMembershipProduct(membershipProduct);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating membershipProduct with ID {id}.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMembershipProduct(int id)
        {
            try
            {
                _membershipProductService.DeleteMembershipProduct(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting membershipProduct with ID {id}.");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}

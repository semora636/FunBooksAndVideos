using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Kata.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipProductController : ControllerBase
    {
        private readonly IMembershipProductRepository _membershipProductRepository;
        private readonly ILogger<MembershipProductController> _logger;

        public MembershipProductController(IMembershipProductRepository membershipProductRepository, ILogger<MembershipProductController> logger)
        {
            _membershipProductRepository = membershipProductRepository;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<MembershipProduct>> GetAllMembershipProducts()
        {
            try
            {
                var membershipProducts = _membershipProductRepository.GetAllMembershipProducts();
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
                var membershipProduct = _membershipProductRepository.GetMembershipProductById(id);

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
                _membershipProductRepository.AddMembershipProduct(membershipProduct);
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

                _membershipProductRepository.UpdateMembershipProduct(membershipProduct);
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
                _membershipProductRepository.DeleteMembershipProduct(id);
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

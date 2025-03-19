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
        public async Task<ActionResult<IEnumerable<MembershipProduct>>> GetAllMembershipProductsAsync()
        {
            var membershipProducts = await _membershipProductService.GetAllMembershipProductsAsync();
            return Ok(membershipProducts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MembershipProduct>> GetMembershipProductByIdAsync(int id)
        {
            var membershipProduct = await _membershipProductService.GetMembershipProductByIdAsync(id);

            if (membershipProduct == null)
            {
                return NotFound();
            }

            return Ok(membershipProduct);
        }

        [HttpPost]
        public async Task<ActionResult<MembershipProduct>> AddMembershipProductAsync([FromBody] MembershipProduct membershipProduct)
        {
            await _membershipProductService.AddMembershipProductAsync(membershipProduct);
            return CreatedAtAction(nameof(GetMembershipProductByIdAsync), new { id = membershipProduct.MembershipProductId }, membershipProduct);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMembershipProductAsync(int id, [FromBody] MembershipProduct membershipProduct)
        {
            if (id != membershipProduct.MembershipProductId)
            {
                return BadRequest("MembershipProductId in the request body must match the id in the URL.");
            }

            await _membershipProductService.UpdateMembershipProductAsync(membershipProduct);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMembershipProductAsync(int id)
        {
            await _membershipProductService.DeleteMembershipProductAsync(id);
            return NoContent();
        }
    }
}

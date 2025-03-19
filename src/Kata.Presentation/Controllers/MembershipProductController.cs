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
            var membershipProducts = _membershipProductService.GetAllMembershipProducts();
            return Ok(membershipProducts);
        }

        [HttpGet("{id}")]
        public ActionResult<MembershipProduct> GetMembershipProductById(int id)
        {
            var membershipProduct = _membershipProductService.GetMembershipProductById(id);

            if (membershipProduct == null)
            {
                return NotFound();
            }

            return Ok(membershipProduct);
        }

        [HttpPost]
        public ActionResult<MembershipProduct> AddMembershipProduct([FromBody] MembershipProduct membershipProduct)
        {
            _membershipProductService.AddMembershipProduct(membershipProduct);
            return CreatedAtAction(nameof(GetMembershipProductById), new { id = membershipProduct.MembershipProductId }, membershipProduct);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateMembershipProduct(int id, [FromBody] MembershipProduct membershipProduct)
        {
            if (id != membershipProduct.MembershipProductId)
            {
                return BadRequest("MembershipProductId in the request body must match the id in the URL.");
            }

            _membershipProductService.UpdateMembershipProduct(membershipProduct);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMembershipProduct(int id)
        {
            _membershipProductService.DeleteMembershipProduct(id);
            return NoContent();
        }
    }
}

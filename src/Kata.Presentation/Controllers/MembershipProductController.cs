using Kata.Domain.Entities;
using Kata.Presentation.Requests.MembershipProducts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kata.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<MembershipProductController> _logger;

        public MembershipProductController(IMediator mediator, ILogger<MembershipProductController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MembershipProduct>>> GetAllMembershipProductsAsync()
        {
            var membershipProducts = await _mediator.Send(new GetAllMembershipProductsRequest());
            return Ok(membershipProducts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MembershipProduct>> GetMembershipProductByIdAsync(int id)
        {
            var membershipProduct = await _mediator.Send(new GetMembershipProductByIdRequest { Id = id });

            if (membershipProduct == null)
            {
                return NotFound();
            }

            return Ok(membershipProduct);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<MembershipProduct>> AddMembershipProductAsync([FromBody] MembershipProduct membershipProduct)
        {
            await _mediator.Send(new AddMembershipProductRequest { MembershipProduct = membershipProduct });
            return CreatedAtAction(nameof(GetMembershipProductByIdAsync), new { id = membershipProduct.MembershipProductId }, membershipProduct);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMembershipProductAsync(int id, [FromBody] MembershipProduct membershipProduct)
        {
            if (id != membershipProduct.MembershipProductId)
            {
                return BadRequest("MembershipProductId in the request body must match the id in the URL.");
            }

            await _mediator.Send(new UpdateMembershipProductRequest { Id = id, MembershipProduct = membershipProduct });
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMembershipProductAsync(int id)
        {
            await _mediator.Send(new DeleteMembershipProductRequest { Id = id });
            return NoContent();
        }
    }
}

using Kata.BusinessLogic.Interfaces;
using Kata.BusinessLogic.Services;
using Kata.Domain.Entities;
using Kata.Presentation.Handlers.Customers;
using Kata.Presentation.Requests.Customers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kata.Presentation.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(IMediator mediator, ILogger<CustomerController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetAllCustomersAsync()
        {
            var customers = await _mediator.Send(new GetAllCustomersRequest());
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomerByIdAsync(int id)
        {
            var customer = await _mediator.Send(new GetCustomerByIdRequest { Id = id });

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [HttpGet("{id}/memberships")]
        public async Task<ActionResult<IEnumerable<Membership>>> GetMembershipsByCustomerIdAsync(int id)
        {
            var memberships = await _mediator.Send(new GetMembershipsByCustomerIdRequest { Id = id });
            return Ok(memberships);
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> AddCustomerAsync([FromBody] Customer customer)
        {
            if (customer == null)
            {
                return BadRequest("Customer object is null.");
            }

            await _mediator.Send(new AddCustomerRequest { Customer = customer });
            return CreatedAtAction(nameof(GetCustomerByIdAsync), new { id = customer.CustomerId }, customer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomerAsync(int id, [FromBody] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return BadRequest("CustomerId in the request body must match the ID in the URL.");
            }

            await _mediator.Send(new UpdateCustomerRequest { Customer = customer });
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            await _mediator.Send(new DeleteCustomerRequest { Id = id });
            return NoContent();
        }
    }
}

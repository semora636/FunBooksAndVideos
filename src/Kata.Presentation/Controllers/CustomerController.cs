using Kata.BusinessLogic.Interfaces;
using Kata.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Kata.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _customerService;
        private readonly IMembershipService _membershipService;

        public CustomerController(ILogger<CustomerController> logger, ICustomerService customerService, IMembershipService membershipService)
        {
            _logger = logger;
            _customerService = customerService;
            _membershipService = membershipService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Customer>> GetAllCustomers()
        {
            var customers = _customerService.GetAllCustomers();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public ActionResult<Customer> GetCustomerById(int id)
        {
            var customer = _customerService.GetCustomerById(id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [HttpGet("{id}/memberships")]
        public ActionResult<IEnumerable<PurchaseOrder>> GetMembershipsByCustomerId(int id)
        {
            var memberships = _membershipService.GetMembershipsByCustomerId(id);
            return Ok(memberships);
        }

        [HttpPost]
        public ActionResult<Customer> AddCustomer([FromBody] Customer customer)
        {
            _customerService.AddCustomer(customer);
            return CreatedAtAction(nameof(GetCustomerById), new { id = customer.CustomerId }, customer);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCustomer(int id, [FromBody] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return BadRequest("CustomerId in the request body must match the ID in the URL.");
            }

            _customerService.UpdateCustomer(customer);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(int id)
        {
            _customerService.DeleteCustomer(id);
            return NoContent();
        }
    }
}

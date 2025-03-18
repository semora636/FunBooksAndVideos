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
            try
            {
                var customers = _customerService.GetAllCustomers();
                return Ok(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all customers.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Customer> GetCustomerById(int id)
        {
            try
            {
                var customer = _customerService.GetCustomerById(id);

                if (customer == null)
                {
                    return NotFound();
                }

                return Ok(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving customer with ID {id}.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{id}/memberships")]
        public ActionResult<IEnumerable<PurchaseOrder>> GetMembershipsByCustomerId(int id)
        {
            try
            {
                var memberships = _membershipService.GetMembershipsByCustomerId(id);
                return Ok(memberships);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all purchase orders.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        public ActionResult<Customer> AddCustomer([FromBody] Customer customer)
        {
            try
            {
                _customerService.AddCustomer(customer);
                return CreatedAtAction(nameof(GetCustomerById), new { id = customer.CustomerId }, customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding a new customer.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCustomer(int id, [FromBody] Customer customer)
        {
            try
            {
                if (id != customer.CustomerId)
                {
                    return BadRequest("CustomerId in the request body must match the ID in the URL.");
                }

                _customerService.UpdateCustomer(customer);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating customer with ID {id}.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(int id)
        {
            try
            {
                _customerService.DeleteCustomer(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting customer with ID {id}.");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}

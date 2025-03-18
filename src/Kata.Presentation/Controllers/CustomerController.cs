using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Kata.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ICustomerRepository customerRepository, ILogger<CustomerController> logger)
        {
            _customerRepository = customerRepository;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Customer>> GetAllCustomers()
        {
            try
            {
                var customers = _customerRepository.GetAllCustomers();
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
                var customer = _customerRepository.GetCustomerById(id);

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

        [HttpPost]
        public ActionResult<Customer> AddCustomer([FromBody] Customer customer)
        {
            try
            {
                _customerRepository.AddCustomer(customer);
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

                _customerRepository.UpdateCustomer(customer);
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
                _customerRepository.DeleteCustomer(id);
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

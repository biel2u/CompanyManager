using CompanyManager.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManager.Server.Features.Customers
{
    [Route("[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public IActionResult GetCustomers()
        {
            var customers = _customerService.GetCustomers();
            return Ok(customers);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerViewModel>> CreateCustomer([FromBody] CustomerViewModel customer)
        {
            if (!ModelState.IsValid || customer == null) return BadRequest(ModelState);

            var newCustomer = await _customerService.AddCustomer(customer); //add model validation
            return Created(nameof(CustomerViewModel), newCustomer);
        }
    }
}

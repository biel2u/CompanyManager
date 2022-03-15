using CompanyManager.Server.Services;
using CompanyManager.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManager.Server.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<ActionResult<List<string>>> SearchCustomers(string searchValue)
        {
            var customersSelectorList = await _customerService.SearchCustomers(searchValue);
            return Ok(customersSelectorList);
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

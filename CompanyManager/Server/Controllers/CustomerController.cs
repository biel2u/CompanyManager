using CompanyManager.Server.Services;
using CompanyManager.Server.Validators;
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
        private readonly ICustomerValidator _customerValidator;

        public CustomerController(ICustomerService customerService, ICustomerValidator customerValidator)
        {
            _customerService = customerService;
            _customerValidator = customerValidator;
        }

        [HttpGet]
        public async Task<ActionResult<List<string>>> SearchCustomers(string searchValue)
        {
            var customersSelectorList = await _customerService.SearchCustomers(searchValue);
            return Ok(customersSelectorList);
        }

        [HttpPost]
        public async Task<ActionResult<EditCustomerModel>> CreateCustomer([FromBody] EditCustomerModel customer)
        {
            await _customerValidator.SetModelStateErrors(customer, ModelState);
            if (ModelState.IsValid == false || customer == null || ModelState.ErrorCount > 0)
            {
                return BadRequest(ModelState);
            }
          
            await _customerService.AddCustomer(customer);
            return Ok(ModelState);
        }
    }
}

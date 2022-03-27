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
        public async Task<ActionResult<EditCustomerModel>> CreateCustomer([FromBody] EditCustomerModel customer)
        {
            if (!ModelState.IsValid || customer == null) return BadRequest(ModelState);
            if(await _customerService.IsPhoneNumberAlreadyExists(customer.Phone))
            {
                ModelState.AddModelError("PhoneNumberError", "Klient o podanym numerze telefonu już istnieje.");
                return BadRequest(ModelState);
            }

            await _customerService.AddCustomer(customer);
            return Ok(ModelState);
        }
    }
}

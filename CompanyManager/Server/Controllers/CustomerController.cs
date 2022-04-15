using CompanyManager.Core.Services;
using CompanyManager.Core.Validators;
using CompanyManager.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CompanyManager.Api.Controllers
{
    [Route("api/customer")]
    public class CustomerController : ApiControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ICustomerValidator _customerValidator;

        public CustomerController(ICustomerService customerService, ICustomerValidator customerValidator)
        {
            _customerService = customerService;
            _customerValidator = customerValidator;
        }

        [HttpGet("{searchValue}")]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<string>>> GetByValue(string searchValue)
        {
            var customers = await _customerService.SearchCustomers(searchValue);
            if(customers.Any() == false) return NoContent();

            return Ok(customers);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EditCustomerModel>> Create([FromBody] EditCustomerModel customer)
        {
            await _customerValidator.SetModelStateErrors(customer, ModelState);
            if (ModelState.IsValid == false || customer == null || ModelState.ErrorCount > 0)
            {
                return BadRequest(ModelState);
            }
          
            await _customerService.AddCustomer(customer);

            return Created("customer", ModelState);
        }
    }
}

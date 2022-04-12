using CompanyManager.Server.Repositories;
using CompanyManager.Shared;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CompanyManager.Server.Validators
{
    public interface ICustomerValidator
    {
        Task<ModelStateDictionary> SetModelStateErrors(EditCustomerModel customer, ModelStateDictionary modelState);
    }

    public class CustomerValidator : ICustomerValidator
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerValidator(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<ModelStateDictionary> SetModelStateErrors(EditCustomerModel customer, ModelStateDictionary modelState)
        {
            if (customer == null) return modelState;

            var errors = await ValidateCustomer(customer);            
            foreach (var error in errors)
            {
                modelState.AddModelError(error.Key, error.Value);
            }

            return modelState;
        }

        private async Task<Dictionary<string, string>> ValidateCustomer(EditCustomerModel customer)
        {
            var errors = new Dictionary<string, string>();
            var customers = await _customerRepository.GetCustomersByPhone(customer.Phone);

            if (customers.Any())
            {
                errors.Add("PhoneNumberError", "Klient o podanym numerze telefonu już istnieje.");
            }

            return errors;
        }
    }
}

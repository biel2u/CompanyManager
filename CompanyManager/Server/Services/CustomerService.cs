using AutoMapper;
using CompanyManager.Server.Models;
using CompanyManager.Server.Repositories;
using CompanyManager.Shared;

namespace CompanyManager.Server.Services
{
    public interface ICustomerService
    {
        Task<CustomerViewModel> AddCustomer(CustomerViewModel customerViewModel);
        Task<List<string>> SearchCustomers(string searchValue);
        Task<Customer?> GetCustomerByExtractedPhoneNumber(string customerNameAndPhone);
    }

    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<List<string>> SearchCustomers(string searchValue)
        {
            var customers = new List<Customer>();

            if (int.TryParse(searchValue, out _))
            {
                customers = await _customerRepository.GetCustomersByPhone(searchValue);
            }
            else
            {
                customers = await _customerRepository.GetCustomersByName(searchValue);
            }

            var customersSearchResult = new List<string>();
            foreach (var customer in customers)
            {
                customersSearchResult.Add($"{customer.Surname} {customer.Name} ({customer.Phone})");
            }

            return customersSearchResult;
        }

        public async Task<CustomerViewModel> AddCustomer(CustomerViewModel customerViewModel)
        {         
            var customerToCreate = _mapper.Map<Customer>(customerViewModel);
            var createdCustomer = await _customerRepository.AddCustomer(customerToCreate);
            var newCustomer = _mapper.Map<CustomerViewModel>(createdCustomer);

            return newCustomer;
        }

        public async Task<Customer?> GetCustomerByExtractedPhoneNumber(string customerNameAndPhone)
        {
            var start = customerNameAndPhone.IndexOf("(") + 1;
            var end = customerNameAndPhone.IndexOf(")", start);
            var phoneNumber = customerNameAndPhone.Substring(start, end - start);

            var customer = await _customerRepository.GetCustomerByPhone(phoneNumber);
            return customer;
        }
    }
}

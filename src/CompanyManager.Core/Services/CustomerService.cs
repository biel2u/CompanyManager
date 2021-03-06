using AutoMapper;
using CompanyManager.Core.Models;
using CompanyManager.Core.Repositories;
using CompanyManager.Shared;

namespace CompanyManager.Core.Services
{
    public interface ICustomerService
    {
        Task AddCustomer(EditCustomerModel customerViewModel);
        Task<List<string>> SearchCustomers(string searchValue);
        Task<Customer?> GetCustomerByExtractedPhoneNumber(string customerNameAndPhone);
        string CreateCustomerNameWithPhoneNumber(Customer customer);
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
                var customerNameWithPhoneNumber = CreateCustomerNameWithPhoneNumber(customer);
                customersSearchResult.Add(customerNameWithPhoneNumber);
            }

            return customersSearchResult;
        }

        public async Task AddCustomer(EditCustomerModel customerViewModel)
        {         
            var customerToCreate = _mapper.Map<Customer>(customerViewModel);
            await _customerRepository.AddCustomer(customerToCreate);
        }

        public async Task<Customer?> GetCustomerByExtractedPhoneNumber(string customerNameAndPhone)
        {
            try
            {
                var start = customerNameAndPhone.IndexOf("(") + 1;
                var end = customerNameAndPhone.IndexOf(")", start);
                var phoneNumber = customerNameAndPhone.Substring(start, end - start);

                var customer = await _customerRepository.GetCustomerByPhone(phoneNumber);
                return customer;
            }
            catch(Exception)
            {
                //log
                return new Customer();
            }
        }

        public string CreateCustomerNameWithPhoneNumber(Customer customer)
        {
            var customerNameWithPhoneNumber = $"{ customer.Surname } { customer.Name} ({ customer.Phone})";
            return customerNameWithPhoneNumber;
        }
    }
}

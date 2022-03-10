using AutoMapper;
using CompanyManager.Server.Models;
using CompanyManager.Shared;

namespace CompanyManager.Server.Features.Customers
{
    public interface ICustomerService
    {
        Task<CustomerViewModel> AddCustomer(CustomerViewModel customerViewModel);
        IEnumerable<CustomerViewModel> GetCustomers();
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

        public IEnumerable<CustomerViewModel> GetCustomers()
        {
            var customers = _customerRepository.GetCustomers();
            var customersCollection = new List<CustomerViewModel>();

            foreach (var customer in customers)
            {
                customersCollection.Add(new CustomerViewModel
                {
                    Name = customer.Name,
                    Surname = customer.Surname,
                    Email = customer.Email,
                    Phone = customer.Phone,
                    Note = customer.Note
                });
            }

            return customersCollection;
        }

        public async Task<CustomerViewModel> AddCustomer(CustomerViewModel customerViewModel)
        {         
            var customerToCreate = _mapper.Map<Customer>(customerViewModel);
            var createdCustomer = await _customerRepository.AddCustomer(customerToCreate);
            var newCustomer = _mapper.Map<CustomerViewModel>(createdCustomer);

            return newCustomer;
        }
    }
}

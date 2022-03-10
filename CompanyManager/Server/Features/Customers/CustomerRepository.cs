using CompanyManager.Server.Data;
using CompanyManager.Server.Models;

namespace CompanyManager.Server.Features.Customers
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetCustomers();
        Task<Customer> AddCustomer(Customer customer);
    }

    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CustomerRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Customer> GetCustomers()
        {
            return _dbContext.Customers;
        }

        public async Task<Customer> AddCustomer(Customer customer)
        {
            try
            {
                var newCustomer = _dbContext.Customers.Add(customer);
                await _dbContext.SaveChangesAsync();

                return newCustomer.Entity;
            }
            catch(Exception ex)
            {
                return new Customer();
            }
        }
    }
}

using CompanyManager.Server.Data;
using CompanyManager.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyManager.Server.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer> AddCustomer(Customer customer);
        Task<List<Customer>> SearchCustomersByName(string searchValue);
        Task<List<Customer>> SearchCustomersByPhone(string phoneNumber);
    }

    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CustomerRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Customer>> SearchCustomersByName(string searchValue)
        {           
            var customers = await _dbContext.Customers.
                Where(c => c.Name.Contains(searchValue, StringComparison.CurrentCultureIgnoreCase) || c.Surname.Contains(searchValue, StringComparison.CurrentCultureIgnoreCase))
                .OrderBy(c => c.Surname).Take(5).ToListAsync();           
            return customers;
        }

        public async Task<List<Customer>> SearchCustomersByPhone(string phoneNumber)
        {            
            var customers = await _dbContext.Customers.Where(c => c.Phone.Contains(phoneNumber)).OrderBy(c => c.Surname).Take(5).ToListAsync();    
            return customers;
        }

        public async Task<Customer> AddCustomer(Customer customer)
        {           
            var newCustomer = _dbContext.Customers.Add(customer);
            await _dbContext.SaveChangesAsync();

            return newCustomer.Entity;         
        }
    }
}

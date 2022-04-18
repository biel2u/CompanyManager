using CompanyManager.Core.Data;
using CompanyManager.Core.Models;

namespace CustomerManager.Api.IntegrationTests.Infrastructure
{
    public class ApplicationDbContextDataFeeder
    {
        public static void Feed(ApplicationDbContext dbContext)
        {
            var customer = new Customer
            {
                Id = 69,
                Name = "Frodo",
                Surname = "Baggins",
                Consent = new Consent(),
                Email = "shire@gondor.me",
                Phone = "123456789",
                Photos = new List<Photo>()
            };

            dbContext.Customers.Add(customer);

            dbContext.SaveChanges();
        }
    }
}

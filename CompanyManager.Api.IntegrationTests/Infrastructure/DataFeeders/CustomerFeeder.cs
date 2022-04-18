using CompanyManager.Core.Data;
using CompanyManager.Core.Models;

namespace CompanyManager.Api.IntegrationTests.Infrastructure.DataFeeders
{
    public static class CustomerFeeder
    {
        public static void Feed(ApplicationDbContext dbContext)
        {
            var customer = new Core.Models.Customer()
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
        }
    }
}

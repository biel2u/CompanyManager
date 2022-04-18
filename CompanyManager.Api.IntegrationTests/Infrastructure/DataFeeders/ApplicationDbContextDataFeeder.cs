using CompanyManager.Core.Data;

namespace CompanyManager.Api.IntegrationTests.Infrastructure.DataFeeders
{
    public class ApplicationDbContextDataFeeder
    {
        public static void Feed(ApplicationDbContext dbContext)
        {
            CustomerFeeder.Feed(dbContext);
            OfferFeeder.Feed(dbContext);

            dbContext.SaveChanges();
        }
    }
}

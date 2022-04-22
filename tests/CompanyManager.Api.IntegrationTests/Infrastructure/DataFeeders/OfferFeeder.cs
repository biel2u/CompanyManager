using CompanyManager.Core.Data;
using CompanyManager.Core.Models;

namespace CompanyManager.Api.IntegrationTests.Infrastructure.DataFeeders
{
    public static class OfferFeeder
    {
        public static void Feed(ApplicationDbContext dbContext)
        {
            var offerCategories = new List<OfferCategory>
            {
                new OfferCategory
                {
                    Id = 1,
                    Name = "Konsultacje"
                },
                new OfferCategory
                {
                    Id = 2,
                    Name = "Zabiegi terapeutyczne",
                },
            };

            dbContext.AddRange(offerCategories);

            var offers = new List<Core.Models.Offer>
            {
                new Core.Models.Offer
                {
                    Id = 1,
                    Name = "Konsultacje 15-minutowa",
                    Price = 0,
                    TimeInMinutes = 15,
                    OfferCategoryId = 1
                },
                new Core.Models.Offer
                {
                    Id = 2,
                    Name = "Konsultacja Beauty dla skór problematycznych",
                    Price = 250,
                    TimeInMinutes = 120,
                    OfferCategoryId = 1
                },
                new Core.Models.Offer
                {
                    Id = 3,
                    Name = "Konsultacja Beauty dla skór starzejących się",
                    Price = 200,
                    TimeInMinutes = 120,
                    OfferCategoryId = 2
                }
            };

            dbContext.Offers.AddRange(offers);
        }
    }
}

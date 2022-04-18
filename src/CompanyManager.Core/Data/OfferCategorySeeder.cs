using CompanyManager.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyManager.Core.Data
{
    public static class OfferCategorySeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OfferCategory>().HasData(
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
                new OfferCategory
                {
                    Id = 3,
                    Name = "Bioinżynieria tkankowa"
                }, new OfferCategory
                {
                    Id = 4,
                    Name = "Mezoterapia mikroigłowa"
                }, new OfferCategory
                {
                    Id = 5,
                    Name = "Mezoterapia igłowa"
                }, new OfferCategory
                {
                    Id = 6,
                    Name = "Redermalizacja"
                }, new OfferCategory
                {
                    Id = 7,
                    Name = "Stymulatory tkankowe"
                }, new OfferCategory
                {
                    Id = 8,
                    Name = "Zabiegi na okolice oczu"
                }, new OfferCategory
                {
                    Id = 9,
                    Name = "Zabiegi na skórę głowy"
                });
        }
    }
}

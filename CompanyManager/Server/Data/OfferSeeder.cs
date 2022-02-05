using CompanyManager.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyManager.Server.Data
{
    public static class OfferSeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Offer>().HasData(
                new Offer
                {
                    Id = 1,
                    Name = "Konsultacje 15-minutowa",
                    Price = 0, 
                    TimeInMinutes = 15,
                    OfferCategoryId = 1                  
                },
                new Offer
                {
                    Id = 2,
                    Name = "Konsultacja Beauty dla skór problematycznych",
                    Price = 250,
                    TimeInMinutes = 120,
                    OfferCategoryId = 1
                },
                new Offer
                {
                    Id = 3,
                    Name = "Konsultacja Beauty dla skór starzejących się",
                    Price = 200,
                    TimeInMinutes = 120,
                    OfferCategoryId = 1
                },
                new Offer
                {
                    Id = 4,
                    Name = "Konsultacja kontrolna",
                    Price = 100,
                    TimeInMinutes = 120,
                    OfferCategoryId = 1
                },
                new Offer
                {
                    Id = 5,
                    Name = "Zabieg oczyszczający",
                    Price = 250,
                    TimeInMinutes = 90,
                    OfferCategoryId = 2
                },
                new Offer
                {
                    Id = 6,
                    Name = "Zabieg z enzymami",
                    Price = 250,
                    TimeInMinutes = 90,
                    OfferCategoryId = 2
                },
                new Offer
                {
                    Id = 7,
                    Name = "Zabieg retinolowy",
                    Price = 400,
                    TimeInMinutes = 90,
                    OfferCategoryId = 2
                },
                new Offer
                {
                    Id = 8,
                    Name = "Zabieg z maską terapeutyczną",
                    Price = 150,
                    TimeInMinutes = 90,
                    OfferCategoryId = 2
                },
                new Offer
                {
                    Id = 9,
                    Name = "Zabieg detoksykujący",
                    Price = 250,
                    TimeInMinutes = 90,
                    OfferCategoryId = 2
                },
                new Offer
                {
                    Id = 10,
                    Name = "Peelingi chemiczne",
                    Price = 300,
                    TimeInMinutes = 90,
                    OfferCategoryId = 2
                },
                new Offer
                {
                    Id = 11,
                    Name = "Sonoforeza",
                    Price = 300,
                    TimeInMinutes = 90,
                    OfferCategoryId = 3
                },
                new Offer
                {
                    Id = 12,
                    Name = "Mezoterapia mikroigłowa",
                    Price = 400,
                    TimeInMinutes = 90,
                    OfferCategoryId = 3
                },              
                new Offer
                {
                    Id = 13,
                    Name = "Mezoterapia mikroigłowa",
                    Price = 300,
                    TimeInMinutes = 90,
                    OfferCategoryId = 4
                },
                new Offer
                {
                    Id = 14,
                    Name = "Mezoterapia igłowa",
                    Price = 400,
                    TimeInMinutes = 90,
                    OfferCategoryId = 5
                },
                new Offer
                {
                    Id = 15,
                    Name = "Hyalual Xela Rederm 1,1% (2ml) / Electri (1,5ml)",
                    Price = 600,
                    TimeInMinutes = 90,
                    OfferCategoryId = 6
                },
                new Offer
                {
                    Id = 16,
                    Name = "Hyalual Xela Rederm 1,8 % (2ml)",
                    Price = 700,
                    TimeInMinutes = 90,
                    OfferCategoryId = 6
                },
                new Offer
                {
                    Id = 17,
                    Name = "Hyalual Xela Rederm 2,2% (2ml)",
                    Price = 800,
                    TimeInMinutes = 90,
                    OfferCategoryId = 6
                },
                new Offer
                {
                    Id = 18,
                    Name = "Nucleofill Medium / Strong",
                    Price = 750,
                    TimeInMinutes = 90,
                    OfferCategoryId = 7
                },
                new Offer
                {
                    Id = 19,
                    Name = "Sunekos 200",
                    Price = 600,
                    TimeInMinutes = 90,
                    OfferCategoryId = 7
                },
                new Offer
                {
                    Id = 20,
                    Name = "Mezoterapia mikroigłowa + ampułka",
                    Price = 200,
                    TimeInMinutes = 90,
                    OfferCategoryId = 8
                },
                new Offer
                {
                    Id = 21,
                    Name = "Mezoterapia mikroigłowa + bioinżynieria tkankowa",
                    Price = 250,
                    TimeInMinutes = 90,
                    OfferCategoryId = 8
                },
                new Offer
                {
                    Id = 22,
                    Name = "Mezoterapia igłowa Dermaheal / RRS HA",
                    Price = 350,
                    TimeInMinutes = 90,
                    OfferCategoryId = 8
                },
                new Offer
                {
                    Id = 23,
                    Name = "Stymulator tkankowy Nucleofil Soft Eyes",
                    Price = 750,
                    TimeInMinutes = 90,
                    OfferCategoryId = 8
                },
                new Offer
                {
                    Id = 24,
                    Name = "Stymulator tkankowy Sunekos 200",
                    Price = 600,
                    TimeInMinutes = 90,
                    OfferCategoryId = 8
                },
                new Offer
                {
                    Id = 25,
                    Name = "Redermalizacja Electri",
                    Price = 600,
                    TimeInMinutes = 90,
                    OfferCategoryId = 8
                },
                new Offer
                {
                    Id = 26,
                    Name = "Mezoterapia mikroigłowa + ampułka",
                    Price = 250,
                    TimeInMinutes = 90,
                    OfferCategoryId = 9
                },
                new Offer
                {
                    Id = 27,
                    Name = "Mezoterapia igłowa RRS XL Hair",
                    Price = 400,
                    TimeInMinutes = 90,
                    OfferCategoryId = 9
                },
                new Offer
                {
                    Id = 28,
                    Name = "Mezoterapia igłowa Dr Cyj Hair Filler",
                    Price = 650,
                    TimeInMinutes = 90,
                    OfferCategoryId = 9
                });
        }
    }
}

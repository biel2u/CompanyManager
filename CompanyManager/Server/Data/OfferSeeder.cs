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
                    Name = "Konsultacje 15-minutowa",
                    Price = 0, 
                    TimeInMinutes = 15,
                    OfferCategoryId = 1                  
                },
                new Offer
                {
                    Name = "Konsultacja Beauty dla skór problematycznych",
                    Price = 250,
                    TimeInMinutes = 120,
                    OfferCategoryId = 1
                },
                new Offer
                {
                    Name = "Konsultacja Beauty dla skór starzejących się",
                    Price = 200,
                    TimeInMinutes = 120,
                    OfferCategoryId = 1
                },
                new Offer
                {
                    Name = "Konsultacja kontrolna",
                    Price = 100,
                    TimeInMinutes = 120,
                    OfferCategoryId = 1
                },
                new Offer
                {
                    Name = "Zabieg oczyszczający",
                    Price = 250,
                    TimeInMinutes = 90,
                    OfferCategoryId = 2
                },
                new Offer
                {
                    Name = "Zabieg z enzymami",
                    Price = 250,
                    TimeInMinutes = 90,
                    OfferCategoryId = 2
                },
                new Offer
                {
                    Name = "Zabieg retinolowy",
                    Price = 400,
                    TimeInMinutes = 90,
                    OfferCategoryId = 2
                },
                new Offer
                {
                    Name = "Zabieg z maską terapeutyczną",
                    Price = 150,
                    TimeInMinutes = 90,
                    OfferCategoryId = 2
                },
                new Offer
                {
                    Name = "Zabieg detoksykujący",
                    Price = 250,
                    TimeInMinutes = 90,
                    OfferCategoryId = 2
                },
                new Offer
                {
                    Name = "Peelingi chemiczne",
                    Price = 300,
                    TimeInMinutes = 90,
                    OfferCategoryId = 2
                },
                new Offer
                {
                    Name = "Sonoforeza",
                    Price = 300,
                    TimeInMinutes = 90,
                    OfferCategoryId = 3
                },
                new Offer
                {
                    Name = "Mezoterapia mikroigłowa",
                    Price = 400,
                    TimeInMinutes = 90,
                    OfferCategoryId = 3
                },              
                new Offer
                {
                    Name = "Mezoterapia mikroigłowa",
                    Price = 300,
                    TimeInMinutes = 90,
                    OfferCategoryId = 4
                },
                new Offer
                {
                    Name = "Mezoterapia igłowa",
                    Price = 400,
                    TimeInMinutes = 90,
                    OfferCategoryId = 5
                },
                new Offer
                {
                    Name = "Hyalual Xela Rederm 1,1% (2ml) / Electri (1,5ml)",
                    Price = 600,
                    TimeInMinutes = 90,
                    OfferCategoryId = 6
                },
                new Offer
                {
                    Name = "Hyalual Xela Rederm 1,8 % (2ml)",
                    Price = 700,
                    TimeInMinutes = 90,
                    OfferCategoryId = 6
                },
                new Offer
                {
                    Name = "Hyalual Xela Rederm 2,2% (2ml)",
                    Price = 800,
                    TimeInMinutes = 90,
                    OfferCategoryId = 6
                },
                new Offer
                {
                    Name = "Nucleofill Medium / Strong",
                    Price = 750,
                    TimeInMinutes = 90,
                    OfferCategoryId = 7
                },
                new Offer
                {
                    Name = "Sunekos 200",
                    Price = 600,
                    TimeInMinutes = 90,
                    OfferCategoryId = 7
                },
                new Offer
                {
                    Name = "Mezoterapia mikroigłowa + ampułka",
                    Price = 200,
                    TimeInMinutes = 90,
                    OfferCategoryId = 8
                },
                new Offer
                {
                    Name = "Mezoterapia mikroigłowa + bioinżynieria tkankowa",
                    Price = 250,
                    TimeInMinutes = 90,
                    OfferCategoryId = 8
                },
                new Offer
                {
                    Name = "Mezoterapia igłowa Dermaheal / RRS HA",
                    Price = 350,
                    TimeInMinutes = 90,
                    OfferCategoryId = 8
                },
                new Offer
                {
                    Name = "Stymulator tkankowy Nucleofil Soft Eyes",
                    Price = 750,
                    TimeInMinutes = 90,
                    OfferCategoryId = 8
                },
                new Offer
                {
                    Name = "Stymulator tkankowy Sunekos 200",
                    Price = 600,
                    TimeInMinutes = 90,
                    OfferCategoryId = 8
                },
                new Offer
                {
                    Name = "Redermalizacja Electri",
                    Price = 600,
                    TimeInMinutes = 90,
                    OfferCategoryId = 8
                },
                new Offer
                {
                    Name = "Mezoterapia mikroigłowa + ampułka",
                    Price = 250,
                    TimeInMinutes = 90,
                    OfferCategoryId = 9
                },
                new Offer
                {
                    Name = "Mezoterapia igłowa RRS XL Hair",
                    Price = 400,
                    TimeInMinutes = 90,
                    OfferCategoryId = 9
                },
                new Offer
                {
                    Name = "Mezoterapia igłowa Dr Cyj Hair Filler",
                    Price = 650,
                    TimeInMinutes = 90,
                    OfferCategoryId = 9
                });
        }
    }
}

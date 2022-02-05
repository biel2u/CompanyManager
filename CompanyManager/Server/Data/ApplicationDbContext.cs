using CompanyManager.Server.Models;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CompanyManager.Server.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Appointment> Appointments => Set<Appointment>();
        public DbSet<OfferCategory> OfferCategories => Set<OfferCategory>();
        public DbSet<Offer> Offers => Set<Offer>();
        public DbSet<Consent> Consents => Set<Consent>();
        public DbSet<Photo> Photos => Set<Photo>();

        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Offer>()
                .HasMany(o => o.Appointments)
                .WithOne(a => a.Offer)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Appointment>()
                .HasMany(a => a.Photos)
                .WithOne(p => p.Appointment)
                .OnDelete(DeleteBehavior.ClientSetNull);

            OfferCategorySeeder.Seed(modelBuilder);
            OfferSeeder.Seed(modelBuilder);
        }
    }
}
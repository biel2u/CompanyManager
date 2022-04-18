using CompanyManager.Core.Models;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CompanyManager.Core.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public virtual DbSet<Customer> Customers => Set<Customer>();
        public virtual DbSet<Appointment> Appointments => Set<Appointment>();
        public virtual DbSet<OfferCategory> OfferCategories => Set<OfferCategory>();
        public virtual DbSet<Offer> Offers => Set<Offer>();
        public virtual DbSet<Consent> Consents => Set<Consent>();
        public virtual DbSet<Photo> Photos => Set<Photo>();
        public virtual DbSet<AppointmentOffer> AppointmentOffers => Set<AppointmentOffer>();

        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Appointment>()
                .HasMany(a => a.Photos)
                .WithOne(p => p.Appointment)
                .OnDelete(DeleteBehavior.ClientSetNull);

            OfferCategorySeeder.Seed(modelBuilder);
            OfferSeeder.Seed(modelBuilder);
        }
    }
}
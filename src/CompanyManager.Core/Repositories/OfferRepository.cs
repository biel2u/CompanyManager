using CompanyManager.Core.Data;
using CompanyManager.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyManager.Core.Repositories
{
    public interface IOfferRepository
    {
        IQueryable<Offer> GetAllOffers();
    }

    public class OfferRepository : RepositoryBase<Offer>, IOfferRepository
    {        
        public OfferRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<Offer> GetAllOffers()
        {
            var offers = DbContext.Offers.Include(o => o.OfferCategory);

            return offers;
        }
    }
}

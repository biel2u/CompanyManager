using CompanyManager.Server.Data;
using CompanyManager.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyManager.Server.Repositories
{
    public interface IOfferRepository
    {
        IQueryable<Offer> GetAllOffers();
    }

    public class OfferRepository : IOfferRepository
    {
        private readonly ApplicationDbContext _dbContext;
        
        public OfferRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Offer> GetAllOffers()
        {
            var offers = _dbContext.Offers.Include(o => o.OfferCategory);
            return offers;
        }
    }
}

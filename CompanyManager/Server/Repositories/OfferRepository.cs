using CompanyManager.Server.Data;
using CompanyManager.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyManager.Server.Repositories
{
    public interface IOfferRepository
    {
        Task<List<Offer>> GetAllOffers();
    }

    public class OfferRepository : IOfferRepository
    {
        private readonly ApplicationDbContext _dbContext;
        
        public OfferRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Offer>> GetAllOffers()
        {
            var offers = await _dbContext.Offers.Include(o => o.OfferCategory).ToListAsync();
            return offers;
        }
    }
}

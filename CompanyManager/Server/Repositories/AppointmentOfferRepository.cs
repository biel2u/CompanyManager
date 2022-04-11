using CompanyManager.Server.Data;
using CompanyManager.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyManager.Server.Repositories
{
    public interface IAppointmentOfferRepository
    {
        Task<bool> CreateAppointmentWithOffers(IEnumerable<AppointmentOffer> appointmentOffers);
        Task<bool> EditAppointmentWithOffers(
            IEnumerable<AppointmentOffer> appointmentOffersToCreate,
            IEnumerable<AppointmentOffer> appointmentOffersToUpdate,
            IEnumerable<AppointmentOffer> appointmentOffersToDelete);
        Task<List<AppointmentOffer>> GetAppointmentOffers(int appointmentId);
    }

    public class AppointmentOfferRepository : IAppointmentOfferRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AppointmentOfferRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<AppointmentOffer>> GetAppointmentOffers(int appointmentId)
        {
            var appointmentOffers = await _dbContext.AppointmentOffers.Where(a => a.AppointmentId == appointmentId).ToListAsync();
            
            return appointmentOffers;
        }

        public async Task<bool> CreateAppointmentWithOffers(IEnumerable<AppointmentOffer> appointmentOffers)
        {
            _dbContext.AppointmentOffers.AddRange(appointmentOffers);
            var result = await _dbContext.SaveChangesAsync() > 0;

            return result;            
        }

        public async Task<bool> EditAppointmentWithOffers(
            IEnumerable<AppointmentOffer> appointmentOffersToCreate, 
            IEnumerable<AppointmentOffer> appointmentOffersToUpdate, 
            IEnumerable<AppointmentOffer> appointmentOffersToDelete)
        {

            _dbContext.AppointmentOffers.AddRange(appointmentOffersToCreate);
            _dbContext.AppointmentOffers.UpdateRange(appointmentOffersToUpdate);
            _dbContext.AppointmentOffers.RemoveRange(appointmentOffersToDelete);
            var result = await _dbContext.SaveChangesAsync() > 0;

            return result;           
        }
    }
}

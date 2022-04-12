using CompanyManager.Server.Data;
using CompanyManager.Server.Models;

namespace CompanyManager.Server.Repositories
{
    public interface IAppointmentOfferRepository
    {
        Task<bool> CreateAppointmentWithOffers(IEnumerable<AppointmentOffer> appointmentOffers);
        Task<bool> EditAppointmentWithOffers(
            IEnumerable<AppointmentOffer> appointmentOffersToCreate,
            IEnumerable<AppointmentOffer> appointmentOffersToUpdate,
            IEnumerable<AppointmentOffer> appointmentOffersToDelete);
    }

    public class AppointmentOfferRepository : IAppointmentOfferRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AppointmentOfferRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
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

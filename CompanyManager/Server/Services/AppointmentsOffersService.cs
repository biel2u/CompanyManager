using CompanyManager.Server.Models;
using CompanyManager.Server.Repositories;
using CompanyManager.Shared;
using Microsoft.EntityFrameworkCore;

namespace CompanyManager.Server.Services
{
    public interface IAppointmentsOffersService
    {
        List<DisplayOfferModel> GetSelectedAppointmentsOffers(IEnumerable<AppointmentOffer> appointmentsOffers);
        Task<bool> CreateAppointmentWithOffers(EditAppointmentModel appointment);
        Task<bool> UpdateAppointmentWithOffers(List<DisplayOfferModel> currentOffers, Appointment appointment);
    }

    public class AppointmentsOffersService : IAppointmentsOffersService
    {
        private readonly IOfferRepository _offerRepository;
        private readonly IAppointmentOfferRepository _appointmentOfferRepository;
        private readonly ICustomerService _customerService;

        public AppointmentsOffersService(IOfferRepository offerRepository, IAppointmentOfferRepository appointmentOfferRepository, ICustomerService customerService)
        {
            _offerRepository = offerRepository;
            _appointmentOfferRepository = appointmentOfferRepository;
            _customerService = customerService;
        }

        public List<DisplayOfferModel> GetSelectedAppointmentsOffers(IEnumerable<AppointmentOffer> appointmentsOffers)
        {
            var selectedOffers = new List<DisplayOfferModel>();

            foreach (var appointmentOffer in appointmentsOffers)
            {
                selectedOffers.Add(new DisplayOfferModel
                {
                    Id = appointmentOffer.OfferId,
                    IsSelected = true,
                    Name = appointmentOffer.Offer.Name,
                    Price = appointmentOffer.CustomOfferPrice,
                    TimeInMinutes = appointmentOffer.CustomOfferTime,
                });
            }

            return selectedOffers;
        }

        public async Task<bool> CreateAppointmentWithOffers(EditAppointmentModel appointment)
        {
            var customer = await _customerService.GetCustomerByExtractedPhoneNumber(appointment.CustomerNameAndPhone);
            if (customer == null) return false;

            var newAppointment = new Appointment
            {
                StartDate = appointment.StartDate + appointment.Time,
                EndDate = appointment.EndDate,
                Note = appointment.Note,
                Status = appointment.Confirmed ? AppointmentStatus.Confirmed : AppointmentStatus.Pending,
                CustomerId = customer.Id,
            };

            var offers = await _offerRepository.GetAllOffers().ToListAsync();
            var appointmentsOffers = BuildAppointmentOffers(appointment.Offers, newAppointment, offers);
            var result = await _appointmentOfferRepository.CreateAppointmentWithOffers(appointmentsOffers);

            return result;
        }

        private List<AppointmentOffer> BuildAppointmentOffers(List<DisplayOfferModel> selectedOffers, Appointment appointment, List<Offer> offers)
        {
            var appointmentOffers = new List<AppointmentOffer>();
            foreach (var offer in selectedOffers)
            {
                appointmentOffers.Add(new AppointmentOffer
                {
                    CustomOfferPrice = offer.Price,
                    CustomOfferTime = offer.TimeInMinutes,
                    Offer = offers.Single(o => o.Id == offer.Id),
                    Appointment = appointment
                });
            }

            return appointmentOffers;
        }

        public async Task<bool> UpdateAppointmentWithOffers(List<DisplayOfferModel> currentOffers, Appointment appointment)
        {           
            var newOffers = currentOffers.Where(s => appointment.AppointmentOffers.Any(e => e.OfferId == s.Id) == false).ToList();
            var appointmetsOffersToCreate = IncludeNewOffersToAppointment(newOffers, appointment);
            var appointmentsOffersToUpdate = GetAppointmentsOffersToUpdate(currentOffers, appointment);
            var appointmentsOffersToDelete = appointment.AppointmentOffers.Where(s => currentOffers.Any(e => e.Id == s.OfferId) == false).ToList();

            var appointmentWithOffers = await _appointmentOfferRepository.UpdateAppointmentWithOffers(appointmetsOffersToCreate, appointmentsOffersToUpdate, appointmentsOffersToDelete);

            return appointmentWithOffers;
        }

        private List<AppointmentOffer> GetAppointmentsOffersToUpdate(List<DisplayOfferModel> currentOffers, Appointment appointment)
        {
            var appointmentsOffersToEdit = new List<AppointmentOffer>();

            foreach (var offer in currentOffers)
            {
                var editedOffer = appointment.AppointmentOffers.SingleOrDefault(e => e.OfferId == offer.Id);
                if (editedOffer != null)
                {
                    editedOffer.CustomOfferPrice = offer.Price;
                    editedOffer.CustomOfferTime = offer.TimeInMinutes;

                    appointmentsOffersToEdit.Add(editedOffer);
                }
            }

            return appointmentsOffersToEdit;
        }

        private List<AppointmentOffer> IncludeNewOffersToAppointment(List<DisplayOfferModel> newOffers, Appointment appointment)
        {
            var appointmentOffers = new List<AppointmentOffer>();
            foreach (var offer in newOffers)
            {
                appointmentOffers.Add(new AppointmentOffer
                {
                    CustomOfferPrice = offer.Price,
                    CustomOfferTime = offer.TimeInMinutes,
                    OfferId = offer.Id,
                    AppointmentId = appointment.Id,
                });
            }

            return appointmentOffers;
        }
    }
}

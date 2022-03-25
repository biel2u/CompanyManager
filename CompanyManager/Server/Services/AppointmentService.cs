using CompanyManager.Server.Repositories;
using CompanyManager.Server.Helpers;
using CompanyManager.Shared;
using CompanyManager.Server.Models;

namespace CompanyManager.Server.Services
{
    public interface IAppointmentService
    {
        Task<AppointmentEditForm> GetAppointment(int? appointmentId);
        Task<bool> CheckForConflicts(AppointmentEditForm appointment);
        Task<bool> CreateAppointment(AppointmentEditForm appointment);
    }

    public class AppointmentService : IAppointmentService
    {
        private IDateTimeProvider _dateTimeProvider;
        private IAppointmentRepository _appointmentRepository;
        private IOfferRepository _offerRepository;
        private ICustomerService _customerService;

        public AppointmentService(
            IDateTimeProvider dateTimeProvider, 
            IAppointmentRepository appointmentRepository, 
            IOfferRepository offerRepository, 
            ICustomerService customeService)
        {
            _dateTimeProvider = dateTimeProvider;
            _appointmentRepository = appointmentRepository;
            _offerRepository = offerRepository;
            _customerService = customeService;
        }

        public async Task<AppointmentEditForm> GetAppointment(int? appointmentId)
        {
            var appointment = new AppointmentEditForm();
            if (appointmentId.HasValue)
            {
                //get existing
            }
            else
            {
                var dateTimeNow = _dateTimeProvider.GetCurrentDateTime();
                appointment.Offers = new List<OfferViewModel>();
                appointment.StartDate = dateTimeNow.Date;
                appointment.Time = dateTimeNow.TimeOfDay;
            }

            return appointment;
        }

        public async Task<bool> CheckForConflicts(AppointmentEditForm appointment)
        {
            var appointmentsInRange = await _appointmentRepository.GetAppointmentsInRange(appointment.StartDate, appointment.EndDate);
            return appointmentsInRange.Any();
        }

        public async Task<bool> CreateAppointment(AppointmentEditForm appointment)
        {
            var customer = await _customerService.GetCustomerByExtractedPhoneNumber(appointment.CustomerNameAndPhone);

            if (customer == null) return false;

            var offers = await _offerRepository.GetAllOffers();
            var selectedOffers = offers.Where(o => appointment.Offers.Any(ao => ao.Id == o.Id)).ToList();

            var newAppointment = new Appointment
            {
                StartDate = appointment.StartDate,
                EndDate = appointment.EndDate,
                Note = appointment.Note,
                Offers = selectedOffers,
                Status = appointment.Confirmed ? AppointmentStatus.Confirmed : AppointmentStatus.Pending,
                CustomerId = customer.Id,
            };

            var result = await _appointmentRepository.AddAppointment(newAppointment);
            return result;
        }
    }
}

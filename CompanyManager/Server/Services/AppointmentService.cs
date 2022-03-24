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
            if (appointment.StartDate == null || appointment.Time == null) return true;

            var startDateTime = appointment.StartDate.Value + appointment.Time.Value;
            var appointmentEndDate = GetAppointmentEndDateTime(startDateTime, appointment.TotalMinutes);
            var appointmentsInRange = await _appointmentRepository.GetAppointmentsInRange(appointment.StartDate.Value, appointmentEndDate);

            return appointmentsInRange.Any();
        }

        public async Task<bool> CreateAppointment(AppointmentEditForm appointment)
        {
            var customer = await _customerService.GetCustomerByExtractedPhoneNumber(appointment.CustomerNameAndPhone);

            if (appointment.StartDate == null || appointment.Time == null || customer == null) return false;

            var startDateTime = appointment.StartDate.Value + appointment.Time.Value;
            var endDateTime = GetAppointmentEndDateTime(startDateTime, appointment.TotalMinutes);
            var offers = await _offerRepository.GetAllOffers();
            var selectedOffers = offers.Where(o => appointment.Offers.Any(ao => ao.Id == o.Id)).ToList();

            var newAppointment = new Appointment
            {
                StartDate = startDateTime,
                EndDate = endDateTime,
                Note = appointment.Note,
                Offers = selectedOffers,
                Status = appointment.Confirmed ? AppointmentStatus.Confirmed : AppointmentStatus.Pending,
                CustomerId = customer.Id,
            };

            var result = await _appointmentRepository.AddAppointment(newAppointment);
            return result;
        }

        private DateTime GetAppointmentEndDateTime(DateTime startDate, int totalMinutes)
        {
            var endDate = startDate.AddMinutes(totalMinutes);
            return endDate;
        }
    }
}

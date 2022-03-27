using CompanyManager.Server.Repositories;
using CompanyManager.Server.Helpers;
using CompanyManager.Shared;
using CompanyManager.Server.Models;

namespace CompanyManager.Server.Services
{
    public interface IAppointmentService
    {
        Task<EditAppointmentModel> GetAppointment(int? appointmentId);
        Task<bool> CheckForConflicts(EditAppointmentModel appointment);
        Task<bool> CreateAppointment(EditAppointmentModel appointment);
        Task<IEnumerable<DisplayAppointmentModel>> GetAppointmentsInRange(AppointmentsRange appointmentsRange);
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

        public async Task<EditAppointmentModel> GetAppointment(int? appointmentId)
        {
            var appointment = new EditAppointmentModel();
            if (appointmentId.HasValue)
            {
                //get existing
            }
            else
            {
                var dateTimeNow = _dateTimeProvider.GetCurrentDateTime();
                appointment.Offers = new List<DisplayOfferModel>();
                appointment.StartDate = dateTimeNow.Date;
                appointment.Time = dateTimeNow.TimeOfDay;
            }

            return appointment;
        }

        public async Task<IEnumerable<DisplayAppointmentModel>> GetAppointmentsInRange(AppointmentsRange appointmentsRange)
        {
            var appointments = await _appointmentRepository.GetAppointmentsInRange(appointmentsRange.StartDate, appointmentsRange.EndDate);
            var appointmentsToDisplay = new List<DisplayAppointmentModel>();
            
            foreach (var appointment in appointments)
            {
                appointmentsToDisplay.Add(new DisplayAppointmentModel
                {
                    Id = appointment.Id,
                    ClientName = appointment.Customer.Name,
                    OfferName = appointment.Offers.First().Name,
                    OffersCount = appointment.Offers.Count(),
                    StartDate = appointment.StartDate,
                    EndDate = appointment.EndDate,
                    StartRow = CalculateAppointmentDisplayRow(appointment.StartDate),
                    EndRow = CalculateAppointmentDisplayRow(appointment.EndDate)
                });
            }

            return appointmentsToDisplay;
        }

        private int CalculateAppointmentDisplayRow(DateTime dateTime)
        {
            var hourRow = CalendarConstants.GridHourRows * dateTime.Hour;
            var minuteRow = dateTime.Minute / CalendarConstants.MinutesSampling;
            var row = hourRow + minuteRow;

            return row;
        }

        public async Task<bool> CheckForConflicts(EditAppointmentModel appointment)
        {
            var appointmentsInRange = await _appointmentRepository.GetAppointmentsInRange(appointment.StartDate, appointment.EndDate);
            return appointmentsInRange.Any();
        }

        public async Task<bool> CreateAppointment(EditAppointmentModel appointment)
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

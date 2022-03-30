using CompanyManager.Server.Repositories;
using CompanyManager.Server.Helpers;
using CompanyManager.Shared;
using CompanyManager.Server.Models;

namespace CompanyManager.Server.Services
{
    public interface IAppointmentService
    {
        Task<EditAppointmentModel> GetAppointment(int? appointmentId);
        Task<Dictionary<string, string>> ValidateAppointment(EditAppointmentModel appointment);
        Task<bool> CreateAppointment(EditAppointmentModel appointment);
        Task<List<DisplayAppointmentModel>> GetAppointmentsInRange(AppointmentsRange appointmentsRange);
        Task<bool> DeleteAppointment(int id);
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

        public async Task<List<DisplayAppointmentModel>> GetAppointmentsInRange(AppointmentsRange appointmentsRange)
        {
            var appointments = await _appointmentRepository.GetAppointmentsInRangeDailyAccuracy(appointmentsRange.StartDate.Date, appointmentsRange.EndDate.Date);
            var appointmentsToDisplay = new List<DisplayAppointmentModel>();
            
            foreach (var appointment in appointments)
            {
                appointmentsToDisplay.Add(new DisplayAppointmentModel
                {
                    Id = appointment.Id,
                    ClientName = $"{appointment.Customer.Name} {appointment.Customer.Surname}",
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
            var hourRow = (CalendarConstants.GridHourRows * dateTime.Hour) + CalendarConstants.InitialTimeRow;
            var minuteRow = dateTime.Minute / CalendarConstants.MinutesSampling;
            var row = hourRow + minuteRow;

            return row;
        }

        public async Task<Dictionary<string,string>> ValidateAppointment(EditAppointmentModel appointment)
        {
            var appointmentsInRange = await _appointmentRepository.GetAppointmentsInRangeHourlyAccuracy(appointment.StartDate, appointment.EndDate);
            var errors = new Dictionary<string, string>();

            if (appointmentsInRange.Any())
            {
                errors.Add("DateConflict", "Czas trwania wizyty pokrywa się z czasem innej wizyty.");
            }

            if(appointment.StartDate.Day != appointment.EndDate.Day)
            {
                errors.Add("TimeExceeded", "Wizyta nie może być rozłożona na dwa dni.");
            }

            return errors;
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

        public async Task<bool> DeleteAppointment(int id)
        {
            var result = await _appointmentRepository.DeleteAppointment(id);
            return result;
        }
    }
}

using CompanyManager.Server.Repositories;
using CompanyManager.Server.Helpers;
using CompanyManager.Shared;
using CompanyManager.Server.Models;

namespace CompanyManager.Server.Services
{
    public interface IAppointmentService
    {
        Task<EditAppointmentModel?> GetAppointment(int? appointmentId);
        Task<List<DisplayAppointmentModel>> GetAppointmentsInRange(AppointmentsRange appointmentsRange);
        Task<bool> DeleteAppointment(int id);
        Task<bool> UpdateAppointment(EditAppointmentModel appointment);
    }

    public class AppointmentService : IAppointmentService
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly ICustomerService _customerService;
        private readonly IAppointmentsOffersService _appointmentsOffersService;

        public AppointmentService(
            IDateTimeProvider dateTimeProvider,
            IAppointmentRepository appointmentRepository,
            ICustomerService customeService,
            IAppointmentsOffersService appointmentsOffersService)
        {
            _dateTimeProvider = dateTimeProvider;
            _appointmentRepository = appointmentRepository;
            _customerService = customeService;
            _appointmentsOffersService = appointmentsOffersService;
        }

        public async Task<EditAppointmentModel?> GetAppointment(int? appointmentId)
        {
            if (appointmentId.HasValue)
            {
                var appointment = await GetAppointmentToEdit(appointmentId.Value);

                return appointment;
            }
            else
            {
                var appointment = GetAppointmentToCreate();

                return appointment;
            }           
        }

        private async Task<EditAppointmentModel?> GetAppointmentToEdit(int appointmentId)
        {
            var dbAppointment = await _appointmentRepository.GetAppointment(appointmentId);
            if (dbAppointment == null) return null;

            var appointment = new EditAppointmentModel();

            appointment.Id = dbAppointment.Id;
            appointment.StartDate = dbAppointment.StartDate;
            appointment.EndDate = dbAppointment.EndDate;
            appointment.Time = dbAppointment.StartDate.TimeOfDay;
            appointment.CustomerNameAndPhone = _customerService.CreateCustomerNameWithPhoneNumber(dbAppointment.Customer);
            appointment.Note = dbAppointment.Note;
            appointment.Confirmed = dbAppointment.Status == AppointmentStatus.Confirmed;
            appointment.Offers = _appointmentsOffersService.GetSelectedAppointmentsOffers(dbAppointment.AppointmentOffers);

            return appointment;
        }       

        private EditAppointmentModel GetAppointmentToCreate()
        {
            var appointment = new EditAppointmentModel();

            var dateTimeNow = _dateTimeProvider.GetCurrentDateTime();
            appointment.Offers = new List<DisplayOfferModel>();
            appointment.StartDate = dateTimeNow.Date;
            appointment.Time = dateTimeNow.TimeOfDay;

            return appointment;
        }

        public async Task<List<DisplayAppointmentModel>> GetAppointmentsInRange(AppointmentsRange appointmentsRange)
        {
            var appointments = await _appointmentRepository.GetAppointmentsInRangeDailyAccuracy(appointmentsRange.StartDate.Date, appointmentsRange.EndDate.Date);
            var appointmentsToDisplay = new List<DisplayAppointmentModel>();

            foreach (var appointment in appointments)
            {
                 var offerName = appointment.AppointmentOffers.First().Offer.Name;

                appointmentsToDisplay.Add(new DisplayAppointmentModel
                {
                    Id = appointment.Id,
                    ClientName = $"{appointment.Customer.Name} {appointment.Customer.Surname}",
                    OfferName = offerName,
                    OffersCount = appointment.AppointmentOffers.Count,
                    StartDate = appointment.StartDate,
                    EndDate = appointment.EndDate
                });
            }

            return appointmentsToDisplay;
        }                       

        public async Task<bool> UpdateAppointment(EditAppointmentModel appointment)
        {
            var customer = await _customerService.GetCustomerByExtractedPhoneNumber(appointment.CustomerNameAndPhone);
            if (customer == null || appointment?.Id == null) return false;

            var dbAppointment = await _appointmentRepository.GetAppointment(appointment.Id.Value);
            if(dbAppointment == null) return false;

            dbAppointment.Status = appointment.Confirmed ? AppointmentStatus.Confirmed : AppointmentStatus.Pending;
            dbAppointment.StartDate = appointment.StartDate + appointment.Time;
            dbAppointment.EndDate = appointment.EndDate;
            dbAppointment.Note = appointment.Note;
            dbAppointment.CustomerId = customer.Id;

            var result = await _appointmentsOffersService.UpdateAppointmentWithOffers(appointment.Offers, dbAppointment);

            return result;
        }     

        public async Task<bool> DeleteAppointment(int id)
        {
            var result = await _appointmentRepository.DeleteAppointment(id);

            return result;
        }
    }
}

using CompanyManager.Server.Repositories;
using CompanyManager.Server.Helpers;
using CompanyManager.Shared;
using CompanyManager.Server.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

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
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IOfferRepository _offerRepository;
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public AppointmentService(
            IDateTimeProvider dateTimeProvider, 
            IAppointmentRepository appointmentRepository, 
            IOfferRepository offerRepository, 
            ICustomerService customeService,
            IMapper mapper)
        {
            _dateTimeProvider = dateTimeProvider;
            _appointmentRepository = appointmentRepository;
            _offerRepository = offerRepository;
            _customerService = customeService;
            _mapper = mapper;
        }

        public async Task<EditAppointmentModel> GetAppointment(int? appointmentId)
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

        private async Task<EditAppointmentModel> GetAppointmentToEdit(int appointmentId)
        {
            var dbAppointment = await _appointmentRepository.GetAppointment(appointmentId);
            var appointment = new EditAppointmentModel();

            appointment.Id = dbAppointment.Id;
            appointment.StartDate = dbAppointment.StartDate;
            appointment.Time = dbAppointment.StartDate.TimeOfDay;
            appointment.CustomerNameAndPhone = _customerService.CreateCustomerNameWithPhoneNumber(dbAppointment.Customer);
            appointment.Note = dbAppointment.Note;
            appointment.Confirmed = dbAppointment.Status == AppointmentStatus.Confirmed;
            appointment.Offers = _mapper.Map<List<DisplayOfferModel>>(dbAppointment.Offers);

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
                appointmentsToDisplay.Add(new DisplayAppointmentModel
                {
                    Id = appointment.Id,
                    ClientName = $"{appointment.Customer.Name} {appointment.Customer.Surname}",
                    OfferName = appointment.Offers.First().Name,
                    OffersCount = appointment.Offers.Count(),
                    StartDate = appointment.StartDate,
                    EndDate = appointment.EndDate
                });
            }

            return appointmentsToDisplay;
        }        

        public async Task<Dictionary<string,string>> ValidateAppointment(EditAppointmentModel appointment)
        {
            var appointmentsInRange = await _appointmentRepository.GetAppointmentsInRangeHourlyAccuracy(appointment.StartDate, appointment.EndDate);
            if(appointment.Id != null)
            {
                appointmentsInRange.RemoveAll(a => a.Id == appointment.Id);
            }

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

            var offers = _offerRepository.GetAllOffers();
            var selectedOffers = await offers.Where(o => appointment.Offers.Any(ao => ao.Id == o.Id)).ToListAsync();

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

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
        Task<bool> UpdateAppointment(EditAppointmentModel appointment);
    }

    public class AppointmentService : IAppointmentService
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IOfferRepository _offerRepository;
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;
        private readonly IAppointmentOfferRepository _appointmentOfferRepository;

        public AppointmentService(
            IDateTimeProvider dateTimeProvider,
            IAppointmentRepository appointmentRepository,
            IOfferRepository offerRepository,
            ICustomerService customeService,
            IMapper mapper, 
            IAppointmentOfferRepository appointmentOfferRepository)
        {
            _dateTimeProvider = dateTimeProvider;
            _appointmentRepository = appointmentRepository;
            _offerRepository = offerRepository;
            _customerService = customeService;
            _mapper = mapper;
            _appointmentOfferRepository = appointmentOfferRepository;
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
            appointment.EndDate = dbAppointment.EndDate;
            appointment.Time = dbAppointment.StartDate.TimeOfDay;
            appointment.CustomerNameAndPhone = _customerService.CreateCustomerNameWithPhoneNumber(dbAppointment.Customer);
            appointment.Note = dbAppointment.Note;
            appointment.Confirmed = dbAppointment.Status == AppointmentStatus.Confirmed;
            appointment.Offers = GetAppointmentOffers(dbAppointment.AppointmentOffers);

            return appointment;
        }

        private List<DisplayOfferModel> GetAppointmentOffers(IEnumerable<AppointmentOffer> appointmentOffers)
        {
            var selectedOffers = new List<DisplayOfferModel>();

            foreach(var appointmentOffer in appointmentOffers)
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

            var newAppointment = new Appointment
            {
                StartDate = appointment.StartDate + appointment.Time,
                EndDate = appointment.EndDate,
                Note = appointment.Note,
                Status = appointment.Confirmed ? AppointmentStatus.Confirmed : AppointmentStatus.Pending,
                CustomerId = customer.Id,
            };

            var offers = await _offerRepository.GetAllOffers().ToListAsync();
                 
            var result = await CreateAppointmentWithOffers(appointment.Offers, newAppointment, offers);
            return result;           
        }

        public async Task<bool> UpdateAppointment(EditAppointmentModel appointment)
        {
            var customer = await _customerService.GetCustomerByExtractedPhoneNumber(appointment.CustomerNameAndPhone);

            if (customer == null || appointment?.Id == null) return false;

            var dbAppointment = await _appointmentRepository.GetAppointment(appointment.Id.Value);
            dbAppointment.Status = appointment.Confirmed ? AppointmentStatus.Confirmed : AppointmentStatus.Pending;
            dbAppointment.StartDate = appointment.StartDate + appointment.Time;
            dbAppointment.EndDate = appointment.EndDate;
            dbAppointment.Note = appointment.Note;
            dbAppointment.CustomerId = customer.Id; //mapper

            var result = await UpdateAppointmentsOffers(appointment.Offers, dbAppointment);
            return result;
        }

        private async Task<bool> UpdateAppointmentsOffers(List<DisplayOfferModel> currentOffers, Appointment appointment)
        {
            var editedAppointmentOffers = new List<AppointmentOffer>();

            foreach (var offer in currentOffers)
            {
                var editedOffer = appointment.AppointmentOffers.SingleOrDefault(e => e.OfferId == offer.Id);
                if (editedOffer != null)
                {
                    editedOffer.CustomOfferPrice = offer.Price;
                    editedOffer.CustomOfferTime = offer.TimeInMinutes;

                    editedAppointmentOffers.Add(editedOffer);
                }
            }

            var deletedAppointmentOffers = appointment.AppointmentOffers.Where(s => currentOffers.Any(e => e.Id == s.OfferId) == false).ToList();
            var newOffers = currentOffers.Where(s => appointment.AppointmentOffers.Any(e => e.OfferId == s.Id) == false).ToList();
            var newAppointmentOffers = IncludeNewOffersToAppointment(newOffers, appointment);

            var appointmentWithOffers = await _appointmentOfferRepository.EditAppointmentWithOffers(newAppointmentOffers, editedAppointmentOffers, deletedAppointmentOffers);
            return appointmentWithOffers;
        }

        private async Task<bool> CreateAppointmentWithOffers(List<DisplayOfferModel> selectedOffers, Appointment appointment, List<Offer> offers)
        {
            var appointmentOffers = BuildAppointmentOffers(selectedOffers, appointment, offers);
            var createdAppointment = await _appointmentOfferRepository.CreateAppointmentWithOffers(appointmentOffers);
            return createdAppointment;
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

        public async Task<bool> DeleteAppointment(int id)
        {
            var result = await _appointmentRepository.DeleteAppointment(id);
            return result;
        }
    }
}

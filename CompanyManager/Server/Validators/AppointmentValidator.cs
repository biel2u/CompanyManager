using CompanyManager.Server.Repositories;
using CompanyManager.Shared;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CompanyManager.Server.Validators
{
    public interface IAppointmentValidator
    {
        Task<ModelStateDictionary> SetModelStateErrors(EditAppointmentModel appointment, ModelStateDictionary modelState);
    }

    public class AppointmentValidator : IAppointmentValidator
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentValidator(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<ModelStateDictionary> SetModelStateErrors(EditAppointmentModel appointment, ModelStateDictionary modelState)
        {
            if (appointment == null) return modelState;

            var errors = await ValidateAppointment(appointment);
            foreach (var error in errors)
            {
                modelState.AddModelError(error.Key, error.Value);
            }

            return modelState;
        }

        public async Task<Dictionary<string, string>> ValidateAppointment(EditAppointmentModel appointment)
        {
            var startDateTime = appointment.StartDate + appointment.Time;
            var appointmentsInRange = await _appointmentRepository.GetAppointmentsInRangeHourlyAccuracy(startDateTime, appointment.EndDate);
            if (appointment.Id != null)
            {
                appointmentsInRange.RemoveAll(a => a.Id == appointment.Id);
            }

            var errors = new Dictionary<string, string>();

            if (appointmentsInRange.Any())
            {
                errors.Add("DateConflict", "Czas trwania wizyty pokrywa się z czasem innej wizyty.");
            }

            if (appointment.StartDate.Day != appointment.EndDate.Day)
            {
                errors.Add("TimeExceeded", "Wizyta nie może być rozłożona na dwa dni.");
            }

            return errors;
        }
    }
}

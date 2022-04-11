using CompanyManager.Shared;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace CompanyManager.Client.DataServices
{
    public interface IAppointmentDataService
    {
        Task<EditAppointmentModel> GetAppointment(int? id);
        Task<HttpResponseMessage> HandleSubmit(EditAppointmentModel appointment);
        Task<List<DisplayAppointmentModel>> GetAppointmentsInRange(AppointmentsRange appointmentsRange);
        Task<HttpResponseMessage> DeleteAppointment(int id);
    }

    public class AppointmentDataService : IAppointmentDataService
    {
        private readonly HttpClient _http;
        private readonly string ControllerName = "Appointment";

        public AppointmentDataService(HttpClient http)
        {
            _http = http;
        }

        public async Task<EditAppointmentModel> GetAppointment(int? id)
        {            
            var response = await _http.GetFromJsonAsync<EditAppointmentModel>($"{ControllerName}/GetAppointment?id={id}");
            return response ?? new EditAppointmentModel();          
        }

        public async Task<HttpResponseMessage> HandleSubmit(EditAppointmentModel appointment)
        {
            if(appointment.Id == null)
            {
                var resposne = await _http.PostAsJsonAsync($"{ControllerName}/CreateAppointment", appointment);
                return resposne;
            }
            else
            {
                var serializedAppointment = JsonSerializer.Serialize(appointment);
                var content = new StringContent(serializedAppointment, Encoding.UTF8, "application/json-patch+json");
                var resposne = await _http.PatchAsync($"{ControllerName}/UpdateAppointment", content);
                return resposne;
            }
        }

        public async Task<List<DisplayAppointmentModel>> GetAppointmentsInRange(AppointmentsRange appointmentsRange)
        {
            var response = await _http.PostAsJsonAsync($"{ControllerName}/GetAppointmentsInRange", appointmentsRange);
            var result = await response.Content.ReadFromJsonAsync<List<DisplayAppointmentModel>>();
            return result ?? new List<DisplayAppointmentModel>();
        }

        public async Task<HttpResponseMessage> DeleteAppointment(int id)
        {
            var resposne = await _http.DeleteAsync($"{ControllerName}/DeleteAppointment?id={id}");
            return resposne;
        }
    }
}

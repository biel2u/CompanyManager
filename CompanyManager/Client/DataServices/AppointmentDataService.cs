using CompanyManager.Shared;
using System.Net.Http.Json;

namespace CompanyManager.Client.DataServices
{
    public interface IAppointmentDataService
    {
        Task<EditAppointmentModel> GetAppointment();
        Task<HttpResponseMessage> CreateAppointment(EditAppointmentModel appointment);
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

        public async Task<EditAppointmentModel> GetAppointment()
        {            
            var response = await _http.GetFromJsonAsync<EditAppointmentModel>($"{ControllerName}/GetAppointment");
            return response ?? new EditAppointmentModel();          
        }

        public async Task<HttpResponseMessage> CreateAppointment(EditAppointmentModel appointment)
        {
            var resposne = await _http.PostAsJsonAsync($"{ControllerName}/CreateAppointment", appointment);          
            return resposne;
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

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
        private readonly string BaseUrl = "api/appointment";

        public AppointmentDataService(HttpClient http)
        {
            _http = http;
        }

        public async Task<EditAppointmentModel> GetAppointment(int? id)
        {            
            var response = await _http.GetFromJsonAsync<EditAppointmentModel>($"{BaseUrl}/{id}");
            return response ?? new EditAppointmentModel();          
        }

        public async Task<HttpResponseMessage> HandleSubmit(EditAppointmentModel appointment)
        {
            if(appointment.Id == null)
            {
                var resposne = await CreateAppointment(appointment);
                return resposne;
            }
            else
            {
                var resposne = await UpdateAppointment(appointment);
                return resposne;
            }
        }

        private async Task<HttpResponseMessage> CreateAppointment(EditAppointmentModel appointment)
        {
            var resposne = await _http.PostAsJsonAsync(BaseUrl, appointment);

            return resposne;
        }

        private async Task<HttpResponseMessage> UpdateAppointment(EditAppointmentModel appointment)
        {
            var serializedAppointment = JsonSerializer.Serialize(appointment);
            var content = new StringContent(serializedAppointment, Encoding.UTF8, "application/json");
            var resposne = await _http.PutAsync(BaseUrl, content);

            return resposne;
        }

        public async Task<List<DisplayAppointmentModel>> GetAppointmentsInRange(AppointmentsRange appointmentsRange)
        {
            var response = await _http.PostAsJsonAsync($"{BaseUrl}/range", appointmentsRange);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = await response.Content.ReadFromJsonAsync<List<DisplayAppointmentModel>>();
                
                return result ?? new List<DisplayAppointmentModel>();
            }

            return new List<DisplayAppointmentModel>();
        }

        public async Task<HttpResponseMessage> DeleteAppointment(int id)
        {
            var resposne = await _http.DeleteAsync($"{BaseUrl}/{id}");

            return resposne;
        }
    }
}

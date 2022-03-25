using CompanyManager.Shared;
using System.Net.Http.Json;

namespace CompanyManager.Client.DataServices
{
    public interface IAppointmentDataService
    {
        Task<AppointmentEditForm> Get();
        Task<HttpResponseMessage> Create(AppointmentEditForm appointment);
    }

    public class AppointmentDataService : IAppointmentDataService
    {
        private readonly HttpClient _http;
        private readonly string ControllerName = "Appointment";

        public AppointmentDataService(HttpClient http)
        {
            _http = http;
        }

        public async Task<AppointmentEditForm> Get()
        {            
            var response = await _http.GetFromJsonAsync<AppointmentEditForm>(ControllerName);
            return response ?? new AppointmentEditForm();          
        }

        public async Task<HttpResponseMessage> Create(AppointmentEditForm appointment)
        {
            var resposne = await _http.PostAsJsonAsync(ControllerName, appointment);          
            return resposne;
        }
    }
}

using CompanyManager.Shared;
using System.Net.Http.Json;

namespace CompanyManager.Client.DataServices
{
    public interface IAppointmentDataService
    {
        Task<AppointmentViewModel> GetAsync();
    }

    public class AppointmentDataService : IAppointmentDataService
    {
        private readonly HttpClient _http;

        public AppointmentDataService(HttpClient http)
        {
            _http = http;
        }

        public async Task<AppointmentViewModel> GetAsync()
        {            
            var response = await _http.GetFromJsonAsync<AppointmentViewModel>("Appointment");
            return response ?? new AppointmentViewModel();          
        }
    }
}

using CompanyManager.Shared;
using System.Net.Http.Json;

namespace CompanyManager.Client.Services
{
    public interface IAppointmentService
    {
        Task<AppointmentViewModel> GetAsync();
    }

    public class AppointmentService : IAppointmentService
    {
        private readonly HttpClient http;

        public AppointmentService(HttpClient http)
        {
            this.http = http;
        }

        public async Task<AppointmentViewModel> GetAsync()
        {            
            var response = await http.GetFromJsonAsync<AppointmentViewModel>("Appointment");
            return response ?? new AppointmentViewModel();          
        }
    }
}

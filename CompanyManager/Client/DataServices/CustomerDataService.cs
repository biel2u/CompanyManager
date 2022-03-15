using System.Net.Http.Json;

namespace CompanyManager.Client.DataServices
{
    public interface ICustomerDataService
    {
        Task<List<string>> SearchCustomers(string searchValue);
    }
    public class CustomerDataService : ICustomerDataService
    {
        private readonly HttpClient _http;

        public CustomerDataService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<string>> SearchCustomers(string searchValue)
        {
            if (searchValue.Length < 2) return new List<string>();

            var response = await _http.GetFromJsonAsync<List<string>>($"Customer/SearchCustomers?searchValue={searchValue}");           
            return response ?? new List<string>();          
        }
    }
}

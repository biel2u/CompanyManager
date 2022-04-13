using CompanyManager.Shared;
using System.Net.Http.Json;

namespace CompanyManager.Client.DataServices
{
    public interface ICustomerDataService
    {
        Task<List<string>> SearchCustomers(string searchValue);
        Task<HttpResponseMessage> CreateCustomer(EditCustomerModel customer);
    }
    public class CustomerDataService : ICustomerDataService
    {
        private readonly HttpClient _http;
        private readonly string ControllerName = "Customer";

        public CustomerDataService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<string>> SearchCustomers(string searchValue)
        {
            if (searchValue.Length < 2) return new List<string>();

            var response = await _http.GetFromJsonAsync<List<string>>($"{ControllerName}/SearchCustomers?searchValue={searchValue}");           
            return response ?? new List<string>();          
        }

        public async Task<HttpResponseMessage> CreateCustomer(EditCustomerModel customer)
        {
            var resposne = await _http.PostAsJsonAsync($"{ControllerName}/CreateCustomer", customer);
            return resposne;
        }
    }
}

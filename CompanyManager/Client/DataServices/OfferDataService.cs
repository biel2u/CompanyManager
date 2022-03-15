using CompanyManager.Shared;
using System.Net.Http.Json;

namespace CompanyManager.Client.DataServices
{
    public interface IOfferDataService
    {
        Task<IEnumerable<IGrouping<string, OfferViewModel>>> GetOffers();
    }

    public class OfferDataService : IOfferDataService
    {
        private readonly HttpClient _http;

        public OfferDataService(HttpClient http)
        {
            _http = http;
        }

        public async Task<IEnumerable<IGrouping<string, OfferViewModel>>> GetOffers()
        {
            var response = await _http.GetFromJsonAsync<IEnumerable<IGrouping<string, OfferViewModel>>>("Offer");
            return response ?? Enumerable.Empty<IGrouping<string, OfferViewModel>>();
        }
    }
}

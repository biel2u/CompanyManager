using CompanyManager.Shared;
using System.Net.Http.Json;

namespace CompanyManager.Client.DataServices
{
    public interface IOfferDataService
    {
        Task<IEnumerable<OffersGroup>> GetOffers();
    }

    public class OfferDataService : IOfferDataService
    {
        private readonly HttpClient _http;

        public OfferDataService(HttpClient http)
        {
            _http = http;
        }

        public async Task<IEnumerable<OffersGroup>> GetOffers()
        {
            var response = await _http.GetFromJsonAsync<IEnumerable<OffersGroup>>("Offer");
            return response ?? Enumerable.Empty<OffersGroup>();
        }
    }
}

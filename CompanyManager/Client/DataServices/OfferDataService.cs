using CompanyManager.Shared;
using System.Net.Http.Json;

namespace CompanyManager.Client.DataServices
{
    public interface IOfferDataService
    {
        Task<IEnumerable<OffersGroup>> GetOffers(IEnumerable<DisplayOfferModel>? selectedOffers);
    }

    public class OfferDataService : IOfferDataService
    {
        private readonly HttpClient _http;

        public OfferDataService(HttpClient http)
        {
            _http = http;
        }

        public async Task<IEnumerable<OffersGroup>> GetOffers(IEnumerable<DisplayOfferModel>? selectedOffers)
        {
            var response = await _http.PostAsJsonAsync("Offer", selectedOffers);
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<OffersGroup>>();

            return result ?? Enumerable.Empty<OffersGroup>();
        }
    }
}

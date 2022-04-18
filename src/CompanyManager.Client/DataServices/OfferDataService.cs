using CompanyManager.Shared;
using System.Net.Http.Json;

namespace CompanyManager.Client.DataServices
{
    public interface IOfferDataService
    {
        Task<List<OffersGroup>> GetOffers(List<DisplayOfferModel> selectedOffers);
    }

    public class OfferDataService : IOfferDataService
    {
        private readonly HttpClient _http;
        private const string BaseUrl = "api/offer";

        public OfferDataService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<OffersGroup>> GetOffers(List<DisplayOfferModel> selectedOffers)
        {
            var offersRequest = new OffersRequest { SelectedOffers = selectedOffers };
            var response = await _http.PostAsJsonAsync(BaseUrl, offersRequest);
            var result = await response.Content.ReadFromJsonAsync<List<OffersGroup>>();

            return result ?? new List<OffersGroup>();
        }
    }
}

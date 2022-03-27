using CompanyManager.Shared;

namespace CompanyManager.Client.Helpers
{
    public interface IOfferSelector
    {
        Task<List<DisplayOfferModel>> BuildSelectedOffersCollection(DisplayOfferModel offer, List<DisplayOfferModel> selectedOffers);
        List<OffersGroup> FilterOffers(IEnumerable<OffersGroup> offersGroups, string offerSearchValue);
    }

    public class OfferSelector : IOfferSelector
    {
        public Task<List<DisplayOfferModel>> BuildSelectedOffersCollection(DisplayOfferModel offer, List<DisplayOfferModel> selectedOffers)
        {
            if (selectedOffers.Any(o => o.Id == offer.Id))
            {
                selectedOffers.Remove(offer);
            }
            else
            {
                selectedOffers.Add(offer);
            }

            return Task.FromResult(selectedOffers);
        }

        public List<OffersGroup> FilterOffers(IEnumerable<OffersGroup> offersGroups, string offerSearchValue)
        {
            var filteredOffers = new List<OffersGroup>();

            foreach (var group in offersGroups)
            {
                if (group.OfferGroupName.Contains(offerSearchValue, StringComparison.CurrentCultureIgnoreCase))
                {
                    filteredOffers.Add(new OffersGroup
                    {
                        OfferGroupName = group.OfferGroupName,
                        Offers = group.Offers
                    });
                }
                else
                {
                    var offersToAdd = group.Offers.Where(o => o.Name.Contains(offerSearchValue, StringComparison.CurrentCultureIgnoreCase));
                    if (offersToAdd.Any())
                    {
                        filteredOffers.Add(new OffersGroup
                        {
                            OfferGroupName = group.OfferGroupName,
                            Offers = offersToAdd
                        });
                    }
                }
            }

            return filteredOffers;
        }
    }
}

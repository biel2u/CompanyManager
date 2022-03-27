namespace CompanyManager.Shared
{
    public class OffersGroup
    {
        public string OfferGroupName { get; set; } = string.Empty;
        public IEnumerable<DisplayOfferModel> Offers { get; set; } = Enumerable.Empty<DisplayOfferModel>();
    }
}

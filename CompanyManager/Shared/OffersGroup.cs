namespace CompanyManager.Shared
{
    public class OffersGroup
    {
        public string OfferGroupName { get; set; } = string.Empty;
        public IEnumerable<OfferViewModel> Offers { get; set; } = Enumerable.Empty<OfferViewModel>();
    }
}

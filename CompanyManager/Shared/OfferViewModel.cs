namespace CompanyManager.Shared
{
    public class OfferViewModel
    {
        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public int TimeInMinutes { get; set; }

        public string OfferCategoryName { get; set; } = null!;
    }
}

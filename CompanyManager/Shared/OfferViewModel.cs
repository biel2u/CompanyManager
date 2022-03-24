namespace CompanyManager.Shared
{
    public class OfferViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int TimeInMinutes { get; set; }

        public bool IsSelected { get; set; }
    }
}

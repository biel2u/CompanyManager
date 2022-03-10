namespace CompanyManager.Shared
{
    public class AppointmentViewModel
    {
        public DateTime? Date { get; set; }

        public TimeSpan? Time { get; set; }

        public CustomerSelector CustomerSelector { get; set; } = new CustomerSelector();

        public string? Note { get; set; }

        public AppointmentStatus Status { get; set; }

        public OfferViewModel Offer { get; set; } = new OfferViewModel();
    }
}

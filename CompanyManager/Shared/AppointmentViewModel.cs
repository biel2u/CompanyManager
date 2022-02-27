namespace CompanyManager.Shared
{
    public class AppointmentViewModel
    {
        public DateTime? Date { get; set; }

        public TimeSpan? Time { get; set; }

        public CustomerViewModel Customer { get; set; } = new CustomerViewModel();

        public string? Note { get; set; }

        public AppointmentStatus Status { get; set; }

        public OfferViewModel Offer { get; set; } = new OfferViewModel();
    }
}

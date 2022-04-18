namespace CompanyManager.Shared
{
    public class DisplayAppointmentModel
    {
        public int Id { get; set; }
        public int StartRow { get; set; }
        public int EndRow { get; set; }
        public int DayOfWeek { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string OfferName { get; set; } = string.Empty;
        public int OffersCount { get; set; }
    }
}

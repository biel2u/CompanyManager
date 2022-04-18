namespace CompanyManager.Client.Models
{
    public class CalendarDate
    {
        public string NameOfDay { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string DisplayedDate { get; set; } = string.Empty;
        public int Column { get; set; }
        public bool IsCurrentDay { get; set; }
    }
}

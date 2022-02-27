namespace CompanyManager.Client.Models
{
    public class CalendarDate
    {
        public string Name { get; set; } = null!;
        public DateTime Date { get; set; }
        public int Column { get; set; }
        public bool IsCurrentDay { get; set; }
    }
}

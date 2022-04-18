namespace CompanyManager.Client.Models
{
    public class CalendarWeek
    {
        public List<CalendarDate> CalendarDates { get; set; } = new List<CalendarDate>();
        public int SelectedWeek { get; set; }
    }
}

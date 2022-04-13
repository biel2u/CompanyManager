namespace CompanyManager.Client.Models
{
    public class CalendarOptions
    {
        public System.Timers.Timer CalendarRefreshTimer { get; set; } = new System.Timers.Timer { Interval = 60000 };
        public bool ShouldScrollToCurrentTime { get; set; }
        public int CurrentDayOfWeek { get; set; }
        public int SelectedWeek { get; set; }
    }
}

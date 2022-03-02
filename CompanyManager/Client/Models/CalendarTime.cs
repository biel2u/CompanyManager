namespace CompanyManager.Client.Models
{
    public class CalendarTime
    {
        public string Hour { get; set; } = string.Empty;
        public int HourRow { get; set; }
        public int MinuteRow { get; set; }
        public bool IsCurrentTime { get; set; }
    }
}

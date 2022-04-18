namespace CompanyManager.Client.Models
{
    public class AppointmentSummary
    {
        public decimal SummarizedCost { get; set; }
        public int SummarizedTime { get; set; }
        public TimeSpan AppointmentTime { get; set; }
    }
}

namespace CompanyManager.Server.Helpers
{
    public interface IDateTimeProvider
    {
        DateTime GetCurrentDateTime();
    }

    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }
    }
}

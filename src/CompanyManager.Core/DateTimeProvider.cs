namespace CompanyManager.Core
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

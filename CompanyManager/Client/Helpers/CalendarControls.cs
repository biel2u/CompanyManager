using CompanyManager.Client.Models;

namespace CompanyManager.Client.Helpers
{
    public interface ICalendarControls
    {
        Task<List<CalendarDate>> SwitchWeek(List<CalendarDate> calendarDates, int selectedWeek, bool moveForward, int currentDayOfWeek);
        Task<List<CalendarDate>> SetCurrentWeek(List<CalendarDate> calendarDates, int currentDayOfWeek);
    }

    public class CalendarControls : ICalendarControls
    {
        public async Task<List<CalendarDate>> SwitchWeek(List<CalendarDate> calendarDates, int selectedWeek, bool moveForward, int currentDayOfWeek)
        {
            if (selectedWeek == 0)
            {
                var currentWeek = await SetCurrentWeek(calendarDates, currentDayOfWeek);
                return currentWeek;
            }

            const int daysInWeek = 7;
            var daysToAdd = moveForward ? daysInWeek : -daysInWeek;

            foreach (var day in calendarDates)
            {
                var date = day.Date.AddDays(daysToAdd);

                day.Date = date;
                day.DisplayedDate = date.ToString(CalendarConstants.DateTimeFormat);
                day.IsCurrentDay = false;
            }

            return calendarDates;
        }

        public Task<List<CalendarDate>> SetCurrentWeek(List<CalendarDate> calendarDates, int currentDayOfWeek)
        {
            var dayEnumeration = 0;
            var dateTimeNow = DateTime.Now;

            foreach (var day in calendarDates)
            {
                var dateDiff = dayEnumeration - currentDayOfWeek;
                var date = dateTimeNow.AddDays(dateDiff);

                day.Date = date;
                day.DisplayedDate = date.ToString(CalendarConstants.DateTimeFormat);
                day.IsCurrentDay = dateDiff == 0;
                dayEnumeration++;
            }

            return Task.FromResult(calendarDates);
        }
    }
}

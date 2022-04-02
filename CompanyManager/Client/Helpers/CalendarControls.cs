using CompanyManager.Client.Models;
using CompanyManager.Shared;

namespace CompanyManager.Client.Helpers
{
    public interface ICalendarControls
    {
        List<CalendarDate> SwitchWeek(List<CalendarDate> calendarDates, int selectedWeek, bool moveForward, int currentDayOfWeek);
        List<CalendarDate> SetCurrentWeek(List<CalendarDate> calendarDates, int currentDayOfWeek);
        CalendarWeek SetCalendarForSelectedWeek(List<CalendarDate> calendarDates, DateTime selectedDateTime, int currentDayOfWeek);
    }

    public class CalendarControls : ICalendarControls
    {
        private readonly ICalendar _calendar;

        public CalendarControls(ICalendar calendar)
        {
            _calendar = calendar;
        }

        public List<CalendarDate> SwitchWeek(List<CalendarDate> calendarDates, int selectedWeek, bool moveForward, int currentDayOfWeek)
        {
            if (selectedWeek == 0)
            {
                var currentWeek = SetCurrentWeek(calendarDates, currentDayOfWeek);
                return currentWeek;
            }

            var daysInWeek = calendarDates.Count();
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

        public List<CalendarDate> SetCurrentWeek(List<CalendarDate> calendarDates, int currentDayOfWeek)
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

            return calendarDates;
        }

        public CalendarWeek SetCalendarForSelectedWeek(List<CalendarDate> calendarDates, DateTime selectedDateTime, int currentDayOfWeek)
        {
            var dayDiff = (selectedDateTime - DateTime.Today).Days;
            var selectedWeek = (Math.Abs(dayDiff) + currentDayOfWeek) / 7;

            if (selectedWeek > 0)
            {
                if (dayDiff > 0)
                {
                    calendarDates = UpdateCalendarDates(calendarDates, selectedDateTime);
                }
                else if (dayDiff < 0)
                {
                    selectedWeek = -selectedWeek;
                    calendarDates = UpdateCalendarDates(calendarDates, selectedDateTime);
                }
            }
            else
            {
                calendarDates = SetCurrentWeek(calendarDates, currentDayOfWeek);
            }

            var calendarWeek = new CalendarWeek
            {
                CalendarDates = calendarDates,
                SelectedWeek = selectedWeek,
            };

            return calendarWeek;
        }

        private List<CalendarDate> UpdateCalendarDates(List<CalendarDate> calendarDates, DateTime selectedDateTime)
        {
            var dayOfWeek = _calendar.GetDayOfWeekWithMondayAsFirstDayOfTheWeek(selectedDateTime);

            for (var i = 0; i <= 6; i++)
            {
                var dateDiff = i - dayOfWeek;
                var date = selectedDateTime.AddDays(dateDiff);

                calendarDates[i].Date = date;
                calendarDates[i].DisplayedDate = date.ToString(CalendarConstants.DateTimeFormat);
                calendarDates[i].IsCurrentDay = false;
            }

            return calendarDates;
        }
    }
}

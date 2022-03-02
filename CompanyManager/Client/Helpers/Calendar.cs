using CompanyManager.Client.Models;

namespace CompanyManager.Client.Helpers
{
    public interface ICalendar
    {
        Task<List<CalendarDate>> BuildCalendarDates(int currentDayOfWeek);
        Task<List<CalendarTime>> BuildCalendarTimes();
        Task<int> GetCurrentDayOfWeekWithMondayAsFirstDayOfTheWeek();
        Task<List<CalendarTime>> SetCurrentHourAndMinuteRow(List<CalendarTime> calendarTimes);
    }

    public class Calendar : ICalendar
    {
        public Task<List<CalendarDate>> BuildCalendarDates(int currentDayOfWeek)
        {
            const int dateColumnStart = 2;
            var dayNames = new List<string>(6) { "Poniedziałek", "Wtorek", "Środa", "Czwartek", "Piątek", "Sobota", "Niedziela" };
            var dateTimeNow = DateTime.Now;
            var calendarDates = new List<CalendarDate>();

            for (var i = 0; i <= 6; i++)
            {
                var dateDiff = i - currentDayOfWeek;
                var date = dateTimeNow.AddDays(dateDiff);

                calendarDates.Add(new CalendarDate
                {
                    NameOfDay = dayNames[i],
                    Column = i + dateColumnStart,
                    Date = date,
                    DisplayedDate = date.ToString(CalendarConstants.DateTimeFormat),
                    IsCurrentDay = dateDiff == 0
                });
            }

            return Task.FromResult(calendarDates);
        }

        public Task<int> GetCurrentDayOfWeekWithMondayAsFirstDayOfTheWeek()
        {
            var currentDayColumn = (int)DateTime.Now.DayOfWeek - 1;
            const int sundayAsLastDayOfWeek = 6;

            if (currentDayColumn < 0)
            {
                currentDayColumn = sundayAsLastDayOfWeek;
            }

            return Task.FromResult(currentDayColumn);
        }

        public Task<List<CalendarTime>> BuildCalendarTimes()
        {
            var calendarTimes = new List<CalendarTime>();
            var dateTimeNow = DateTime.Now;
            var currentHourRow = dateTimeNow.Hour * CalendarConstants.GridHourRows;

            for (var i = 0; i < CalendarConstants.GridTotalRows; i = i + CalendarConstants.GridHourRows)
            {
                var hour = i / CalendarConstants.GridHourRows;
                calendarTimes.Add(new CalendarTime
                {
                    Hour = $"{hour}:00",
                    HourRow = i + CalendarConstants.TimeRowStart,
                    IsCurrentTime = i == currentHourRow
                });
            }

            SetMinuteRow(dateTimeNow, calendarTimes);
            return Task.FromResult(calendarTimes);
        }

        private void SetMinuteRow(DateTime dateTimeNow, List<CalendarTime> calendarTimes)
        {
            var currentMinuteRow = dateTimeNow.Minute / CalendarConstants.MinutesSampling;
            var currentHour = calendarTimes.Single(c => c.IsCurrentTime);
            currentHour.MinuteRow = currentHour.HourRow + currentMinuteRow;
        }

        public Task<List<CalendarTime>> SetCurrentHourAndMinuteRow(List<CalendarTime> calendarTimes)
        {
            var dateTimeNow = DateTime.Now;
            var currentHourRow = dateTimeNow.Hour * CalendarConstants.GridHourRows + CalendarConstants.TimeRowStart;
            var currentMinuteRow = dateTimeNow.Minute / CalendarConstants.MinutesSampling;

            foreach (var time in calendarTimes)
            {
                if (time.HourRow == currentHourRow)
                {
                    time.IsCurrentTime = true;
                    time.MinuteRow = currentMinuteRow + time.HourRow;
                }
                else
                {
                    time.IsCurrentTime = false;
                }
            }

            return Task.FromResult(calendarTimes);
        }
    }
}

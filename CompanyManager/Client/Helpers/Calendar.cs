using CompanyManager.Client.DataServices;
using CompanyManager.Client.Models;
using CompanyManager.Shared;

namespace CompanyManager.Client.Helpers
{
    public interface ICalendar
    {
        Task<List<CalendarDate>> BuildCalendarDates(int currentDayOfWeek);
        Task<List<CalendarTime>> BuildCalendarTimes();
        int GetDayOfWeekWithMondayAsFirstDayOfTheWeek(DateTime dateTime);
        Task<List<CalendarTime>> SetCurrentHourAndMinuteRow(List<CalendarTime> calendarTimes);
        Task<List<DisplayAppointmentModel>> SetAppointmentsOnCalendar(List<CalendarDate> calendarDates);
    }

    public class Calendar : ICalendar
    {
        private readonly IAppointmentDataService _appointmentDataService;

        public Calendar(IAppointmentDataService appointmentDataService)
        {
            _appointmentDataService = appointmentDataService;
        }

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

        public int GetDayOfWeekWithMondayAsFirstDayOfTheWeek(DateTime dateTime)
        {
            const int sundayAsLastDayOfWeek = 6;
            var dayOfWeek = (int)dateTime.DayOfWeek - 1;
            if (dayOfWeek < 0)
            {
                dayOfWeek = sundayAsLastDayOfWeek;
            }

            return dayOfWeek;
        }

        public Task<List<CalendarTime>> BuildCalendarTimes()
        {
            var dateTimeNow = DateTime.Now;
            var currentHourRow = dateTimeNow.Hour * CalendarConstants.GridHourRows;
            var calendarTimes = new List<CalendarTime>();

            for (var i = 0; i < CalendarConstants.GridTotalRows; i = i + CalendarConstants.GridHourRows)
            {
                var hour = i / CalendarConstants.GridHourRows;
                calendarTimes.Add(new CalendarTime
                {
                    Hour = $"{hour}:00",
                    HourRow = i + CalendarConstants.InitialTimeRow,
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
            var currentHourRow = dateTimeNow.Hour * CalendarConstants.GridHourRows + CalendarConstants.InitialTimeRow;
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

        public async Task<List<DisplayAppointmentModel>> SetAppointmentsOnCalendar(List<CalendarDate> calendarDates)
        {
            var appointments = await _appointmentDataService.GetAppointmentsInRange(new AppointmentsRange
            {
                StartDate = calendarDates.First().Date,
                EndDate = calendarDates.Last().Date
            });

            foreach (var appointment in appointments)
            {
                appointment.DayOfWeek = GetDayOfWeekWithMondayAsFirstDayOfTheWeek(appointment.StartDate);
                appointment.StartRow = CalculateAppointmentDisplayRow(appointment.StartDate);
                appointment.EndRow = CalculateAppointmentDisplayRow(appointment.EndDate);
            }

            return appointments;
        }

        private int CalculateAppointmentDisplayRow(DateTime dateTime)
        {
            var hourRow = (CalendarConstants.GridHourRows * dateTime.Hour) + CalendarConstants.InitialTimeRow;
            var minuteRow = dateTime.Minute / CalendarConstants.MinutesSampling;
            var row = hourRow + minuteRow;

            return row;
        }
    }
}

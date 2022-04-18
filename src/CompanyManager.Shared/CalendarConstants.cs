namespace CompanyManager.Shared
{
    public static class CalendarConstants
    {
        //Each hour on calendar consists of 12 rows.
        public static int GridHourRows { get; } = 12;

        //Total calendar rows. GridHoursRows*HoursInDay (12*24=288).
        public static int GridTotalRows { get; } = 288;

        //First column is hours section. 2-9 columns are for days, from monday to sunday
        public static int GridTotalColumns { get; } = 9;

        //Set to "2" because first column is occupied by hours.
        public static int CalendarAreaStartColumn { get;  } = 2;

        //There is no 0 row in CSS Grid. In certian calculations there is need to add this value.
        public static int InitialTimeRow { get; } = 1;

        //Each hour consists of 12 rows (GridHourRows) so 12*5=60minutes.
        public static int MinutesSampling { get;  } = 5;

        //DateTime string format for days representation.
        public static string DateTimeFormat { get; } = "dd/MM";
    }
}

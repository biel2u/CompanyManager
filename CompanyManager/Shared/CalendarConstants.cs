namespace CompanyManager.Shared
{
    public static class CalendarConstants
    {
        public static int GridHourRows { get; } = 12;
        public static int GridTotalRows { get; } = 288;
        public  static int CalendarAreaStartColumn { get;  } = 2;
        public static int InitialTimeRow { get; } = 1;
        public static int MinutesSampling { get;  } = 5;
        public static string DateTimeFormat { get; } = "dd/MM";
    }
}

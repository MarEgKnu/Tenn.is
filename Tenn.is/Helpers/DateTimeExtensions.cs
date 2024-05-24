namespace Tennis.Helpers
{
    public static class DateTimeExtensions
    {
        public static DateTime RoundDownToHour(this DateTime d)
        {
            return new DateTime(d.Year, d.Month, d.Day, d.Hour, 0, 0);
        }
    }
}

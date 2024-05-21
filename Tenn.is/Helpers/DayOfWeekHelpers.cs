namespace Tennis.Helpers
{
    public static class DayOfWeekHelpers
    {
        public static DayOfWeek IntToDayOfWeek(int num)
        {
            return ((DayOfWeek)(num % 7));
        }
    }
}

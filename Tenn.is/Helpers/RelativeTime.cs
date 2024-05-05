namespace Tennis.Helpers
{
    public enum RelativeTime
    {
        Past = 0,
        Ongoing = 1,
        Future = 2
    }
    public static class RelativeTimeHelpers
    {
        public static readonly Dictionary<RelativeTime, string> TimeDisplay = new Dictionary<RelativeTime, string>()
        {
            {RelativeTime.Past, "\U0001f7e5 Fortid" },
            {RelativeTime.Ongoing, "\U0001f7e8 Igangværende" },
            {RelativeTime.Future, "\U0001f7e9 Kommende" }
        };
    }
}

namespace Tennis.Helpers
{
    public class TimeBetween
    {

        public TimeBetween(DateTime startTime, DateTime endTime)
        {
            if (EndTime <= startTime)
            {
                throw new ArgumentException("Start Time is larger than End Time");
            }
            StartTime = startTime;
            EndTime = endTime;
        }
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public TimeSpan TimeSpan { get {
                return EndTime - StartTime;
            } }

    }
}

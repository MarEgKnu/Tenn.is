namespace Tennis.Helpers
{
    public class WeeklyTimeBetween
    {
        public WeeklyTimeBetween(TimeOnly startTime, TimeOnly endTime, DayOfWeek dayOfWeek)
        {
            StartTime = startTime;
            EndTime = endTime;
            StartDay = dayOfWeek;
        }
        public DayOfWeek StartDay { get; set; }

        public bool OverMidnight { get {
                if (StartTime > EndTime)
                {
                    return true;
                }
                else
                {
                    return false;
                }
     
            } }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; } 
        
        public TimeSpan TimeSpan { get {
            if (StartTime == EndTime)
                {
                    return new TimeSpan(24, 0, 0);
                }
            else
                {
                    return EndTime - StartTime;
                }
                    
            } }
    }
}

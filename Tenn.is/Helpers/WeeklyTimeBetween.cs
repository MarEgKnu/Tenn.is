using System.ComponentModel.DataAnnotations;

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
        public WeeklyTimeBetween()
        {
            StartTime = null;
            EndTime = null;
            StartDay = null;
        }
        public DayOfWeek? StartDay { get; set; }

        public bool OverMidnight { get {
                if (StartTime >= EndTime)
                {
                    return true;
                }
                else
                {
                    return false;
                }
     
            } }
        public TimeOnly? StartTime { get; set; }

        public TimeOnly? EndTime { get; set; } 
        
        public TimeSpan? TimeSpan { get {
            if (StartTime == null || EndTime == null)
                {
                    return null;
                }
            else if (StartTime == EndTime)
                {
                    return new TimeSpan(24, 0, 0);
                }
            else
                {
                    return EndTime - StartTime;
                }
                    
            } }
        public string? IsValidForLaneBooking { get
            {
                if (EndTime == null || StartTime ==  null || StartDay == null)
                {
                    return "Invalid tid";
                }
                else if (EndTime.Value.Hour <= StartTime.Value.Hour)
                {
                    return "Sluttid kan ikke være mindre end eller lig med starttid";
                }
                else if (!LaneBookingHelpers.HourOptions.ContainsKey(EndTime.Value.Hour) ||
                    !LaneBookingHelpers.HourOptions.ContainsKey(StartTime.Value.Hour))
                {
                    return $"Tidspunkt skal være mellem {LaneBookingHelpers.HourOptions.First().Value} og {LaneBookingHelpers.HourOptions.Last().Value}";
                }
                else
                {
                    return null;
                }

            } }
    }
}

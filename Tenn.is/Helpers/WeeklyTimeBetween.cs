using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace Tennis.Helpers
{
    public class WeeklyTimeBetween : IComparable
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
        public DateTime NextStart { get
            {
                DateTime now = DateTime.Now;
                int startDay = (int)StartDay.Value;
                int today = (int)now.DayOfWeek;
                
                DateTime result = now.Date.AddDays(MathExtensions.Modulo(startDay - today, 7)).AddHours(StartTime.Value.Hour).AddMinutes(StartTime.Value.Minute);
                if (result < now)
                {
                    return result.AddDays(7);
                }
                else
                {
                    return result;
                }
            } }
        public RelativeTime TimeStateAt(TimeOnly time)
        {
            if (time == null ||time < StartTime)
            {
                return RelativeTime.Past;
            }
            else if (time >= StartTime && time <= EndTime)
            {
                return RelativeTime.Ongoing;
            }
            else
            {
                return RelativeTime.Future;
            }
        }
        public override string ToString()
        {
            string dayString = LaneBookingHelpers.DayOptions[StartDay.Value] ?? "Ukendt dag";
            return $"{dayString} {StartTime.Value}-{EndTime.Value}";
        }
        public override bool Equals(object? obj)
        {
            WeeklyTimeBetween? timeBetween = obj as WeeklyTimeBetween;
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return StartTime == timeBetween.StartTime &&
                   EndTime == timeBetween.EndTime &&
                   StartDay == timeBetween.StartDay;
        }
        public static bool operator ==(WeeklyTimeBetween a, WeeklyTimeBetween b)
        {
            if ((object)a == null)
            {
                return (object)b == null;
            }
            return a.Equals(b);
        }
        public static bool operator !=(WeeklyTimeBetween a, WeeklyTimeBetween b)
        {
            return !(a == b);
        }
        public int CompareTo(object? obj)
        {
            if (obj == null)
            {
                return 1;
            }
            else if (StartDay == null || StartTime == null || EndTime == null)
            {
                return -1;
            }
            WeeklyTimeBetween otherTimeBetween = obj as WeeklyTimeBetween;
            if (StartDay.Value.CompareTo(otherTimeBetween.StartDay.Value) == 0)
            {
                if (StartTime.Value.CompareTo(otherTimeBetween.StartTime.Value) == 0)
                {
                    return EndTime.Value.CompareTo(otherTimeBetween.EndTime.Value);
                }
                else
                {
                    return StartTime.Value.CompareTo(otherTimeBetween.StartTime.Value);
                }
            }
            else
            {
                return StartDay.Value.CompareTo(otherTimeBetween.StartDay.Value);
            }
        }
    }
}

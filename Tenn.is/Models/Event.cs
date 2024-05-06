using System.ComponentModel.DataAnnotations;
using Tennis.Helpers;

namespace Tennis.Models
{
    public class Event
    {
        public Event() 
        {
            EventID = 0;
            Title = string.Empty;
            CancellationThresholdMinutes = 0;
            Description = string.Empty;
            EventTime = new TimeBetween(null, null);
            Cancelled = false;
        }
        public Event(int eventId, string title, int cancellationThresholdMinutes, string description, TimeBetween eventTime)
        {
            EventID = eventId;
            Title = title;
            CancellationThresholdMinutes = cancellationThresholdMinutes;
            Description = description;
            EventTime = eventTime;
            Cancelled = false;
        }

        public Event(int eventId, string title, int cancellationThresholdMinutes, string description, TimeBetween eventTime, bool cancelled)
        {
            EventID = eventId;
            Title = title;
            CancellationThresholdMinutes = cancellationThresholdMinutes;
            Description = description;
            EventTime = eventTime;
            Cancelled = cancelled;
        }


        public int EventID { get; set; }
        [Required(ErrorMessage = "Titel er krævet")]
        [StringLength(60)]
        public string Title { get; set; }

        public bool Cancelled { get; set; }
        [Required(ErrorMessage = "Afmældningsgrænse er krævet")]
        public int CancellationThresholdMinutes { get; set; }
        [Required(ErrorMessage = "Beskrivelse er krævet")]
        [StringLength(2000)]
        public string Description { get; set; }

        public TimeBetween EventTime {  get; set; }
        /// <summary>
        /// Returns a RelativeTime enum, representing if the event is currently ongoing, in the past, or in the future
        /// </summary>
        public RelativeTime EventState
        {
            get
            {
                return EventTime.TimeState;
            }
        }
        public TimeSpan? TimeUntillOrAfterStart
        {
            get
            {
                return EventTime.TimeUntillOrAfterStart;
            }
        }
        public string TimeTillStartDisplay
        {
            get
            {
                if (TimeUntillOrAfterStart.Value.TotalDays < 1 || TimeUntillOrAfterStart.Value.TotalDays <= -1)
                {
                    return $"{Math.Abs(TimeUntillOrAfterStart.Value.Hours)} time{(TimeUntillOrAfterStart.Value.Hours != 1 ? "r" : "")} og {Math.Abs(TimeUntillOrAfterStart.Value.Minutes)} minut{(TimeUntillOrAfterStart.Value.Minutes != 1 ? "ter" : "")}{(TimeUntillOrAfterStart.Value.TotalSeconds < 0 ? " siden" : " til")} start";
                }
                else
                {
                    return $"{Math.Abs(TimeUntillOrAfterStart.Value.Days)} dag{(TimeUntillOrAfterStart.Value.Days != 1 ? "e" : "") }, {Math.Abs(TimeUntillOrAfterStart.Value.Hours)} time{(TimeUntillOrAfterStart.Value.Hours != 1 ? "r" : "")} og {Math.Abs(TimeUntillOrAfterStart.Value.Minutes)} minut{(TimeUntillOrAfterStart.Value.Minutes != 1 ? "ter" : "")}{(TimeUntillOrAfterStart.Value.TotalSeconds < 0 ? " siden" : " til")} start";
                }
                    
            }
        }
        /// <summary>
        /// Returns a RelativeTime enum, representing if the event is ongoing, in the past, or in the future at the specified time
        /// </summary>
        public RelativeTime EventStateAt(DateTime date)
        {
            return EventTime.TimeStateAt(date);
            
        }
    }
}

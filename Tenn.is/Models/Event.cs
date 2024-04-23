using Tennis.Helpers;

namespace Tennis.Models
{
    public class Event
    {
        public Event(int eventId, string title, int cancellationThresholdMinutes, string description, TimeBetween eventTime)
        {
            EventID = eventId;
            Title = title;
            CancellationThresholdMinutes = cancellationThresholdMinutes;
            Description = description;
            EventTime = eventTime;
            Cancelled = false;
        }
        public int EventID { get; set; }

        public string Title { get; set; }

        public bool Cancelled { get; set; }

        public int CancellationThresholdMinutes { get; set; }

        public string Description { get; set; }

        public TimeBetween EventTime {  get; set; }
    }
}

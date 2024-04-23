﻿using System.ComponentModel.DataAnnotations;
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
        public int EventID { get; set; }
        [Required]
        [StringLength(60)]
        public string Title { get; set; }

        public bool Cancelled { get; set; }
        [Required]
        public int CancellationThresholdMinutes { get; set; }
        [Required]
        [StringLength(2000)]
        public string Description { get; set; }

        public TimeBetween EventTime {  get; set; }

        public bool IsInPast
        {
            get
            {
                return EventTime.IsInPast;
            }
        }
        public bool Ongoing
        {
            get
            {
                return EventTime.Ongoing;
            }
        }
    }
}

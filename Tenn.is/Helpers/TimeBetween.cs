using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Tennis.Helpers
{
    public class TimeBetween
    {

        public TimeBetween(DateTime? startTime, DateTime? endTime)
        {
            if (startTime == null || endTime == null)
            {
                _startTime = startTime;
                _endTime = endTime;
            }
            else if (endTime <= startTime)
            {
                throw new ArgumentException("Start Time is larger than End Time");
            }
            else
            {
                _startTime = startTime;
                _endTime = endTime;
            }
            
        }

        private DateTime? _startTime;

        private DateTime? _endTime;
        [Required, DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? StartTime
        {
            get { return _startTime; }
            set
            {
                if (value == null)
                {
                    _startTime = value;
                }
                else if (value >= EndTime)
                {
                    throw new ArgumentException("Start Time is larger than End Time");
                }
                else
                {
                    _startTime = value;
                }
                
            }
        }
        [Required, DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? EndTime
        {
            get { return _endTime; }
            set { 
                if (value == null)
                {
                    _endTime = value;
                }
                else if (value <= StartTime)
                {
                    throw new ArgumentException("Start Time is larger than End Time");
                }
                else
                {
                    _endTime = value;
                }
                 }
        }

        public TimeSpan? TimeSpan { get {
                return EndTime - StartTime;
            } }

        public void SetNewTime(DateTime? startTime, DateTime? endTime)
        {
            if (startTime == null || endTime == null)
            {
                _startTime = startTime;
                _endTime = endTime;
            }
            else if (endTime <= startTime)
            {
                throw new ArgumentException("Start Time is larger than End Time");
            }
            else
            {
                _startTime = startTime;
                _endTime = endTime;
            }
        }

        public bool IsInPast
        {
            get
            {
                return EndTime < DateTime.Now;
            }
        }
        public bool Ongoing
        {   
            get
            {
                return EndTime >= DateTime.Now && StartTime <= DateTime.Now;
            }
        }
        public bool OngoingAt(DateTime date)
        {
            return EndTime >= date && StartTime <= date;
        }

    }
}

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
                throw new ArgumentException("Starttid kan ikke være senere end sluttid");
            }
            else
            {
                _startTime = startTime;
                _endTime = endTime;
            }
            
        }

        private DateTime? _startTime;

        private DateTime? _endTime;
        [Required(ErrorMessage = "Startid er krævet"), DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
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
                    throw new ArgumentException("Starttid kan ikke være senere end sluttid");
                }
                else
                {
                    _startTime = value;
                }
                
            }
        }
        [Required(ErrorMessage = "Sluttid er krævet"), DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
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
                    throw new ArgumentException("Starttid kan ikke være senere end sluttid");
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
                throw new ArgumentException("Starttid kan ikke være senere end sluttid");
            }
            else
            {
                _startTime = startTime;
                _endTime = endTime;
            }
        }

        public RelativeTime TimeState
        {
            get
            {
                if (EndTime < DateTime.Now)
                {
                    return RelativeTime.Past;
                }
                else if (StartTime > DateTime.Now)
                {
                    return RelativeTime.Future;
                }
                else
                {
                    return RelativeTime.Ongoing;
                }
                
            }
        }

        public TimeSpan? TimeUntillStart { get
            {
                return StartTime - DateTime.Now;
            } }
        public RelativeTime TimeStateAt(DateTime date)
        {
            if (EndTime < date)
            {
                return RelativeTime.Past;
            }
            else if (StartTime > date)
            {
                return RelativeTime.Future;
            }
            else
            {
                return RelativeTime.Ongoing;
            }   
        }

    }
}

namespace Tennis.Helpers
{
    public class TimeBetween
    {

        public TimeBetween(DateTime startTime, DateTime endTime)
        {
            if (endTime <= startTime)
            {
                throw new ArgumentException("Start Time is larger than End Time");
            }
            StartTime = startTime;
            EndTime = endTime;
        }

        private DateTime _startTime;

        private DateTime _endTime;

        public DateTime StartTime
        {
            get { return _startTime; }
            set
            {
                if (value > EndTime)
                {
                    throw new ArgumentException("Start Time is larger than End Time");
                }
                _startTime = value;
            }
        }

        public DateTime EndTime
        {
            get { return _endTime; }
            set { 
                if (value < StartTime)
                {
                    throw new ArgumentException("Start Time is larger than End Time");
                }
                _endTime = value; }
        }

        public TimeSpan TimeSpan { get {
                return EndTime - StartTime;
            } }

    }
}

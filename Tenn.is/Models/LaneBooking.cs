using Tennis.Helpers;

namespace Tennis.Models
{
    public class LaneBooking
    {
        public int BookingID { get; set; }
        public int LaneNumber { get; set; }
        public bool Cancelled { get; set; }
        public TimeBetween _timeBetween { get; set; }

        public LaneBooking(int lanenumber, TimeBetween timeBetween, int bookingID, bool cancelled)
        {
            bookingID++;
            //BookingID ++;
            LaneNumber = lanenumber;
            cancelled = false;
            //Cancelled = cancelled;
            _timeBetween = timeBetween;
        }

        public LaneBooking()
        {

        }

        public override string ToString()
        {
            return $"";
        }
    }
}

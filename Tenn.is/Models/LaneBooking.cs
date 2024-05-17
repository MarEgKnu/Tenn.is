using Tennis.Helpers;

namespace Tennis.Models
{
    public class LaneBooking
    {
        public int BookingID { get; set; }
        public int LaneNumber { get; set; }
        public bool Cancelled { get; set; }
        public DateTime DateStart { get; set; }

        public LaneBooking(int lanenumber, DateTime dateStart, int bookingID, bool cancelled)
        {
            BookingID = bookingID;
            LaneNumber = lanenumber;
            Cancelled = cancelled;
            DateStart = dateStart;
        }

        public LaneBooking() 
        {

        }

        public RelativeTime BookingState
        {
            get
            {
                if (DateStart.AddHours(1) < DateTime.Now)
                {
                    return RelativeTime.Past;
                }
                else if (DateStart > DateTime.Now)
                {
                    return RelativeTime.Future;
                }
                else
                {
                    return RelativeTime.Ongoing;
                }

            }
        }

        public override string ToString()
        {
            return $"";
        }
    }
}

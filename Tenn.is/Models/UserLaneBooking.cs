using Tennis.Helpers;

namespace Tennis.Models
{
    public class UserLaneBooking : LaneBooking
    {
        public int UserID { get; set; }
        public int MateID { get; set; }
        public UserLaneBooking(int bookingID, int lanenumber, DateTime DateStart, int userID, int mateID,  bool cancelled) : base(lanenumber, DateStart, bookingID, cancelled)
        {
            UserID = userID;
            MateID = mateID;
        }

        public UserLaneBooking() { }

    }
}

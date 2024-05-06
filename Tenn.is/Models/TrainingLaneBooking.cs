using Tennis.Helpers;

namespace Tennis.Models
{
    public class TrainingLaneBooking : LaneBooking
    {
        public TrainingTeam  trainingTeam { get; set; }
        public TrainingLaneBooking(int lanenumber, DateTime DateStart, int bookingID, bool cancelled, TrainingTeam trainingTeam) : base(lanenumber, DateStart, bookingID, cancelled)
        {
            this.trainingTeam = trainingTeam;
        }
    }
}

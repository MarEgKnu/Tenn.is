using Tennis.Helpers;

namespace Tennis.Models
{
    public class TrainingLaneBooking : LaneBooking
    {
        public TrainingTeam  trainingTeam { get; set; }
        public TrainingLaneBooking(int lanenumber, TimeBetween timeBetween, int bookingID, bool cancelled, TrainingTeam trainingTeam) : base(lanenumber, timeBetween, bookingID, cancelled)
        {
            this.trainingTeam = trainingTeam;
        }
    }
}

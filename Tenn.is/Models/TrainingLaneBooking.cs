using Tennis.Helpers;

namespace Tennis.Models
{
    public class TrainingLaneBooking : LaneBooking
    {
        public TrainingTeam  trainingTeam { get; set; }
        public bool Automatic { get; set; }
        public TrainingLaneBooking(int lanenumber, DateTime dateStart, int bookingID, bool cancelled, TrainingTeam trainingTeam, bool automatic) : base(lanenumber, dateStart , bookingID, cancelled)
        {
            this.trainingTeam = trainingTeam;
            Automatic = automatic;
        }
    }
}

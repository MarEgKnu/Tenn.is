using Tennis.Models;

namespace Tennis.Interfaces
{
    public interface ILaneBookingService
    {
        bool CancelLaneBonking(int id);

        public int GetNoOfBookings(int laneID, DateTime minTime, DateTime maxTime);
        Lane GetAnyFreeLane(DateTime time);

        bool CreateLaneBooking(UserLaneBooking laneBooking);

        bool CreateLaneBooking(TrainingLaneBooking laneBooking);

        bool DeleteLaneBooking(int id);

        bool EditLaneBooking(UserLaneBooking laneBooking, int id);

        int DeleteAutomaticBookingOnTeam(int teamID);

        bool EditLaneBooking(TrainingLaneBooking laneBooking, int id);

        List<T> GetAllLaneBookings<T>() where T : LaneBooking;

        List<LaneBooking> IsLaneBooked(List<int> laneIDs, DateTime time);

        LaneBooking? IsLaneBooked(int laneID, DateTime time);
        public UserLaneBooking GetUserLaneBookingById(int id);
        public TrainingLaneBooking GetTrainingLaneBookingById(int id);
    }
}

using Tennis.Models;

namespace Tennis.Interfaces
{
    public interface ILaneBookingService
    {
        bool CancelLaneBonking(int id);

        bool CreateLaneBooking(UserLaneBooking laneBooking);

        bool CreateLaneBooking(TrainingLaneBooking laneBooking);

        bool DeleteLaneBooking(int id);

        bool EditLaneBooking(UserLaneBooking laneBooking, int id);

        bool EditLaneBooking(TrainingLaneBooking laneBooking, int id);

        List<T> GetAllLaneBookings<T>() where T : LaneBooking;

        public UserLaneBooking GetUserLaneBookingById(int id);
        public TrainingLaneBooking GetTrainingLaneBookingById(int id);
    }
}

using Tennis.Models;

namespace Tennis.Interfaces
{
    public interface ILaneBookingService
    {
        bool CreateLaneBooking(UserLaneBooking laneBooking);

        bool CreateLaneBooking(TrainingLaneBooking laneBooking);

        bool DeleteLaneBooking(int id);

        bool EditLaneBooking(LaneBooking laneBooking, int id);

        List<T> GetAllLaneBookings<T>() where T : LaneBooking;

        LaneBooking GetLaneBookingById(int id);
    }
}

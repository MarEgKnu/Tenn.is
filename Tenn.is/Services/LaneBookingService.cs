using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Services
{
    public class LaneBookingService : Connection, ILaneBookingService
    {
        public LaneBookingService()
        {
            connectionString = Secret.ConnectionString;
        }
        public LaneBookingService(bool test)
        {
            if (test)
            {
                connectionString = Secret.ConnectionStringTest;
            }
            else
            {
                connectionString = Secret.ConnectionString;
            }
        }
        public bool CreateLaneBooking(UserLaneBooking laneBooking)
        {
            throw new NotImplementedException();
        }

        public bool CreateLaneBooking(TrainingLaneBooking laneBooking)
        {
            throw new NotImplementedException();
        }

        public bool DeleteLaneBooking(int id)
        {
            throw new NotImplementedException();
        }

        public bool EditLaneBooking(LaneBooking laneBooking, int id)
        {
            throw new NotImplementedException();
        }

        public List<T> GetAllLaneBookings<T>() where T : LaneBooking
        {
            throw new NotImplementedException();
        }

        public LaneBooking GetLaneBookingById(int id)
        {
            throw new NotImplementedException();
        }
    }
}

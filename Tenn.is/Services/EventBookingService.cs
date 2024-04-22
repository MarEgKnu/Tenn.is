using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Services
{
    public class EventBookingService : Connection, IEventBookingService
    {
        public EventBookingService()
        {
            connectionString = Secret.ConnectionString;
        }
        public EventBookingService(bool test)
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
        public bool CreateEventBooking(EventBooking eventBooking)
        {
            throw new NotImplementedException();
        }

        public bool DeleteEventBooking(int id)
        {
            throw new NotImplementedException();
        }

        public bool EditEventBooking(EventBooking eventBooking, int id)
        {
            throw new NotImplementedException();
        }

        public List<EventBooking> GetAllEventBookings()
        {
            throw new NotImplementedException();
        }

        public EventBooking GetEventBookingById(int id)
        {
            throw new NotImplementedException();
        }
    }
}

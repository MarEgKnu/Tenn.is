using Tennis.Models;

namespace Tennis.Interfaces
{
    public interface IEventBookingService
    {
        public event Action<EventBooking> OnCreate;
        public event Action<EventBooking> OnDelete;
        public event Action<EventBooking> OnEdit;
        bool CreateEventBooking(EventBooking eventBooking);


        bool DeleteEventBooking(int id);

        bool EditEventBooking(EventBooking eventBooking, int id);

        List<EventBooking> GetAllEventBookings();

        EventBooking GetEventBookingById(int id);


        List<EventBooking> GetEventBookingsByUser(int userID);

        List<EventBooking> GetAllBookingsByEventID(int id);

        EventBooking? AlreadyHasEventBooking(int userID, int eventID);

        /// <summary>
        /// Returns a bool specifiying if the given user is allowed to create a booking for the specified event
        /// </summary>
        /// <param name="user"></param>
        /// <param name="evt"></param>
        /// <returns></returns>
        bool CanBook(User user, Event evt);           

      
    }
}

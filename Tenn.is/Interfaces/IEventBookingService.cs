using Tennis.Models;

namespace Tennis.Interfaces
{
    public interface IEventBookingService
    {
        bool CreateEventBooking(EventBooking eventBooking);


        bool DeleteEventBooking(int id);

        bool EditEventBooking(EventBooking eventBooking, int id);

        List<EventBooking> GetAllEventBookings();

        EventBooking GetEventBookingById(int id);

        List<EventBooking> GetAllBookingsByEventID(int id);

        EventBooking? AlreadyHasEventBooking(int userID, int eventID);


        bool CanBook(User user, Event evt);           

        /// <summary>
        /// Returns a list of filtered eventsbookings based on the conditions from the database
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        List<EventBooking> GetEventBookingsOnConditions(List<Predicate<EventBooking>> conditions);
        /// <summary>
        /// Returns a list of filtered eventsbookings based on the conditions from the eventsbookings parameter
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="events"></param>
        /// <returns></returns>
        List<EventBooking> GetEventBookingsOnConditions(List<Predicate<EventBooking>> conditions, List<EventBooking> eventBookings);

    }
}

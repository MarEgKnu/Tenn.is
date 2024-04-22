namespace Tennis.Interfaces
{
    public interface IEventBookingService
    {
        bool CreateEventBooking(EventBooking eventBooking);

        bool DeleteEventBooking(int id);

        bool EditEventBooking(EventBooking eventBooking, int id);

        List<EventBooking> GetAllEventBookings();

        EventBooking GetEventBookingById(int id);
    }
}

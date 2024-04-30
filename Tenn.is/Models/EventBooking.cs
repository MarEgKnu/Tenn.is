namespace Tennis.Models
{
    public class EventBooking
    {
        public EventBooking()
        {
            
        }
        public EventBooking(Event evt, User user, string? comment )
        {
            Event = evt;
            User = user;
            Comment = comment;
            
        }
        public EventBooking(int id, Event evt, User user, string? comment)
        {
            Event = evt;
            User = user;
            Comment = comment;
            BookingID = id;

        }
        public int BookingID { get; }

        public Event Event { get; set; }

        public User User { get; set; }

        public string? Comment { get; set; }
    }
}

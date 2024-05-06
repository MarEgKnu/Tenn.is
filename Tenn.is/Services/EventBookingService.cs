using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using Tennis.Exceptions;
using Tennis.Helpers;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Services
{
    public class EventBookingService : Connection, IEventBookingService
    {
        private IEventService _eventService;
        private IUserService _userService;
        public EventBookingService(IEventService eventService, IUserService userService)
        {
            connectionString = Secret.ConnectionString;
            _eventService = eventService;
            _userService = userService;

        }
        public EventBookingService(bool test, IEventService eventService, IUserService userService)
        {
            if (test)
            {
                connectionString = Secret.ConnectionStringTest;
            }  
            else
            {
                connectionString = Secret.ConnectionString;
            }
            _eventService = eventService;
            _userService = userService;
        }
        public event Action<EventBooking> OnCreate;
        public event Action<EventBooking> OnDelete;
        public event Action<EventBooking> OnEdit;

        private string insertString = "INSERT INTO EventBookings (EventID, UserID, Comment)\n" +
                                      "OUTPUT INSERTED.BookingID\n" +
                                      "VALUES (@EventID, @UserID, @Comment)";
        private string getAllString = "SELECT * FROM EventBookings";
        private string getAllOnEventID = "SELECT * FROM EventBookings\n" +
                                         "WHERE EventID = @EventID";
        private string getBookingOnUserIDAndEventID = "SELECT * FROM EventBookings\n" +
                                                      "WHERE EventID = @EventID AND UserID = @UserID";
        private string getAllOnBookingID = "SELECT * FROM EventBookings\n" +
                                           "WHERE BookingID = @BookingID";
        private string deleteString = "DELETE FROM EventBookings WHERE BookingID = @BookingID";

        private string getAllOnUserID = "SELECT * FROM EventBookings\n" +
                                        "WHERE UserID = @UserID";
        public bool CreateEventBooking(EventBooking eventBooking)
        {
            if (eventBooking == null || eventBooking.User == null || 
                eventBooking.Event == null)
            {
                return false;
            }
            Event evt = _eventService.GetEventByNumber(eventBooking.Event.EventID);
            User user = _userService.GetUserById(eventBooking.User.UserId);
            if (evt == null)
            {
                throw new EventNotFoundException("Event kunne ikke findes til booking");
            }
            else if (user == null)
            {
                throw new UserNotFoundException("Bruger kunne ikke findes til booking");
            }
            else if (evt.EventState == RelativeTime.Past)
            {
                throw new InvalidTimeException("Booking kan ikke blive oprettet på en event i fortid");
            }
            else if (AlreadyHasEventBooking(user.UserId, evt.EventID) != null)
            {
                throw new DuplicateBookingException("Kan ikke oprette flere bookinger til den samme event");
            }
            else if (evt.Cancelled)
            {
                return false;
            }
            eventBooking.Event = evt;
            eventBooking.User = user;
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                try
                {
                    SqlCommand command = new SqlCommand(insertString, connection);
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@EventID", eventBooking.Event.EventID);
                    command.Parameters.AddWithValue("@UserID", eventBooking.User.UserId);
                    _ = (eventBooking.Comment.IsNullOrEmpty()) ? command.Parameters.AddWithValue("@Comment", DBNull.Value) : command.Parameters.AddWithValue("@Comment", eventBooking.Comment);
                    //command.Parameters.AddWithValue("@Comment", eventBooking.Comment);
                    int primaryKey = (int)command.ExecuteScalar();
                    OnCreate?.Invoke(GetEventBookingById(primaryKey));
                    return true;

                }
                catch (SqlException ex)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return false;
        }

        public bool DeleteEventBooking(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    EventBooking deletedBooking = GetEventBookingById(id);
                    SqlCommand command = new SqlCommand(deleteString, connection);
                    command.Parameters.AddWithValue("@BookingId", id);
                    if (command.ExecuteNonQuery() != 1)
                    {
                        return false;
                    }
                    else
                    {
                        OnDelete?.Invoke(deletedBooking);
                        return true;
                    }

                }
                catch (SqlException ex)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return false;
        }

        public bool EditEventBooking(EventBooking eventBooking, int id)
        {
            throw new NotImplementedException();
        }

        public List<EventBooking> GetAllEventBookings()
        {
            List<EventBooking> eventBookings = new List<EventBooking>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(getAllString, connection);
                    command.Connection.Open();
                    eventBookings = ProcessReader(command);
                }
                catch (SqlException ex)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return eventBookings;
        }

        public EventBooking GetEventBookingById(int id)
        {
            List<EventBooking> eventBookings = new List<EventBooking>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                try
                {
                    SqlCommand command = new SqlCommand(getAllOnBookingID, connection);
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@BookingID", id);
                    eventBookings = ProcessReader(command);

                    //OnCreate?.Invoke(GetEventByNumber(primaryKey));

                }
                catch (SqlException ex)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return eventBookings.FirstOrDefault();
        }

        public List<EventBooking> GetAllBookingsByEventID(int id)
        {
            List<EventBooking> eventBookings = new List<EventBooking>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                try
                {
                    SqlCommand command = new SqlCommand(getAllOnEventID, connection);
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@EventID", id);
                    eventBookings = ProcessReader(command);

                    //OnCreate?.Invoke(GetEventByNumber(primaryKey));

                }
                catch (SqlException ex)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return eventBookings;
        }


        public List<EventBooking> GetEventBookingsByUser(int userID)
        {
            List<EventBooking> eventBookings = new List<EventBooking>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                try
                {
                    SqlCommand command = new SqlCommand(getAllOnUserID, connection);
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@UserID", userID);
                    eventBookings = ProcessReader(command);

                    //OnCreate?.Invoke(GetEventByNumber(primaryKey));

                }
                catch (SqlException ex)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return eventBookings;
        }

        public EventBooking? AlreadyHasEventBooking(int userID, int eventID)
        {
            List<EventBooking> eventBookings = new List<EventBooking>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                try
                {
                    SqlCommand command = new SqlCommand(getBookingOnUserIDAndEventID, connection);
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@EventID", eventID);
                    command.Parameters.AddWithValue("@UserID", userID);
                    eventBookings = ProcessReader(command);
                    return eventBookings.FirstOrDefault();

                    //OnCreate?.Invoke(GetEventByNumber(primaryKey));

                }
                catch (SqlException ex)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
        private List<EventBooking> ProcessReader(SqlCommand command)
        {
            List<EventBooking> eventBookings = new List<EventBooking>();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int bookingID = reader.GetInt32("BookingID");
                int evtID = reader.GetInt32("EventID");
                int uID = reader.GetInt32("UserID");
                string? comment = DBReaderHelper.GetStringOrNull(reader, 3);
                Event @event = _eventService.GetEventByNumber(evtID);
                User user = _userService.GetUserById(uID);
                EventBooking eventBooking = new EventBooking(bookingID, @event, user, comment);
                eventBookings.Add(eventBooking);
            }
            reader.Close();
            return eventBookings;
        }
      
        public bool CanBook(User user, Event evt)
        {
            if (user == null || evt == null) return false;
            EventBooking booking = AlreadyHasEventBooking(user.UserId, evt.EventID);
            return booking == null && !evt.Cancelled && evt.EventState != RelativeTime.Past;
        }
    }
}

using System.Data;
using Microsoft.Data.SqlClient;
using Tennis.Helpers;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Services
{
    public class UserService : Connection, IUserService
    {

        private string _getAllSQL = "SELECT * FROM Users";
        private string _getByIdSQL = "SELECT * FROM Users WHERE UserID = @UID";
        private string _insertSQL = "INSERT INTO Users VALUES (@UID, @UNAME, @FNAME, @LNAME, @PWORD, @EMAIL, @PHONE, @ADMIN, @RWORD)";
        private string _deleteSQL = "DELETE FROM Users WHERE UserID = @UID";
        private string _editSQL = "UPDATE Users SET UserName = @UNAME, FirstName = @FNAME, LastName = @LNAME, Password = @PWORD, Phone = @PHONE, Email = @EMAIL, Administrator = @ADMIN, RandomPassword = @RWORD WHERE UserID = @UID";
        private string _getAllLaneBookingsWithUserIdSQL = "Select * FROM lANEBOOKINGS WHERE UserID = @UID OR MateID = @UID";
        private string _getByUserNameSQL = "SELECT * FROM Users WHERE UserName = @UserName";
        private string _getUtilUsers = "SELECT * FROM Users\n" +
                                      "WHERE UserID < 1";
        private string _getNonUtilUsers = "SELECT * FROM Users\n" +
                                         "WHERE UserID > 0";
        private string _getAllEventsWithUserIdSQL = "SELECT EVENTS.EventID, EVENTS.Title, EVENTS.Description, EVENTS.Cancelled, EVENTS.DateStart, EVENTS.DateEnd, EVENTS.CancellationThreshold FROM EVENTS INNER JOIN EVENTBOOKINGS ON EVENTS.EventID = EVENTBOOKINGS.EventID WHERE EVENTBOOKINGS.UserID = @UserID";

        public UserService()
        {
            connectionString = Secret.ConnectionString;
        }
        public UserService(bool test)
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
        public List<UserLaneBooking> GetAllLaneBookingsWithUserId(int userId)
        {
            List<UserLaneBooking> userLaneBooking = new List<UserLaneBooking>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(_getAllLaneBookingsWithUserIdSQL, conn);
                    command.Parameters.AddWithValue("@UID", userId);
                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int BookingID = reader.GetInt32("BookingID");
                        int LaneNumber = reader.GetInt32("LaneNumber");
                        bool Cancelled = reader.GetBoolean("Cancelled");
                        DateTime DateStart = reader.GetDateTime("DateStart");
                        int UserID = reader.GetInt32("UserID");
                        int MateID = reader.GetInt32("MateID");
                        userLaneBooking.Add(new UserLaneBooking(BookingID, LaneNumber,  DateStart, UserID, MateID, Cancelled));
                    }
                    reader.Close();
                }
                catch (SqlException sqlExp)
                {
                    Console.WriteLine("Database error" + sqlExp.Message);
                    throw sqlExp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("General error" + ex.Message);
                    throw ex;
                }
            }
            return userLaneBooking;



        }


        public bool CreateUser(User user)
        {
            if (user.UserId < 1)
            {
                return false;
            }
            else
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        SqlCommand command = new SqlCommand(_insertSQL, conn);
                        command.Parameters.AddWithValue("@UID", user.UserId);
                        command.Parameters.AddWithValue("@UNAME", user.Username);
                        command.Parameters.AddWithValue("@FNAME", user.FirstName);
                        command.Parameters.AddWithValue("@LNAME", user.LastName);
                        command.Parameters.AddWithValue("@PWORD", user.Password);
                        command.Parameters.AddWithValue("@EMAIL", user.Email);
                        command.Parameters.AddWithValue("@PHONE", user.Phone);
                        command.Parameters.AddWithValue("@ADMIN", user.Administrator);
                        command.Parameters.AddWithValue("@RWORD", user.RandomPassword);
                        command.Connection.Open();
                        int noOfRows = command.ExecuteNonQuery();
                        return noOfRows == 1;
                    }
                    catch (SqlException sqlExp)
                    {
                        Console.WriteLine("Database error" + sqlExp.Message);
                        throw sqlExp;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("General error" + ex.Message);
                        throw ex;
                    }
                }
                return false;
            }
        }

        public bool DeleteUser(int id)
        {
            User user = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(_deleteSQL, conn);
                    command.Parameters.AddWithValue("@UID", id);
                    user = GetUserById(id);
                    if (user != null)
                    {
                        if (user.UserId != 0)
                        {
                            command.Connection.Open();
                            command.ExecuteNonQuery();
                        } else
                        {
                            user = null;
                        }
                    }
                }
                catch (SqlException sqlExp)
                {
                    Console.WriteLine("Database error" + sqlExp.Message);
                    throw sqlExp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("General error" + ex.Message);
                    throw ex;
                }

            }
            return user != null;
        }

            public bool EditUser(User user, int id)
            {
            if (user.UserId < 1 || id < 1)
            {
                return false;
            }
            else
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        SqlCommand command = new SqlCommand(_editSQL, conn);
                        command.Parameters.AddWithValue("@UID", id);
                        command.Parameters.AddWithValue("@UNAME", user.Username);
                        command.Parameters.AddWithValue("@FNAME", user.FirstName);
                        command.Parameters.AddWithValue("@LNAME", user.LastName);
                        command.Parameters.AddWithValue("@PWORD", user.Password);
                        command.Parameters.AddWithValue("@EMAIL", user.Email);
                        command.Parameters.AddWithValue("@PHONE", user.Phone);
                        command.Parameters.AddWithValue("@ADMIN", user.Administrator);
                        command.Parameters.AddWithValue("@RWORD", user.RandomPassword);
                        command.Connection.Open();
                        int noOfRows = command.ExecuteNonQuery();
                        return noOfRows == 1;
                    }
                    catch (SqlException sqlExp)
                    {
                        Console.WriteLine("Database error" + sqlExp.Message);
                        throw sqlExp;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("General error" + ex.Message);
                        throw ex;
                    }
                }
                return false;
            }
        }

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(_getAllSQL, conn);
                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int userID = reader.GetInt32("UserID");
                        string username = reader.GetString("UserName");
                        string firstname = reader.GetString("FirstName");
                        string lastname = reader.GetString("LastName");
                        string password = reader.GetString("Password");
                        string email = reader.GetString("Email");
                        string phone = reader.GetString("Phone");
                        bool admin = reader.GetBoolean("Administrator");
                        bool randompassword = reader.GetBoolean("RandomPassword");
                        users.Add(new User(userID, username, firstname, lastname, email, password, phone, admin, randompassword));
                    }
                    reader.Close();
                } catch (SqlException sqlExp) { 
                Console.WriteLine("Database error" + sqlExp.Message);
                    throw sqlExp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("General error" +  ex.Message);
                    throw ex;
                }
            }
            return users;
        }



        public List<User> GetAllUsers(bool isUtilityUser)
        {
            List<User> users = new List<User>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command;
                    if (isUtilityUser)
                    {
                        command = new SqlCommand(_getUtilUsers, conn);
                    }
                    else
                    {
                        command = new SqlCommand(_getNonUtilUsers, conn);
                    }
                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int userID = reader.GetInt32("UserID");
                        string username = reader.GetString("UserName");
                        string firstname = reader.GetString("FirstName");
                        string lastname = reader.GetString("LastName");
                        string password = reader.GetString("Password");
                        string email = reader.GetString("Email");
                        string phone = reader.GetString("Phone");
                        bool admin = reader.GetBoolean("Administrator");
                        bool randompassword = reader.GetBoolean("RandomPassword");
                        users.Add(new User(userID, username, firstname, lastname, email, password, phone, admin, randompassword));
                    }
                    reader.Close();
                }
                catch (SqlException sqlExp)
                {
                    Console.WriteLine("Database error" + sqlExp.Message);
                    throw sqlExp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("General error" + ex.Message);
                    throw ex;
                }
            }
            return users;
        }


        public User GetUserById(int id)
        {
            User user = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(_getByIdSQL, conn);
                    command.Parameters.AddWithValue("@UID", id);
                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string username = reader.GetString("UserName");
                        string firstname = reader.GetString("FirstName");
                        string lastname = reader.GetString("LastName");
                        string password = reader.GetString("Password");
                        string email = reader.GetString("Email");
                        string phone = reader.GetString("Phone");
                        bool admin = reader.GetBoolean("Administrator");
                        bool randompassword = reader.GetBoolean("RandomPassword");
                        user = new(id, username, firstname, lastname, email, password, phone, admin, randompassword);
                    }
                    reader.Close();
                }
                catch (SqlException sqlExp)
                {
                    Console.WriteLine("Database error" + sqlExp.Message);
                    throw sqlExp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("General error" + ex.Message);
                    throw ex;
                }
            }
            return user;
        }

        public User? VerifyUser(string UserName, string Password)
        {
            List<User> users = GetAllUsers();
            foreach (User user in users)
            {
                if (user.Username == UserName && user.Password == Password)
                {
                    return user;
                }
            }
            return null;
        }

        public void LogOut(HttpContext context)
        {
            context.Session.Remove("Username");
            context.Session.Remove("Password");
        }

        public bool AdminVerify(string username, string password)
        {
            User userToVerify = VerifyUser(username, password);
            if (userToVerify != null)
            {
                return userToVerify.Administrator;
            }
            return false;
        }

        public string RandomPassword()
        {
            string password = "";
            var random = new Random();
            string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            for (int i = 0; i < 8; i++)
            {
                password += validChars[random.Next(0, validChars.Length-1)];
            }
            return password;
        }

        public List<User> GetUsersOnConditions(List<Predicate<User>> conditions, List<User> users)
        {
            if (users == null)
            {
                return new List<User>();
            }
            else if (conditions == null || conditions.Count == 0)
            {
                return users;
            }
            foreach (Predicate<User> condition in conditions)
            {
                if (condition == null)
                {
                    continue;
                }
                users = users.FindAll(condition);
                if (users.Count == 0)
                {
                    return users;
                }
            }
            return users;
        }

        public bool ValidatePhoneLength(string phoneLength)
        {
            if (string.IsNullOrEmpty(phoneLength))
            {
                return false;
            }
            string trimmed = phoneLength.Replace(" ", "");

            return trimmed.Length == 8;
        }

        public List<Event> GetAllEventBookingWithUserId(int userId)
        {
            List<Event> events = new List<Event>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(_getAllEventsWithUserIdSQL, connection);
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@UserID", userId);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        int eventID = reader.GetInt32("EventID");
                        string title = reader.GetString("Title");
                        string description = reader.GetString("Description");
                        bool cancelled = reader.GetBoolean("Cancelled");
                        TimeBetween eventTime = new TimeBetween(reader.GetDateTime("DateStart"), reader.GetDateTime("DateEnd"));
                        int cancellationThreshold = reader.GetInt32("CancellationThreshold");
                        Event @event = new Event(eventID, title, cancellationThreshold, description, eventTime, cancelled);
                        events.Add(@event);
                    }
                }

                catch (Exception ex)
                {


                }
                return events;
            }
        }


    }
}

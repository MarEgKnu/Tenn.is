using System.Data;
using Microsoft.Data.SqlClient;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Services
{
    public class UserService : Connection, IUserService
    {

        private string getAllSQL = "SELECT * FROM Users";
        private string getByIdSQL = "SELECT * FROM Users WHERE UserID = @UID";
        private string insertSQL = "INSERT INTO Users VALUES (@UID, @UNAME, @FNAME, @LNAME, @PWORD, @EMAIL, @PHONE, @ADMIN, @RWORD)";
        private string deleteSQL = "DELETE FROM Users WHERE UserID = @UID";
        private string editSQL = "UPDATE Users SET UserName = @UNAME, FirstName = @FNAME, LastName = @LNAME, Password = @PWORD, Phone = @PHONE, Email = @EMAIL, Administrator = @ADMIN, RandomPassword = @RWORD WHERE UserID = @UID";
        private string getAllLaneBookingsWithUserIdSQL = "Select * FROM lANEBOOKINGS WHERE UserID = @UID OR MateID = @UID";
        private string getByUserNameSQL = "SELECT * FROM Users WHERE UserName = @UserName";
        private string getUtilUsers = "SELECT * FROM Users\n" +
                                      "WHERE UserID < 1";
        private string getNonUtilUsers = "SELECT * FROM Users\n" +
                                         "WHERE UserID > 0";
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
                    SqlCommand command = new SqlCommand(getAllLaneBookingsWithUserIdSQL, conn);
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
                        SqlCommand command = new SqlCommand(insertSQL, conn);
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
                    SqlCommand command = new SqlCommand(deleteSQL, conn);
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
                        SqlCommand command = new SqlCommand(editSQL, conn);
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
                    SqlCommand command = new SqlCommand(getAllSQL, conn);
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
                        command = new SqlCommand(getUtilUsers, conn);
                    }
                    else
                    {
                        command = new SqlCommand(getNonUtilUsers, conn);
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
                    SqlCommand command = new SqlCommand(getByIdSQL, conn);
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
        public User GetUserByUserName(string userName)
        {
            User user = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(getByUserNameSQL, conn);
                    command.Parameters.AddWithValue("@UserName", userName);
                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string username = reader.GetString("UserName");
                        int userID = reader.GetInt32("UserID");
                        string firstname = reader.GetString("FirstName");
                        string lastname = reader.GetString("LastName");
                        string password = reader.GetString("Password");
                        string email = reader.GetString("Email");
                        string phone = reader.GetString("Phone");
                        bool admin = reader.GetBoolean("Administrator");
                        bool randompassword = reader.GetBoolean("RandomPassword");
                        user = new(userID, username, firstname, lastname, email, password, phone, admin, randompassword);
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
                password += validChars[random.Next(0, validChars.Length)];
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


    }
}

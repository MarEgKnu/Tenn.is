using System.Data;
using Microsoft.Data.SqlClient;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Services
{
    public class UserService : Connection, IUserService
    {

        private string getAllSQL = "SELECT * FROM Users";
        private string getByIdSQL = "SELECT * FROM Users WHERE UserID = '@UID'";
        private string insertSQL = "INSERT INTO Users VALUES ('@UID', @UNAME, @FNAME, @LNAME, @PWORD, @EMAIL, @PHONE, '@ADMIN'";
        private string deleteSQL = "DELETE FROM Users WHERE UserID = '@UID'";
        private string editSQL = "UPDATE Users SET UserName = @UNAME, FirstName = @FNAME, LastName = @LNAME, Password = @PWORD, Phone = @PHONE, Administrator = '@ADMIN'";
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
        public bool CreateUser(User user)
        {
            throw new NotImplementedException();
        }

        public bool DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public bool EditUser(User user, int id)
        {
            throw new NotImplementedException();
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
                        users.Add(new User(userID, username, firstname, lastname, email, password, phone, admin));
                    }
                    reader.Close();
                } catch (SqlException sqlExp) { 
                Console.WriteLine("Database error" + sqlExp.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("General error" +  ex.Message);
                }
            }
            return users;
        }

        public User GetUserById(int id)
        {
            throw new NotImplementedException();
        }

        public bool LogIn(string UserName, string Password)
        {
            throw new NotImplementedException();
        }

        public void LogOut(HttpContext context)
        {
            throw new NotImplementedException();
        }
    }
}

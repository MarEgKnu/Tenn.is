﻿using System.Data;
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

        public bool AdminVerify(int id, string password)
        {
            throw new NotImplementedException();
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
    }
}

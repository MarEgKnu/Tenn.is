using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Services
{
    public class UserService : Connection, IUserService
    {
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
            throw new NotImplementedException();
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

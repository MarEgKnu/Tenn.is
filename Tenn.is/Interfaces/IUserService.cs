namespace Tennis.Interfaces
{
    public interface IUserService
    {
        bool CreateUser(User user);

        bool DeleteUser(int id);

        bool EditUser(User user, int id);

        List<User> GetAllUsers();

        User GetUserById(int id);

        bool LogIn(string UserName, string Password);

        void LogOut(HttpContext context);
    }
}

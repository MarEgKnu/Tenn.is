using Tennis.Models;

namespace Tennis.Interfaces
{
    public interface IUserService
    {
        //User GetUserByUserName(string userName);
        bool CreateUser(User user);

        bool DeleteUser(int id);

        bool EditUser(User user, int id);

        List<User> GetAllUsers();
        /// <summary>
        /// Returns a list of all users within the database. If true, returns only utility users. If false, returns only personal users (SystemAdmin being neither)
        /// </summary>
        /// <param name="isUtilityUser"></param>
        /// <returns></returns>
        List<User> GetAllUsers(bool isUtilityUser);

        User GetUserById(int id);
        /// <summary>
        /// Checks whether the given username and password match any specific user within the database. Used to verify login.
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <returns>User object if password and username match a user, otherwise null.</returns>
        User? VerifyUser(string UserName, string Password);

        void LogOut(HttpContext context);
        /// <summary>
        /// Checks whether the given username and password match an administrator user.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>False if no user is found, or the found user isn't an administrator. Otherwise, true.</returns>
        bool AdminVerify(string username, string password);
        /// <summary>
        /// Generates a pseudo-random password.
        /// </summary>
        /// <returns>A random string of numbers, uppercase and lowercase letters, 8 characters in length.</returns>
        string RandomPassword();
        /// <summary>
        /// Filters a given list of users based on the given list of predicates. Used to ensure list adheres by multiple search requirements.
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="users"></param>
        /// <returns>A list of users, filtered by every given predicate</returns>
        public List<User> GetUsersOnConditions(List<Predicate<User>> conditions, List<User> users);
        /// <summary>
        /// Checks that a given phone number is 8 numbers long, ignoring spaces.
        /// </summary>
        /// <param name="phoneLength"></param>
        /// <returns>True if string is 8 characters long without spaces, false if not.</returns>
        public bool ValidatePhoneLength(string phoneLength);

        public List<UserLaneBooking> GetAllLaneBookingsWithUserId(int userId);

        public List<Event> GetAllEventBookingWithUserId(int userId);
    }
}

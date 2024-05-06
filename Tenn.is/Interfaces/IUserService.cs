﻿using Tennis.Models;

namespace Tennis.Interfaces
{
    public interface IUserService
    {
        bool CreateUser(User user);

        bool DeleteUser(int id);

        bool EditUser(User user, int id);

        List<User> GetAllUsers();

        User GetUserById(int id);

        User? VerifyUser(string UserName, string Password);

        void LogOut(HttpContext context);

        bool AdminVerify(string username, string password);

        string RandomPassword();

        public List<User> GetUsersOnConditions(List<Predicate<User>> conditions, List<User> users);

        public bool ValidatePhoneLength(string phoneLength);

        public List<UserLaneBooking> GetAllLaneBookingsWithUserId(int userId);
    }
}

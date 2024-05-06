﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tennis.Services;
using Tennis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Tennis.Interfaces;
using Microsoft.Data.SqlClient;

namespace Tennis.Services.Tests
{
    [TestClass()]
    public class UserServiceTests
    {

        [TestMethod()]
        public void GetAllUsersTest()
        {
            //Setup
            UserService userService = new UserService(true);
            //Execute
            List<User> users = userService.GetAllUsers();
            //Assert
            Assert.IsNotNull(users);
        }
        [TestMethod()]
        public void CreateUserTest_Valid()
        {
            //Setup
            User user = new User(101, "test1", "Test", "Testson", "test", "testestest", "1234", false, false);
            int before = 0;
            int after = 0;
            //Execution + cleanup
            UserService userService = new UserService(true);
            before = userService.GetAllUsers().Count();
            userService.CreateUser(user);
            after = userService.GetAllUsers().Count();
            userService.DeleteUser(user.UserId);
            //Assert
            Assert.AreEqual(before+1, after);
        }

        [TestMethod()]
        [ExpectedException(typeof(SqlException))]
        public void CreateUserTest_Invalid_IDAlreadyExists()
        {
            //Setup
            User user = new User(201, "test2", "Test", "Testson", "test", "testestest", "1234", false, false);
            UserService userService = new UserService(true);
            int before = 0;
            int after = 0;
            //Execution + cleanup
            try { 

            userService.CreateUser(user);
            before = userService.GetAllUsers().Count();
            userService.CreateUser(user);
                after = userService.GetAllUsers().Count();
            } catch (SqlException ex)
            {
                throw ex;
            } finally
            {
                userService.DeleteUser(user.UserId);
            }
                //Assert
                Assert.AreEqual(before, after);

        }
        [TestMethod()]
        [ExpectedException(typeof(SqlException))]
        public void CreateUserTest_Invalid_NameAlreadyExists()
        {
            //Setup
            User user = new User(201, "test3", "Test", "Testson", "test", "testestest", "1234", false, false);
            User sameuser = new User(301, "test3", "Test", "Testson", "test", "testestest", "1234", false, false);

            int before = 0;
            int after = 0;
            //Execution + cleanup
            UserService userService = new UserService(true);
            try
            {

                userService.CreateUser(user);
                before = userService.GetAllUsers().Count();
                userService.CreateUser(sameuser);
                after = userService.GetAllUsers().Count();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                userService.DeleteUser(user.UserId);
            }
            //Assert
            Assert.AreEqual(before, after);
        }
        [TestMethod()]
        public void CreateUserTest_Invalid_BadID()
        {
            //Setup
            User user = new User(0, "test4", "Test", "Testson", "test", "testestest", "1234", false, false);

            int before = 0;
            int after = 0;
            //Execution
            UserService userService = new UserService(true);
            before = userService.GetAllUsers().Count();
            userService.CreateUser(user);
            after = userService.GetAllUsers().Count();
            //Assert
            Assert.AreEqual(before, after);
        }

        [TestMethod()]
        public void GetUserByIdTest_Valid()
        {
            //Setup
            User originaluser = new User(102, "test5", "Test", "Testson", "test", "testestest", "1234", false, false);
            UserService service = new UserService(true);
            service.CreateUser(originaluser);

            //Execution + cleanup
            User newuser = service.GetUserById(originaluser.UserId);
            service.DeleteUser(originaluser.UserId);
            //Assert
            Assert.AreEqual(originaluser.Username, newuser.Username);
        }

        [TestMethod()]
        public void GetUserByIdTest_Invalid_NoUserfound()
        {
            //Setup
            User originaluser = new User(202, "test6", "Test", "Testson", "test", "testestest", "1234", false, false);
            UserService service = new UserService(true);
            service.CreateUser(originaluser);

            //Execution + cleanup
            User newuser = service.GetUserById(305);
            service.DeleteUser(originaluser.UserId);

            //Assert
            Assert.IsNull(newuser);
        }

        [TestMethod()]
        public void DeleteUserTest_Valid()
        {
            //Setup
            User user = new User(103, "test7", "Test", "Testson", "test", "testestest", "1234", false, false);

            int before = 0;
            int after = 0;

            UserService userService = new UserService(true);
            userService.CreateUser(user);
            before = userService.GetAllUsers().Count();
            //Execution
            userService.DeleteUser(user.UserId);
            after = userService.GetAllUsers().Count();
            //Assert
            Assert.AreEqual(before-1, after);
        }

        [TestMethod()]
        public void DeleteUserTest_Invalid_NoUserFound()
        {
            //Setup
            int before = 0;
            int after = 0;

            UserService userService = new UserService(true);
            before = userService.GetAllUsers().Count();
            //Execution
            userService.DeleteUser(404);
            after = userService.GetAllUsers().Count();
            //Assert
            Assert.AreEqual(before, after);
        }

        [TestMethod()]
        public void DeleteUserTest_Invalid_DeleteAdmin()
        {
            //Setup
            int before = 0;
            int after = 0;

            UserService userService = new UserService(true);
            before = userService.GetAllUsers().Count();
            //Execution
            userService.DeleteUser(0);
            after = userService.GetAllUsers().Count();
            //Assert
            Assert.AreEqual(before, after);
        }


        [TestMethod()]
        public void EditUserTest_Valid()
        {
            //Setup
            User user = new User(104, "test8", "Test", "Testson", "test", "testestest", "1234", false, false);


            UserService userService = new UserService(true);
            userService.CreateUser(user);

            //Execution + cleanup
            User newuser = new User(104, "test8", "Test", "Testson", "newmail", "testestest", "1234", false, false);

            userService.EditUser(newuser, user.UserId);


            //Assert
            Assert.AreEqual(userService.GetUserById(user.UserId).Email, newuser.Email);
            userService.DeleteUser(user.UserId);
        }
        [TestMethod()]
        public void EditUserTest_Invalid_IDNotFound()
        {
            //Setup
            User user = new User(204, "test9", "Test", "Testson", "test", "testestest", "1234", false, false);

            UserService userService = new UserService(true);
            userService.CreateUser(user);

            //Execution + cleanup
            User newuser = new User(210, "test", "Test", "Testson", "newmail", "testestest", "1234", false, false);

            userService.EditUser(newuser, newuser.UserId);

            //Assert
            Assert.AreNotEqual(userService.GetUserById(user.UserId).Email, newuser.Email);
            userService.DeleteUser(user.UserId);
        }

        [TestMethod()]
        public void RandomPasswordTest_Valid_IsNotNull()
        {
            //Setup
            UserService userService = new UserService(true);
            //Execution
            string password = userService.RandomPassword();

            //Assert
            Assert.IsNotNull(password);
        }

        [TestMethod()]
        public void RandomPasswordTest_Valid_CorrectLength()
        {
            //Setup
            UserService userService = new UserService(true);
            //Execution
            string password = userService.RandomPassword();

            //Assert
            Assert.AreEqual(8, password.Length);
        }

        [TestMethod()]
        public void ValidatePhoneLength_Valid_CorrectLength_Spaces()
        {
            UserService users = new UserService(true);
            string dummyNumber = "12 34 56 78";
            bool valid = users.ValidatePhoneLength(dummyNumber);

            Assert.IsTrue(valid);
        }

        [TestMethod()]
        public void ValidatePhoneLength_Valid_CorrectLength_NoSpaces()
        {
            UserService users = new UserService(true);
            string dummyNumber = "12345678";
            bool valid = users.ValidatePhoneLength(dummyNumber);

            Assert.IsTrue(valid);
        }

        [TestMethod()]
        public void ValidatePhoneLength_Invalid_TooLong()
        {
            UserService users = new UserService(true);
            string dummyNumber = "123456789";
            bool valid = users.ValidatePhoneLength(dummyNumber);

            Assert.IsFalse(valid);
        }

        [TestMethod()]
        public void ValidatePhoneLength_Invalid_TooShort()
        {
            UserService users = new UserService(true);
            string dummyNumber = "1234567";
            bool valid = users.ValidatePhoneLength(dummyNumber);

            Assert.IsFalse(valid);
        }

        [TestMethod()]
        public void ValidatePhoneLength_Invalid_PaddedWithSpaces()
        {
            UserService users = new UserService(true);
            string dummyNumber = "1 2 3 4 ";
            bool valid = users.ValidatePhoneLength(dummyNumber);

            Assert.IsFalse(valid);
        }

        [TestMethod()]
        public void ValidatePhoneLength_Invalid_Null()
        {
            UserService users = new UserService(true);
            string dummyNumber = null;
            bool valid = users.ValidatePhoneLength(dummyNumber);

            Assert.IsFalse(valid);
        }

        [TestMethod()]
        public void GetAllLaneBookingsWithUserId_Valide()
        {
            UserService users = new UserService(true);
            User user = new User(103, "test7", "Test", "Testson", "test", "testestest", "1234", false, false);
            LaneBookingService laneBookingService = new LaneBookingService();
            LaneService laneService = new LaneService();
            laneService.CreateLane(new Lane(250, true, true));

            //laneBookingService.CreateLaneBooking(new LaneBooking());
            users.GetAllLaneBookingsWithUserId(103);
            laneBookingService.CreateLaneBooking(new UserLaneBooking(1, 1, DateTime.Now.AddHours(2), 201, 101, false));

        }
    }
}
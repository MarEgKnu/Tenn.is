using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            User user = new User(101, "test", "Test", "Testson", "test", "testestest", "1234", false, false);
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
        public void CreateUserTest_Invalid_AlreadyExists()
        {
            //Setup
            User user = new User(201, "test", "Test", "Testson", "test", "testestest", "1234", false, false);

            int before = 0;
            int after = 0;
            //Execution + cleanup
            UserService userService = new UserService(true);
            userService.CreateUser(user);
            before = userService.GetAllUsers().Count();
            userService.CreateUser(user);
            after = userService.GetAllUsers().Count();
            userService.DeleteUser(user.UserId);
            //Assert
            Assert.AreEqual(before, after);
        }
        [TestMethod()]
        public void CreateUserTest_Invalid_BadID()
        {
            //Setup
            User user = new User(0, "test", "Test", "Testson", "test", "testestest", "1234", false, false);

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
            User originaluser = new User(102, "test", "Test", "Testson", "test", "testestest", "1234", false, false);
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
            User originaluser = new User(202, "test", "Test", "Testson", "test", "testestest", "1234", false, false);
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
            User user = new User(103, "test", "Test", "Testson", "test", "testestest", "1234", false, false);

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
            User user = new User(104, "test", "Test", "Testson", "test", "testestest", "1234", false, false);


            UserService userService = new UserService(true);
            userService.CreateUser(user);

            //Execution + cleanup
            User newuser = new User(104, "test", "Test", "Testson", "newmail", "testestest", "1234", false, false);

            userService.EditUser(newuser, user.UserId);


            //Assert
            Assert.AreEqual(userService.GetUserById(user.UserId).Email, newuser.Email);
            userService.DeleteUser(user.UserId);
        }
        [TestMethod()]
        public void EditUserTest_Invalid_IDNotFound()
        {
            //Setup
            User user = new User(204, "test", "Test", "Testson", "test", "testestest", "1234", false, false);

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


        //[TestMethod()]
        //public void LogInTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void LogOutTest()
        //{
        //    Assert.Fail();
        //}
    }
}
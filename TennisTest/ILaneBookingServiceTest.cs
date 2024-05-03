using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tennis.Helpers;
using Tennis.Models;
using Tennis.Services;

namespace TennisTest
{
    [TestClass]
    public class ILaneBookingServiceTest
    {
        public LaneBookingService laneBookingService { get; set; }
        public LaneService laneService { get; set; }
        public UserService userService { get; set; }

        void TestSetUp()
        {
            laneBookingService = new LaneBookingService(true);
            laneService = new LaneService(true);
            userService = new UserService(true);
            CleanTest();
        }

        void CreateLaneBooking()
        {
            TimeBetween timeBetween = new TimeBetween(DateTime.Now.AddHours(2), DateTime.Now.AddHours(3));
            laneService.CreateLane(new Lane(1, true, false));
            userService.CreateUser(new User(201, "test2", "Test", "Testson", "test", "testestest", "1234", false, false));
            userService.CreateUser(new User(101, "test1", "Test", "Testson", "test", "testestest", "1234", false, false));
            laneBookingService.CreateLaneBooking(new UserLaneBooking(1, 1, timeBetween, 201, 101, false));
        }
        void CleanTest()
        {
            laneBookingService.DeleteLaneBooking(1);
            laneService.DeleteLane(1);
            userService.DeleteUser(201);
            userService.DeleteUser(101);
        }

        [TestMethod]
        public void CreateLaneBookingUserTestAcceptable()
        {
            TestSetUp();
            TimeBetween timeBetween = new TimeBetween(DateTime.Now.AddHours(2), DateTime.Now.AddHours(3));
            laneService.CreateLane(new Lane(1, true, false));
            userService.CreateUser(new User(201, "test2", "Test", "Testson", "test", "testestest", "1234", false, false));
            userService.CreateUser(new User(101, "test1", "Test", "Testson", "test", "testestest", "1234", false, false));
            bool Testresult = laneBookingService.CreateLaneBooking(new UserLaneBooking(1, 1, timeBetween, 201, 101, false));
            CleanTest();
            Assert.IsTrue(Testresult);
        }

        [TestMethod]
        public void CreateLaneBookingUserTestUnaccesptableValues()
        {
            TestSetUp();
            TimeBetween timeBetween = new TimeBetween(DateTime.Now.AddHours(2), DateTime.Now.AddHours(3));
            laneService.CreateLane(new Lane(1, true, false));
            userService.CreateUser(new User(101, "test1", "Test", "Testson", "test", "testestest", "1234", false, false));
            bool Testresult = laneBookingService.CreateLaneBooking(new UserLaneBooking(1,1,  timeBetween, 101, 101, false));
            CleanTest();
            Assert.IsFalse(Testresult);
        }


        [TestMethod]
        public void CreateLaneBookingUserTest2TimesTheSame()
        {
            TestSetUp();
            TimeBetween timeBetween = new TimeBetween(DateTime.Now.AddHours(2), DateTime.Now.AddHours(3));
            laneService.CreateLane(new Lane(1, true, false));
            userService.CreateUser(new User(201, "test2", "Test", "Testson", "test", "testestest", "1234", false, false));
            userService.CreateUser(new User(101, "test1", "Test", "Testson", "test", "testestest", "1234", false, false));
            laneBookingService.CreateLaneBooking(new UserLaneBooking(1, 1, timeBetween, 201, 101, false));
            bool Testresult = laneBookingService.CreateLaneBooking(new UserLaneBooking(1, 1, timeBetween, 201, 101, false));
            CleanTest();
            Assert.IsFalse(Testresult);
        }

        [TestMethod]
        public void DeleteLaneBookingTestExisting()
        {
            TestSetUp();
            TimeBetween timeBetween = new TimeBetween(DateTime.Now.AddHours(2), DateTime.Now.AddHours(3));
            laneService.CreateLane(new Lane(1, true, false));
            userService.CreateUser(new User(201, "test2", "Test", "Testson", "test", "testestest", "1234", false, false));
            userService.CreateUser(new User(101, "test1", "Test", "Testson", "test", "testestest", "1234", false, false));
            laneBookingService.CreateLaneBooking(new UserLaneBooking(1, 1, timeBetween, 201, 101, false));
            int Id = laneBookingService.GetAllLaneBookings<UserLaneBooking>().First().BookingID;
            bool Testresult = laneBookingService.DeleteLaneBooking(Id);
            CleanTest();
            Assert.IsTrue(Testresult);
        }

        [TestMethod]
        public void DeleteLaneBookingTestInexisting()
        {
            TestSetUp();
            bool Testresult = laneService.DeleteLane(199);
            Assert.IsFalse(Testresult);
        }

        [TestMethod]
        public void EditLaneBookingTestExistingAndAcceptable()
        {
            TestSetUp();

            TimeBetween timeBetween = new TimeBetween(DateTime.Now.AddHours(2), DateTime.Now.AddHours(3));
            laneService.CreateLane(new Lane(1, true, false));
            userService.CreateUser(new User(201, "test2", "Test", "Testson", "test", "testestest", "1234", false, false));
            userService.CreateUser(new User(101, "test1", "Test", "Testson", "test", "testestest", "1234", false, false));
            laneBookingService.CreateLaneBooking(new UserLaneBooking(1, 1, timeBetween, 201, 101, false));
            bool Testresult = laneBookingService.EditLaneBooking(new UserLaneBooking(1, 1, timeBetween, 201, 101, false), 1);
            CleanTest();
        }

        [TestMethod]
        public void EditLaneBookingTestInexistingBooking()
        {
            TestSetUp();
            TimeBetween timeBetween = new TimeBetween(DateTime.Now.AddHours(2), DateTime.Now.AddHours(3));
            bool Testresult = laneBookingService.EditLaneBooking(new UserLaneBooking(1, 1, timeBetween, 201, 101, false), 1);
            Assert.IsFalse(Testresult);
        }


        [TestMethod]
        public void EditLaneBookingTestExistingAndUnacceptableValues()
        {
            TestSetUp();
            TimeBetween timeBetween = new TimeBetween(DateTime.Now.AddHours(2), DateTime.Now.AddHours(3));
            laneService.CreateLane(new Lane(1, true, false));
            userService.CreateUser(new User(201, "test2", "Test", "Testson", "test", "testestest", "1234", false, false));
            userService.CreateUser(new User(101, "test1", "Test", "Testson", "test", "testestest", "1234", false, false));
            laneBookingService.CreateLaneBooking(new UserLaneBooking(1, 1, timeBetween, 201, 101,  false));
            laneBookingService.CreateLaneBooking(new UserLaneBooking(1, 2, timeBetween, 201, 101, false));
            bool Testresult = laneBookingService.EditLaneBooking(new UserLaneBooking(1, 2, timeBetween, 101, 101, false), 2);
            laneBookingService.DeleteLaneBooking(2);
            CleanTest();
            Assert.IsFalse(Testresult);
        }

        [TestMethod]
        public void GetAllLaneBookingsTest()
        {
            TestSetUp();
            int NumberBefore = laneBookingService.GetAllLaneBookings<UserLaneBooking>().Count;
            CreateLaneBooking();
            int NumberAfter = laneBookingService.GetAllLaneBookings<UserLaneBooking>().Count;
            CleanTest();
            Assert.AreEqual(NumberBefore +1, NumberAfter);
        }

        [TestMethod]
        public void GetLaneBookingByIdTestExisting()
        {
            TestSetUp();
            CreateLaneBooking();
            bool Testresult = false;
            int Id = laneBookingService.GetAllLaneBookings<UserLaneBooking>().First().BookingID;
            if (laneBookingService.GetUserLaneBookingById(Id) != null)
                Testresult = true;
            CleanTest();
            Assert.IsTrue(Testresult);
        }

        [TestMethod]
        public void GetLaneBookingByIdTestInexisting()
        {
            TestSetUp();
            Assert.IsNull(laneBookingService.GetUserLaneBookingById(199));
        }

        [TestMethod]
        public void CancelLaneBonkingTest()
        {
            TestSetUp();
            CreateLaneBooking();
            int Id = laneBookingService.GetAllLaneBookings<UserLaneBooking>().First().BookingID;
            bool Testresult = laneBookingService.CancelLaneBonking(Id);
            CleanTest();
            Assert.IsTrue(Testresult);
        }
    }
}

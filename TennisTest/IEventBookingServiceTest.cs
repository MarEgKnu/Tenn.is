using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tennis.Exceptions;
using Tennis.Helpers;
using Tennis.Interfaces;
using Tennis.Models;
using Tennis.Services;

namespace TennisTest
{
    [TestClass]
    public class IEventBookingServiceTest
    {
        private IEventBookingService bookingService;
        private IEventService eventService;
        private IUserService userService;

        void TestSetUp()
        {
            eventService = new EventService(true);
            userService = new UserService(true);
            bookingService = new EventBookingService(true, eventService, userService);
            
            using (SqlConnection conn = new SqlConnection(Secret.ConnectionStringTest))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM EventBookings", conn);
                SqlCommand cmd2 = new SqlCommand("DELETE FROM Events", conn);
                SqlCommand cmd3 = new SqlCommand("DELETE FROM Users", conn);

                cmd.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
                cmd3.ExecuteNonQuery();
            }
        }
        [TestMethod]
        public void GetAllEventBookings_0Bookings()
        {

            TestSetUp();
            int count = bookingService.GetAllEventBookings().Count;
            Assert.AreEqual(0, count);

        }
        [TestMethod]
        public void GetAllEventBookings_1Event2Users2Bookings()
        {
            TestSetUp();
            TimeBetween time = new TimeBetween(new DateTime(2040, 10, 20), new DateTime(2040, 10, 30));
            bool sucess = eventService.CreateEvent(new Event(1, "big event", 0, "meget stor event", time));
            Event evt = eventService.GetAllEvents().Last();

            userService.CreateUser(new User(1, "KlausXxX", "Klaus", "Nielsen", "xxxx@gmail.com", "ZxKjdJF!!!45", "12345678", false, false));
           
            userService.CreateUser(new User(2,"KlausXxYx", "Klaus", "Nielsen", "xxxx@gmail.com", "ZxKjdJF!!!45", "12345678", false, false));
            User user = userService.GetAllUsers()[0];
            User user2 = userService.GetAllUsers()[1];
            EventBooking booking = new EventBooking(evt, user, "Ingen cola");
            EventBooking booking2 = new EventBooking(evt, user2, "Ingen cola 2");
            int beforeCreate = bookingService.GetAllEventBookings().Count;
            bookingService.CreateEventBooking(booking);
            bookingService.CreateEventBooking(booking2);
            int afterCreate = bookingService.GetAllEventBookings().Count;
            Assert.AreEqual(beforeCreate + 2, afterCreate);

        }


        [TestMethod]
        public void CreateEventBooking_Suceed()
        {
            TestSetUp();
            TimeBetween time = new TimeBetween(new DateTime(2040, 10, 20 ), new DateTime(2040, 10, 30));
            bool sucess = eventService.CreateEvent(new Event(1, "big event", 0, "meget stor event", time));
            Event evt = eventService.GetAllEvents().Last();

            userService.CreateUser(new User(1, "KlausXxX", "Klaus", "Nielsen", "xxxx@gmail.com", "ZxKjdJF!!!45", "12345678", false, false));
            User user = userService.GetAllUsers().First();
            EventBooking booking = new EventBooking(evt, user, "Ingen cola");
            int beforeCreate = bookingService.GetAllEventBookings().Count;
            bookingService.CreateEventBooking(booking);
            int afterCreate = bookingService.GetAllEventBookings().Count;
            booking = bookingService.GetAllEventBookings().First();
            Assert.AreEqual(beforeCreate + 1, afterCreate);
            Assert.AreEqual("KlausXxX", booking.User.Username);
            Assert.AreEqual("Ingen cola", booking.Comment);

        }
        [ExpectedException(typeof(InvalidTimeException))]
        [TestMethod]
        public void CreateEventBooking_Fail_EventIsInPast()
        {
            TestSetUp();
            TimeBetween time = new TimeBetween(new DateTime(2020, 10, 20), new DateTime(2020, 10, 30));
            bool sucess = eventService.CreateEventNoRequirements(new Event(1, "big event", 0, "meget stor event", time));
            Event evt = eventService.GetAllEvents().Last();

            userService.CreateUser(new User(1, "KlausXxX", "Klaus", "Nielsen", "xxxx@gmail.com", "ZxKjdJF!!!45", "12345678", false, false));
            User user = userService.GetAllUsers().First();
            EventBooking booking = new EventBooking(evt, user, "Ingen cola");
            bookingService.CreateEventBooking(booking);
        }

        [ExpectedException(typeof(DuplicateBookingException))]
        [TestMethod]
        public void CreateEventBooking_Fail_UserBooksToTheSameEventTwice()
        {
            TestSetUp();
            TimeBetween time = new TimeBetween(new DateTime(2040, 10, 20), new DateTime(2040, 10, 30));
            bool sucess = eventService.CreateEvent(new Event(1, "big event", 0, "meget stor event", time));
            Event evt = eventService.GetAllEvents().Last();

            userService.CreateUser(new User(1, "KlausXxX", "Klaus", "Nielsen", "xxxx@gmail.com", "ZxKjdJF!!!45", "12345678", false, false));
            User user = userService.GetAllUsers().First();
            EventBooking booking = new EventBooking(evt, user, "Ingen cola");
            EventBooking booking2 = new EventBooking(evt, user, "Ingen cola virkelig");
            
            bool sucess1 = bookingService.CreateEventBooking(booking);
            bool sucess2 = bookingService.CreateEventBooking(booking2);
            int count = bookingService.GetAllEventBookings().Count;

            Assert.AreEqual(1, count);
            Assert.IsTrue(sucess1);
            Assert.IsFalse(sucess2);
        }
        [ExpectedException(typeof(EventNotFoundException))]
        [TestMethod]
        public void CreateEventBooking_Fail_EventNotFound()
        {
            TestSetUp();
            TimeBetween time = new TimeBetween(new DateTime(2040, 10, 20), new DateTime(2040, 10, 30));
            bool sucess = eventService.CreateEvent(new Event(1, "big event", 0, "meget stor event", time));
            Event evt = eventService.GetAllEvents().Last();
            evt.EventID = evt.EventID + 1;

            userService.CreateUser(new User(1, "KlausXxX", "Klaus", "Nielsen", "xxxx@gmail.com", "ZxKjdJF!!!45", "12345678", false, false));
            User user = userService.GetAllUsers().First();
            EventBooking booking = new EventBooking(evt, user, "Ingen cola");

            bookingService.CreateEventBooking(booking);
        }
        [ExpectedException(typeof(UserNotFoundException))]
        [TestMethod]
        public void CreateEventBooking_Fail_UserNotFound()
        {
            TestSetUp();
            TimeBetween time = new TimeBetween(new DateTime(2040, 10, 20), new DateTime(2040, 10, 30));
            bool sucess = eventService.CreateEvent(new Event(1, "big event", 0, "meget stor event", time));
            Event evt = eventService.GetAllEvents().Last();

            userService.CreateUser(new User(1, "KlausXxX", "Klaus", "Nielsen", "xxxx@gmail.com", "ZxKjdJF!!!45", "12345678", false, false));
            User user = userService.GetAllUsers().First();
            user.UserId = user.UserId + 1;
            EventBooking booking = new EventBooking(evt, user, "Ingen cola");

            bookingService.CreateEventBooking(booking);
        }
        [TestMethod]
        public void CreateEventBooking_Fail_ArgumentNull()
        {
            TestSetUp();
            TimeBetween time = new TimeBetween(new DateTime(2040, 10, 20), new DateTime(2040, 10, 30));
            eventService.CreateEvent(new Event(1, "big event", 0, "meget stor event", time));
            Event evt = eventService.GetAllEvents().Last();

            userService.CreateUser(new User(1, "KlausXxX", "Klaus", "Nielsen", "xxxx@gmail.com", "ZxKjdJF!!!45", "12345678", false, false));
            User user = userService.GetAllUsers().First();
            EventBooking booking = new EventBooking(evt, user, "Ingen cola");

            bool sucess1 = bookingService.CreateEventBooking(null);
            Assert.IsFalse(sucess1);
            Assert.AreEqual(0,bookingService.GetAllEventBookings().Count());

        }
        [TestMethod]
        public void CreateEventBooking_Fail_EventCancelled()
        {
            TestSetUp();
            TimeBetween time = new TimeBetween(new DateTime(2040, 10, 20), new DateTime(2040, 10, 30));
            eventService.CreateEvent(new Event(1, "big event", 0, "meget stor event", time, true));
            Event evt = eventService.GetAllEvents().Last();

            userService.CreateUser(new User(1, "KlausXxX", "Klaus", "Nielsen", "xxxx@gmail.com", "ZxKjdJF!!!45", "12345678", false, false));
            User user = userService.GetAllUsers().First();
            EventBooking booking = new EventBooking(evt, user, "Ingen cola");

            bool sucess1 = bookingService.CreateEventBooking(booking);
            Assert.IsFalse(sucess1);
            Assert.AreEqual(0, bookingService.GetAllEventBookings().Count());

        }


        [TestMethod]
        public void GetAllBookingsOnEvent_2BookingsOn1Event()
        {
            TestSetUp();
            TimeBetween time = new TimeBetween(new DateTime(2040, 10, 20), new DateTime(2040, 10, 30));
            eventService.CreateEvent(new Event(1, "big event", 0, "meget stor event", time));
            eventService.CreateEvent(new Event(2, "big eventee", 0, "meget stor event", time));
            Event evt1 = eventService.GetAllEvents()[0];
            Event evt2 = eventService.GetAllEvents()[1];

            userService.CreateUser(new User(1, "KlausXxX", "Klaus", "Nielsen", "xxxx@gmail.com", "ZxKjdJF!!!45", "12345678", false, false));

            userService.CreateUser(new User(2, "KlausXxYx", "Klaus", "Nielsen", "xxxx@gmail.com", "ZxKjdJF!!!45", "12345678", false, false));
            User user = userService.GetAllUsers()[0];
            User user2 = userService.GetAllUsers()[1];
            EventBooking booking = new EventBooking(evt1, user, "Ingen cola");
            EventBooking booking2 = new EventBooking(evt1, user2, "Ingen cola 2");
            EventBooking booking3 = new EventBooking(evt2, user2, "Ingen cola 2");
            bookingService.CreateEventBooking(booking);
            bookingService.CreateEventBooking(booking2);
            bookingService.CreateEventBooking(booking3);
            List<EventBooking> bookings = bookingService.GetAllBookingsByEventID(evt1.EventID);
            Assert.AreEqual(2, bookings.Count);
            Assert.AreEqual(evt1.EventID, bookings[0].Event.EventID);
            Assert.AreEqual(evt1.EventID, bookings[1].Event.EventID);
            Assert.AreEqual(evt1.Title, bookings[1].Event.Title);
        }
        [TestMethod]
        public void GetAllBookingsOnEvent_NoBookingsFound()
        {
            TestSetUp();
            TimeBetween time = new TimeBetween(new DateTime(2040, 10, 20), new DateTime(2040, 10, 30));
            eventService.CreateEvent(new Event(1, "big event", 0, "meget stor event", time));
            eventService.CreateEvent(new Event(2, "big eventee", 0, "meget stor event", time));
            Event evt1 = eventService.GetAllEvents()[0];
            Event evt2 = eventService.GetAllEvents()[1];

            userService.CreateUser(new User(1, "KlausXxX", "Klaus", "Nielsen", "xxxx@gmail.com", "ZxKjdJF!!!45", "12345678", false, false));

            userService.CreateUser(new User(2, "KlausXxYx", "Klaus", "Nielsen", "xxxx@gmail.com", "ZxKjdJF!!!45", "12345678", false, false));
            User user = userService.GetAllUsers()[0];
            User user2 = userService.GetAllUsers()[1];
            EventBooking booking = new EventBooking(evt1, user, "Ingen cola");
            EventBooking booking2 = new EventBooking(evt1, user2, "Ingen cola 2");
            EventBooking booking3 = new EventBooking(evt2, user2, "Ingen cola 2");
            bookingService.CreateEventBooking(booking);
            bookingService.CreateEventBooking(booking2);
            bookingService.CreateEventBooking(booking3);
            List<EventBooking> bookings = bookingService.GetAllBookingsByEventID(evt2.EventID + 2);
            Assert.AreEqual(0, bookings.Count);
        }

        [TestMethod]
        public void AlreadyHasBooking_DosentHaveAnyBooking()
        {
            TestSetUp();
            TimeBetween time = new TimeBetween(new DateTime(2040, 10, 20), new DateTime(2040, 10, 30));
            eventService.CreateEvent(new Event(1, "big event", 0, "meget stor event", time));
  
            Event evt1 = eventService.GetAllEvents()[0];

            userService.CreateUser(new User(1, "KlausXxX", "Klaus", "Nielsen", "xxxx@gmail.com", "ZxKjdJF!!!45", "12345678", false, false));
            User user = userService.GetAllUsers()[0];
            EventBooking? hasBooking = bookingService.AlreadyHasEventBooking(user.UserId, evt1.EventID);
         
            Assert.IsNull(hasBooking);
        }
        [TestMethod]
        public void AlreadyHasBooking_HasBookingButWrongBooking()
        {
            TestSetUp();
            TimeBetween time = new TimeBetween(new DateTime(2040, 10, 20), new DateTime(2040, 10, 30));
            eventService.CreateEvent(new Event(1, "big event", 0, "meget stor event", time));

            Event evt1 = eventService.GetAllEvents()[0];

            userService.CreateUser(new User(1, "KlausXxX", "Klaus", "Nielsen", "xxxx@gmail.com", "ZxKjdJF!!!45", "12345678", false, false));
            
            User user = userService.GetAllUsers()[0];
            EventBooking booking = new EventBooking(evt1, user, "Ingen cola");
            bookingService.CreateEventBooking(booking);
            EventBooking? hasBooking = bookingService.AlreadyHasEventBooking(user.UserId, evt1.EventID + 2);

            Assert.IsNull(hasBooking);
        }
        [TestMethod]
        public void AlreadyHasBooking_HasBooking()
        {
            TestSetUp();
            TimeBetween time = new TimeBetween(new DateTime(2040, 10, 20), new DateTime(2040, 10, 30));
            eventService.CreateEvent(new Event(1, "big event", 0, "meget stor event", time));

            Event evt1 = eventService.GetAllEvents()[0];

            userService.CreateUser(new User(1, "KlausXxX", "Klaus", "Nielsen", "xxxx@gmail.com", "ZxKjdJF!!!45", "12345678", false, false));

            User user = userService.GetAllUsers()[0];
            EventBooking booking = new EventBooking(evt1, user, "Ingen cola");
            bookingService.CreateEventBooking(booking);
            EventBooking? hasBooking = bookingService.AlreadyHasEventBooking(user.UserId, evt1.EventID);

            Assert.IsNotNull(hasBooking);
            Assert.AreEqual("KlausXxX", hasBooking.User.Username);
            Assert.AreEqual("big event", hasBooking.Event.Title);
            Assert.AreEqual(new DateTime(2040, 10, 20), hasBooking.Event.EventTime.StartTime);
        }
    }
    
}

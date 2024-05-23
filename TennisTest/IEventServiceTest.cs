using Microsoft.AspNetCore.Mvc.Filters;
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
    public class IEventServiceTest
    {
        private IEventService eventService;

        void TestSetUp()
        {
            eventService = new EventService(true);
            using (SqlConnection conn = new SqlConnection(Secret.ConnectionStringTest))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Events", conn);

                cmd.ExecuteNonQuery();
            }
        }

        [TestMethod]
        public void CreateEventTest_Sucess()
        {
            TestSetUp();
            List<Event> oldEvents = eventService.GetAllEvents();
            Event newEvent = new Event(1, "epic event", 0, "big event",new TimeBetween(new DateTime(2030,12,5), new DateTime(2030, 12, 6)));
            bool sucess = eventService.CreateEvent(newEvent);
            List<Event> newEvents = eventService.GetAllEvents();
            Assert.AreEqual(oldEvents.Count + 1, newEvents.Count);
            Assert.IsTrue(sucess);
            Assert.AreEqual("epic event", newEvents[0].Title);
            Assert.AreEqual("big event", newEvents[0].Description);
        }
        [TestMethod]
        public void CreateEventTest_Fail_Null()
        {
            TestSetUp();
    
            bool sucess = eventService.CreateEvent(null);
            Assert.IsFalse(sucess);
           
        }
        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void CreateEventTest_Fail_StartTimeBiggerThanEndTime()
        {
            TestSetUp();
            Event newEvent = new Event(1, "epic event", 0, "big event", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 5)));
            bool sucess = eventService.CreateEvent(newEvent);
        }
        [ExpectedException(typeof(InvalidTimeException))]
        [TestMethod]
        public void CreateEventTest_Fail_EventIsInThePast()
        {
            TestSetUp();
            Event newEvent = new Event(1, "epic event", 0, "big event", new TimeBetween(new DateTime(2022, 12, 6), new DateTime(2022, 12, 7)));
            bool sucess = eventService.CreateEvent(newEvent);
        }

        [TestMethod]
        public void GetAllEvents_0Events()
        {
            TestSetUp();
            List<Event> events = eventService.GetAllEvents();
            Assert.AreEqual(0, events.Count);
        }

        [TestMethod]
        public void GetAllEvents_3Events()
        {
            TestSetUp();
            List<Event> oldEvents = eventService.GetAllEvents();
            Event newEvent = new Event(1, "epic event", 0, "big event", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            Event newEvent2 = new Event(2, "epic event", 0, "big event", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            Event newEvent3 = new Event(3, "epic event", 0, "big event", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            eventService.CreateEvent(newEvent);
            eventService.CreateEvent(newEvent2);
            eventService.CreateEvent(newEvent3);
            List<Event> newEvents = eventService.GetAllEvents();
            Assert.AreEqual(oldEvents.Count + 3, newEvents.Count);
        }

        [TestMethod]
        public void DeleteEventTest_Sucess()
        {
            TestSetUp();
            Event newEvent = new Event(1, "epic event", 0, "big event", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            eventService.CreateEvent(newEvent);
            int oldEventsCount = eventService.GetAllEvents().Count;
            newEvent = eventService.GetAllEvents()[0];
            bool sucess = eventService.DeleteEvent(newEvent.EventID);
            int newEventsCount = eventService.GetAllEvents().Count;
            Assert.AreEqual(oldEventsCount-1, newEventsCount);
            Assert.IsTrue(sucess);
        }

        [TestMethod]
        public void DeleteEventTest_Fail_EventNotExist()
        {
            TestSetUp();
            Event newEvent = new Event(1, "epic event", 0, "big event", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            eventService.CreateEvent(newEvent);
            int oldEventsCount = eventService.GetAllEvents().Count;
            bool sucess = eventService.DeleteEvent(999999);
            int newEventsCount = eventService.GetAllEvents().Count;
            Assert.AreEqual(oldEventsCount, newEventsCount);
            Assert.IsFalse(sucess);
        }
        [TestMethod]
        public void GetEventByIdTest_Sucess()
        {
            TestSetUp();
            Event newEvent = new Event(1, "epic evente", 0, "big evente", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            eventService.CreateEvent(newEvent);
            List<Event> events = eventService.GetAllEvents();
            Event grabbedEvent = eventService.GetEventByNumber(events[0].EventID);
            Assert.AreEqual(newEvent.Title, grabbedEvent.Title);
            Assert.AreEqual(newEvent.Description, grabbedEvent.Description);
        }
        [TestMethod]
        public void GetEventByIdTest_Fail_NoEventFound()
        {
            TestSetUp();
            Event newEvent = new Event(1, "epic evente", 0, "big evente", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            eventService.CreateEvent(newEvent);
            List<Event> events = eventService.GetAllEvents();
            Event grabbedEvent = eventService.GetEventByNumber(999999);
            Assert.IsNull(grabbedEvent);
         
        }






        [TestMethod]
        public void EditEventTest_Sucess()
        {
            TestSetUp();
            Event newEvent = new Event(1, "epic event", 0, "big event", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            Event editedEvent = new Event(2, "big event", 0, "epic event", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            eventService.CreateEvent(newEvent);
            List<Event> events = eventService.GetAllEvents();
            bool sucess = eventService.EditEvent(editedEvent, events[0].EventID);
            Event afterEdit = eventService.GetEventByNumber(events[0].EventID);
            Assert.AreEqual("big event", afterEdit.Title);
            Assert.AreEqual("epic event", afterEdit.Description);
            Assert.AreEqual(false, afterEdit.Cancelled);
            Assert.AreEqual(events[0].EventID, afterEdit.EventID);
            Assert.IsTrue(sucess);
        }
        [TestMethod]
        public void EditEventTest_Fail_Null()
        {
            TestSetUp();
            Event newEvent = new Event(1, "epic event", 30, "big event", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            eventService.CreateEvent(newEvent);
            Event eventInDB = eventService.GetAllEvents().First();
            bool sucess = eventService.EditEvent(null, eventInDB.EventID);
            eventInDB = eventService.GetAllEvents().First();
            Assert.IsFalse(sucess);
            Assert.AreEqual("epic event", eventInDB.Title);
            Assert.AreEqual("big event", eventInDB.Description);
            Assert.AreEqual(30, eventInDB.CancellationThresholdMinutes);
            Assert.AreEqual(false, eventInDB.Cancelled);


        }
        [TestMethod]
        public void EditEventTest_Fail_CantFindEventFromID()
        {
            TestSetUp();
            Event editedEvent = new Event(1, "epic eventeee", 30, "big eventeee", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            bool sucess = eventService.EditEvent(editedEvent, int.MaxValue);
            Assert.IsFalse(sucess);
        }


        [TestMethod]
        public void EditEventTest_Sucess_EventIDCantBeChanged()
        {
            TestSetUp();
            Event newEvent = new Event(1, "epic event", 0, "big event", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            Event editedEvent = new Event(2, "epic event", 0, "big event", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            eventService.CreateEvent(newEvent);
            List<Event> events = eventService.GetAllEvents();
            bool sucess = eventService.EditEvent(editedEvent, events[0].EventID);
            Event afterEdit = eventService.GetEventByNumber(events[0].EventID);
            Assert.AreEqual(newEvent.Title, afterEdit.Title);
            Assert.AreEqual(newEvent.Description, afterEdit.Description);
            Assert.AreEqual(events[0].EventID, afterEdit.EventID);
            Assert.IsTrue(sucess);
        }


        [ExpectedException(typeof(InvalidTimeException))]
        [TestMethod]
        public void EditEventTest_Fail_ChangeTimeToPast()
        {
            TestSetUp();
            Event newEvent = new Event(1, "epic event", 0, "big event", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            Event editedEvent = new Event(2, "epic event", 0, "big event", new TimeBetween(new DateTime(2022, 12, 6), new DateTime(2022, 12, 7)));
            eventService.CreateEvent(newEvent);
            List<Event> events = eventService.GetAllEvents();
            bool sucess = eventService.EditEvent(editedEvent, events[0].EventID);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void EditEventTest_Fail_StartDateBiggerThanEndDate()
        {
            TestSetUp();
            Event newEvent = new Event(1, "epic event", 0, "big event", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            Event editedEvent = new Event(2, "epic event", 0, "big event", new TimeBetween(new DateTime(2022, 12, 7), new DateTime(2022, 12, 6)));
            eventService.CreateEvent(newEvent);
            List<Event> events = eventService.GetAllEvents();
            bool sucess = eventService.EditEvent(editedEvent, events[0].EventID);
        }


        [TestMethod]
        public void GetEventsByConditions_Fail_ConditionNull()
        {
            TestSetUp();

            List<Predicate<Event>> conditions = new List<Predicate<Event>>();
            conditions.Add(null);

            Event newEvent = new Event(1, "epic event", 0, "big event", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            Event newEvent2 = new Event(2, "epic event", 0, "big eveneet", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            Event newEvent3 = new Event(2, "not epic event", 0, "big eveneet", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            eventService.CreateEvent(newEvent);
            eventService.CreateEvent(newEvent2);
            eventService.CreateEvent(newEvent3);
            List<Event> events = eventService.GetAllEvents();
            events = FilterHelpers.GetItemsOnConditions(conditions, events);

            Assert.AreEqual(3, events.Count);
        }


        [TestMethod]
        public void GetEventsByConditions_Failure_ListNull()
        {
            TestSetUp();

            List<Predicate<Event>> conditions = null;

            Event newEvent = new Event(1, "epic event", 0, "big event", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            Event newEvent2 = new Event(2, "epic event", 0, "big eveneet", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            Event newEvent3 = new Event(2, "not epic event", 0, "big eveneet", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            eventService.CreateEvent(newEvent);
            eventService.CreateEvent(newEvent2);
            eventService.CreateEvent(newEvent3);
            List<Event> events = eventService.GetAllEvents();
            events = FilterHelpers.GetItemsOnConditions(conditions, events);

            Assert.AreEqual(3, events.Count);
        }


        [TestMethod]
        public void GetEventsByConditions_Success_SearchTitle()
        {
            TestSetUp();

            List<Predicate<Event>> conditions = new List<Predicate<Event>>();
            
            conditions.Add(new Predicate<Event>(e => e.Title == "epic event"));

            Event newEvent = new Event(1, "epic event", 0, "big event", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            Event newEvent2 = new Event(2, "epic event", 0, "big eveneet", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            Event newEvent3 = new Event(2, "not epic event", 0, "big eveneet", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            eventService.CreateEvent(newEvent);
            eventService.CreateEvent(newEvent2);
            eventService.CreateEvent(newEvent3);
            List<Event> events = eventService.GetAllEvents();
            events = FilterHelpers.GetItemsOnConditions(conditions, events);

            Assert.AreEqual(2, events.Count);
            Assert.AreEqual(events[0].Title, "epic event");
            Assert.AreEqual(events[1].Title, "epic event");
        }


        [TestMethod]
        public void GetEventsByConditions_Success_SearchTitleANDDescription()
        {
            TestSetUp();

            List<Predicate<Event>> conditions = new List<Predicate<Event>>();
            conditions.Add(new Predicate<Event>(e => e.Title == "epic event"));
            conditions.Add(new Predicate<Event>(e => e.Description == "big event"));
            Event newEvent = new Event(1, "epic event", 0, "big event", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            Event newEvent2 = new Event(2, "epic event", 0, "big eveneet", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            Event newEvent3 = new Event(2, "not epic event", 0, "big eveneet", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            eventService.CreateEvent(newEvent);
            eventService.CreateEvent(newEvent2);
            eventService.CreateEvent(newEvent3);
            List<Event> events = eventService.GetAllEvents();
            events = FilterHelpers.GetItemsOnConditions(conditions, events);

            Assert.AreEqual(1, events.Count);
            Assert.AreEqual(events[0].Title, "epic event");
            Assert.AreEqual(events[0].Description, "big event");
        }
        [TestMethod]
        public void GetEventsByConditions_Success_EmptyList()
        {
            TestSetUp();

            List<Predicate<Event>> conditions = new List<Predicate<Event>>();
            
            Event newEvent = new Event(1, "epic event", 0, "big event", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            Event newEvent2 = new Event(2, "epic event", 0, "big eveneet", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            Event newEvent3 = new Event(2, "not epic event", 0, "big eveneet", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            eventService.CreateEvent(newEvent);
            eventService.CreateEvent(newEvent2);
            eventService.CreateEvent(newEvent3);
            List<Event> events = eventService.GetAllEvents();
            events = FilterHelpers.GetItemsOnConditions(conditions, events);

            Assert.AreEqual(3, events.Count);
        }


        public void GetEventsByConditionsOtherCTOR_Fail_ConditionNull()
        {
            TestSetUp();
            List<Event> events = new List<Event>();
            List<Predicate<Event>> conditions = new List<Predicate<Event>>();
            conditions.Add(null);

            Event newEvent = new Event(1, "epic event", 0, "big event", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            Event newEvent2 = new Event(2, "epic event", 0, "big eveneet", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            Event newEvent3 = new Event(2, "not epic event", 0, "big eveneet", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            events.Add(newEvent);
            events.Add(newEvent2);
            events.Add(newEvent3);
            events = FilterHelpers.GetItemsOnConditions(conditions, events);

            Assert.AreEqual(3, events.Count);
        }


        [TestMethod]
        public void GetEventsByConditionsOtherCTOR_Failure_ListNull()
        {
            TestSetUp();

            List<Predicate<Event>> conditions = null;
            List<Event> events = new List<Event>();
            Event newEvent = new Event(1, "epic event", 0, "big event", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            Event newEvent2 = new Event(2, "epic event", 0, "big eveneet", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            Event newEvent3 = new Event(2, "not epic event", 0, "big eveneet", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            events.Add(newEvent);
            events.Add(newEvent2);
            events.Add(newEvent3);
            events = FilterHelpers.GetItemsOnConditions(conditions, events);

            Assert.AreEqual(3, events.Count);
        }


        [TestMethod]
        public void GetEventsByConditionsOtherCTOR_Success_SearchTitle()
        {
            TestSetUp();
            List<Event> events = new List<Event>();
            List<Predicate<Event>> conditions = new List<Predicate<Event>>();

            conditions.Add(new Predicate<Event>(e => e.Title == "epic event"));

            Event newEvent = new Event(1, "epic event", 0, "big event", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            Event newEvent2 = new Event(2, "epic event", 0, "big eveneet", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            Event newEvent3 = new Event(2, "not epic event", 0, "big eveneet", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            events.Add(newEvent);
            events.Add(newEvent2);
            events.Add(newEvent3);
            events = FilterHelpers.GetItemsOnConditions(conditions, events);

            Assert.AreEqual(2, events.Count);
            Assert.AreEqual(events[0].Title, "epic event");
            Assert.AreEqual(events[1].Title, "epic event");
        }


        [TestMethod]
        public void GetItemsByConditionsOtherCTOR_Success_SearchTitleANDDescription()
        {
            TestSetUp();
            List<Event> events = new List<Event>();
            List<Predicate<Event>> conditions = new List<Predicate<Event>>();
            conditions.Add(new Predicate<Event>(e => e.Title == "epic event"));
            conditions.Add(new Predicate<Event>(e => e.Description == "big event"));
            Event newEvent = new Event(1, "epic event", 0, "big event", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            Event newEvent2 = new Event(2, "epic event", 0, "big eveneet", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            Event newEvent3 = new Event(2, "not epic event", 0, "big eveneet", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            events.Add(newEvent);
            events.Add(newEvent2);
            events.Add(newEvent3);
            events = FilterHelpers.GetItemsOnConditions(conditions, events);

            Assert.AreEqual(1, events.Count);
            Assert.AreEqual(events[0].Title, "epic event");
            Assert.AreEqual(events[0].Description, "big event");
        }
        [TestMethod]
        public void GetEventsByConditionsOtherCTOR_Success_EmptyList()
        {
            TestSetUp();
            List<Event> events = new List<Event>();
            List<Predicate<Event>> conditions = new List<Predicate<Event>>();

            Event newEvent = new Event(1, "epic event", 0, "big event", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            Event newEvent2 = new Event(2, "epic event", 0, "big eveneet", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            Event newEvent3 = new Event(2, "not epic event", 0, "big eveneet", new TimeBetween(new DateTime(2030, 12, 6), new DateTime(2030, 12, 7)));
            events.Add(newEvent);
            events.Add(newEvent2);
            events.Add(newEvent3);
            events = FilterHelpers.GetItemsOnConditions(conditions, events);

            Assert.AreEqual(3, events.Count);
        }
    }
}

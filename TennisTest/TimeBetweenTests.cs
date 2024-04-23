using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tennis.Helpers;

namespace TennisTest
{
    [TestClass]
    public class TimeBetweenTests
    {
        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void StartTimeIsLaterThanEndTime_Fail()
        {
            TimeBetween time = new TimeBetween(new DateTime(2013), new DateTime(2000));
        }
        [TestMethod]
        public void EndTimeIsLaterThanStartTime_Sucess()
        {
            TimeBetween time = new TimeBetween(new DateTime(2013, 6, 1), new DateTime(2014, 7, 30));
            Assert.AreEqual(1, time.StartTime.Day);
            Assert.AreEqual(6, time.StartTime.Month);
            Assert.AreEqual(2013, time.StartTime.Year);

            Assert.AreEqual(30, time.EndTime.Day);
            Assert.AreEqual(7, time.EndTime.Month);
            Assert.AreEqual(2014, time.EndTime.Year);
        }

        [TestMethod]
        public void TimeSpanTest_1Year_Sucess()
        {
            TimeBetween time = new TimeBetween(new DateTime(2013, 6, 1), new DateTime(2014, 6, 1));
            Assert.AreEqual(365, time.TimeSpan.Days);
        }

        [TestMethod]
        public void TimeSpanTest_4Hours_Sucess()
        {
            TimeBetween time = new TimeBetween(new DateTime(2013, 6, 1,6,0,0), new DateTime(2013, 6, 1, 10, 0, 0));
            Assert.AreEqual(4, time.TimeSpan.Hours);
        }
        [TestMethod]
        public void TimeSpanTest_4Hours30Minutes_Sucess()
        {
            TimeBetween time = new TimeBetween(new DateTime(2013, 6, 1, 6, 0, 0), new DateTime(2013, 6, 1, 10, 30, 0));
            Assert.AreEqual(4.5, time.TimeSpan.TotalHours);
        }
        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void ChangeStartTimeToBeLaterThanEndTime_Fail()
        {
            TimeBetween time = new TimeBetween(new DateTime(2013, 6, 1, 6, 0, 0), new DateTime(2013, 6, 1, 10, 30, 0));
            time.SetNewTime(new DateTime(2013, 6, 2, 6, 0, 0), new DateTime(2013, 6, 1, 6, 0, 0));
        }
        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void ChangeEndTimeToBeLaterThanStartTime_Fail()
        {
            TimeBetween time = new TimeBetween(new DateTime(2013, 6, 1, 6, 0, 0), new DateTime(2013, 6, 1, 10, 30, 0));
            time.SetNewTime(new DateTime(2013, 6, 1, 6, 0, 0), new DateTime(2013, 5, 2, 6, 0, 0));
        }
        [TestMethod]
        public void ChangeTime_Sucess()
        {
            TimeBetween time = new TimeBetween(new DateTime(2013, 6, 1, 6, 0, 0), new DateTime(2013, 6, 1, 10, 30, 0));
            time.SetNewTime(new DateTime(2013, 6, 1, 6, 0, 0), new DateTime(2013, 6, 10, 6, 0, 0)); 
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void ChangeTime_DateTimeMethods_Fail()
        {
            TimeBetween time = new TimeBetween(new DateTime(2013, 6, 1, 6, 0, 0), new DateTime(2013, 6, 1, 10, 30, 0));
            TimeBetween timeNew = new TimeBetween(new DateTime(2013, 6, 1, 6, 0, 0), new DateTime(2013, 6, 1, 10, 30, 0));
            time.EndTime.AddMinutes(100);
            time.EndTime.AddHours(100);
            time.EndTime.AddDays(100);
            Assert.AreEqual(timeNew.EndTime, time.EndTime);
        }


    }
}

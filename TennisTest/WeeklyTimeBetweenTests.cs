using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tennis.Helpers;

namespace TennisTest
{
    //    [TestClass]
    //    public class WeeklyTimeBetweenTests
    //    {

    //        [TestMethod]
    //        public void EndTimeIsLaterThanStartTime_Sucess()
    //        {
    //            WeeklyTimeBetween time = new WeeklyTimeBetween(new TimeOnly(18,10,0), new TimeOnly(20, 10,0), DayOfWeek.Monday);
    //            Assert.AreEqual(18, time.StartTime.Hour);
    //            Assert.AreEqual(10, time.StartTime.Minute);

    //            Assert.AreEqual(20, time.EndTime.Hour);
    //            Assert.AreEqual(10, time.EndTime.Minute);
    //        }

    //        [TestMethod]
    //        public void TimeSpanTest_1Hour_Sucess()
    //        {
    //            WeeklyTimeBetween time = new WeeklyTimeBetween(new TimeOnly(16, 10, 0), new TimeOnly(17, 10, 0), DayOfWeek.Monday);
    //            Assert.AreEqual(1, time.TimeSpan.Hours);
    //        }

    //        [TestMethod]
    //        public void TimeSpanTest_4Hours20Minutes_Sucess()
    //        {
    //            WeeklyTimeBetween time = new WeeklyTimeBetween(new TimeOnly(16, 0, 0), new TimeOnly(20, 20, 0), DayOfWeek.Monday);
    //            Assert.AreEqual(4, time.TimeSpan.Hours);
    //            Assert.AreEqual(20, time.TimeSpan.Minutes);
    //        }
    //        [TestMethod]
    //        public void TimeSpanTest_4Hours20Minutes_OverMidnight_Sucess()
    //        {
    //            WeeklyTimeBetween time = new WeeklyTimeBetween(new TimeOnly(21, 0, 0), new TimeOnly(1, 20, 0), DayOfWeek.Monday);
    //            Assert.AreEqual(4, time.TimeSpan.Hours);
    //            Assert.AreEqual(20, time.TimeSpan.Minutes);
    //        }
    //        [TestMethod]
    //        public void TimeSpanTest_SameTime_OverMidnight_Sucess()
    //        {
    //            WeeklyTimeBetween time = new WeeklyTimeBetween(new TimeOnly(21, 0, 0), new TimeOnly(21, 0, 0), DayOfWeek.Monday);
    //            Assert.AreEqual(24, time.TimeSpan.TotalHours);
    //            Assert.AreEqual(0, time.TimeSpan.Minutes);
    //        }
    //    }
}

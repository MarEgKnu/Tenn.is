﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tennis.Helpers;

namespace TennisTest
{
    [TestClass]
    public class WeeklyTimeBetweenTests
    {

        [TestMethod]
        public void EndTimeIsLaterThanStartTime_Sucess()
        {
            WeeklyTimeBetween time = new WeeklyTimeBetween(new TimeOnly(18,10,0), new TimeOnly(20, 10,0), DayOfWeek.Monday);
            Assert.AreEqual(18, time.StartTime.Value.Hour);
            Assert.AreEqual(10, time.StartTime.Value.Minute);

            Assert.AreEqual(20, time.EndTime.Value.Hour);
            Assert.AreEqual(10, time.EndTime.Value.Minute);
        }

        [TestMethod]
        public void TimeSpanTest_1Hour_Sucess()
        {
            WeeklyTimeBetween time = new WeeklyTimeBetween(new TimeOnly(16, 10, 0), new TimeOnly(17, 10, 0), DayOfWeek.Monday);
            Assert.AreEqual(1, time.TimeSpan.Value.Hours);
        }

        [TestMethod]
        public void TimeSpanTest_4Hours20Minutes_Sucess()
        {
            WeeklyTimeBetween time = new WeeklyTimeBetween(new TimeOnly(16, 0, 0), new TimeOnly(20, 20, 0), DayOfWeek.Monday);
            Assert.AreEqual(4, time.TimeSpan.Value.Hours);
            Assert.AreEqual(20, time.TimeSpan.Value.Minutes);
        }
        [TestMethod]
        public void TimeSpanTest_4Hours20Minutes_OverMidnight_Sucess()
        {
            WeeklyTimeBetween time = new WeeklyTimeBetween(new TimeOnly(21, 0, 0), new TimeOnly(1, 20, 0), DayOfWeek.Monday);
            Assert.AreEqual(4, time.TimeSpan.Value.Hours);
            Assert.AreEqual(20, time.TimeSpan.Value.Minutes);
        }
        [TestMethod]
        public void TimeSpanTest_SameTime_OverMidnight_Sucess()
        {
            WeeklyTimeBetween time = new WeeklyTimeBetween(new TimeOnly(21, 0, 0), new TimeOnly(21, 0, 0), DayOfWeek.Monday);
            Assert.AreEqual(24, time.TimeSpan.Value.TotalHours);
            Assert.AreEqual(0, time.TimeSpan.Value.Minutes);
        }
        [TestMethod]
        public void NextStartTest_Sucess_2days2hrs()
        {
            DateTime now = DateTime.Now;
            WeeklyTimeBetween time = new WeeklyTimeBetween(new TimeOnly(now.AddHours(2).Hour,  0, 0), new TimeOnly(now.AddHours(3).Hour, 0, 0), DayOfWeekHelpers.IntToDayOfWeek( (int)now.DayOfWeek + 2));
            Assert.AreEqual(now.AddDays(2).AddHours(2).Day, time.NextStart.Day);
            Assert.AreEqual(now.AddDays(2).AddHours(2).Hour, time.NextStart.Hour);
            Assert.AreEqual(now.AddDays(2).AddHours(2).DayOfWeek, time.NextStart.DayOfWeek);
        }
        [TestMethod]
        public void NextStartTest_Sucess_1hour()
        {
            DateTime now = DateTime.Now;
            WeeklyTimeBetween time = new WeeklyTimeBetween(new TimeOnly(now.AddHours(1).Hour, 0, 0), new TimeOnly(now.AddHours(2).Hour, 0, 0), now.DayOfWeek);
            Assert.AreEqual(now.AddHours(1).Day, time.NextStart.Day);
            Assert.AreEqual(now.AddHours(1).Hour, time.NextStart.Hour);
            Assert.AreEqual(now.AddHours(1).DayOfWeek, time.NextStart.DayOfWeek);
        }
        [TestMethod]
        public void NextStartTest_Sucess_SameDay1HourAgo()
        {
            DateTime now = DateTime.Now;
            WeeklyTimeBetween time = new WeeklyTimeBetween(new TimeOnly(now.AddHours(-2).Hour, 0, 0), new TimeOnly(now.AddHours(-1).Hour, 0, 0), now.DayOfWeek);
            Assert.AreEqual(now.AddHours(-2).AddDays(7).Day, time.NextStart.Day);
            Assert.AreEqual(now.AddHours(-2).AddDays(7).Hour, time.NextStart.Hour);
            Assert.AreEqual(now.AddHours(-2).AddDays(7).DayOfWeek, time.NextStart.DayOfWeek);
        }
    }
}

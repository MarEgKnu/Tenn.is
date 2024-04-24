using System;
using System.Collections.Generic;
using Tennis.Interfaces;
using Tennis.Models;
using Tennis.Services;

namespace TennisTest
{
    [TestClass]
    public class ILaneServiceTest
    {
        //private ILaneService service;

        public LaneService service { get; set; }

        public void TestSetUp()
        {
            service = new LaneService(true);
        }

        [TestMethod]
        public void GetAllLanes()
        {
            TestSetUp();
            service.DeleteLane(250);
            int numberbefore = service.GetAllLanes().Count;
            service.CreateLane(new Lane(250, true, true));

            Assert.AreEqual(service.GetAllLanes().Count, numberbefore +1);
        }

        [TestMethod]
        public void GetLaneByNumberExist()
        {
            TestSetUp();
            service.CreateLane(new Lane(250, true, true));
            Assert.AreNotEqual(service.GetLaneByNumber(250), null);
        }




        [TestMethod]
        public void GetLaneByNumberInexistant()
        {
            TestSetUp();
            service.DeleteLane(300);
            Assert.AreEqual(service.GetLaneByNumber(300), null);
        }


        [TestMethod]
        public void CreateLaneTestAcceptableValues()
        {
            TestSetUp();
            service.DeleteLane(250);
            Assert.AreEqual(service.CreateLane(new Lane(250, true, true)), true);
        }



        [TestMethod]
        public void CreateLaneTestUnacceptableValues()
        {
            TestSetUp();
            service.DeleteLane(250);
            bool created = service.CreateLane(new Lane(299, true, true));

            Assert.AreEqual( created,false);
        }

        [TestMethod]
        public void CreateLaneTest2TimesTheSame()
        {
            TestSetUp();
            service.DeleteLane(250);
            int numberbefore = service.GetAllLanes().Count;
            service.CreateLane(new Lane(250, true, true));
            service.CreateLane(new Lane(250, true, true));
            Assert.AreEqual(service.GetAllLanes().Count, numberbefore + 1);
        }


        [TestMethod]
        public void DeleteLaneTestExisting()
        {
            TestSetUp();
            service.CreateLane(new Lane(250, true, true));
            
            Assert.AreEqual(true, service.DeleteLane(250));
        }



        [TestMethod]
        public void DeleteLaneTestInexisting()
        {
            TestSetUp();
            service.DeleteLane(150);
            Assert.AreEqual(false, service.DeleteLane(150));
        }


        [TestMethod]
        public void EditLaneTestAcceptable()
        {
            TestSetUp();
            service.CreateLane(new Lane(250, true, true));
            Assert.AreEqual(service.EditLane(new Lane(250, true, true), 250), true);
        }


        [TestMethod]
        public void EditLaneTestUnAcceptableNotExistingID()
        {
            TestSetUp();
            service.DeleteLane(250);
            Assert.AreEqual(service.EditLane(new Lane(250, true, true), 250), false);
        }



        [TestMethod]
        public void TestMethod1()
        {

        }
    }
}

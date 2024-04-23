using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tennis.Interfaces;
using Tennis.Models;
using Tennis.Services;

namespace TennisTest
{
    [TestClass]
    public class ILaneServiceTest
    {
        ILaneService service;
        public ILaneServiceTest( ILaneService Service)
        {
            service = Service;
        }

        string SelectAllSQL = "SELECT * FROM LANES";
        string SelectLaneByIdSQL = "SELECT * FROM LANES WHERE LANENUMBER = @ID";
        string InsertLaneSQL = "INSERT INTO LANE VALUES(@ID,@OUTDOOR, @PADELTENNIS)";
        string UpdateLaneSQL = "UPDATE LANE SET OUTDOORS = @OUTDOOR, PADELTENNIS = PADELTENNIS WHERE LANENUMBER = @ID";
        string DeleteLane = "DELETE FROM LANE WHERE LANENUMBER = @ID";


        void TestSetUp()
        {

        }

        //[TestMethod]
        //public void GetAllLanes()
        //{
        //    List<Lane> Lanelist = new List<Lane>();
        //    using (SqlConnection connection = new SqlConnection(ConnectionStringTest))
        //        try
        //        {
        //            SqlCommand command = new SqlCommand(SelectAllSQL, connection);
        //            command.Connection.Open();
        //            SqlDataReader reader = command.ExecuteReader();
        //            while (reader.Read())
        //            {
        //                Lanelist.Add(new Lane(reader.GetInt32(LANENUMBER), reader.GetBoolean(OUTDOOR), reader.GetBoolean(PADELTENNIS)));
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //        }

        //    Assert.AreEqual(service.GetAllLanes().Count, Lanelist.Count);
        //}

        [TestMethod]
        public void GetLaneByNumber()
        {

        }



        [TestMethod]
        public void CreateLaneTestAcceptableValues()
        {

        }



        [TestMethod]
        public void CreateLaneTestUnacceptableValues()
        {

        }


        [TestMethod]
        public void TestMethod3()
        {

        }

    }
}

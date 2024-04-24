using System.Data;
using Microsoft.Data.SqlClient;
using System.Numerics;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Services
{
    public class LaneService : Connection, ILaneService
    {
        public LaneService()
        {
            connectionString = Secret.ConnectionString;
        }
        public LaneService(bool test)
        {
            if (test)
            {
                connectionString = Secret.ConnectionStringTest;
            }
            else
            {
                connectionString = Secret.ConnectionString;
            }
            
        }

        string SelectAllSQL = "SELECT * FROM Lanes";
        string SelectLaneByIdSQL = "SELECT * FROM Lanes WHERE LANENUMBER = @ID";
        string InsertLaneSQL = "INSERT INTO Lanes VALUES (@ID, @OUTDOOR, @PADELTENNIS)";
        string UpdateLaneSQL = "UPDATE Lanes SET OUTDOORS = @OUTDOOR, PADELTENNIS = @PADELTENNIS WHERE LANENUMBER = @ID";
        string DeleteLaneSQL = "DELETE FROM Lanes WHERE LANENUMBER = @ID";


        public List<Lane> GetAllLanes()
        {
            List<Lane> Lanelist = new List<Lane>();
            using (SqlConnection connection = new SqlConnection(connectionString))
                try
                {
                    SqlCommand command = new SqlCommand("SELECT * FROM Lanes", connection);
                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lanelist.Add(new Lane(reader.GetInt32("LANENUMBER"), reader.GetBoolean("OUTDOOR"), reader.GetBoolean("PADELTENNIS")));
                    }
                }
                catch (SqlException sqlExp)
                {
                    Console.WriteLine("Database error" + sqlExp.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            return Lanelist;
        }

        public Lane GetLaneByNumber(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
                try
                {
                    SqlCommand command = new SqlCommand("SELECT * FROM DBO.Users", connection);
                    //command.Parameters.AddWithValue("@ID", id);
                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        return new Lane(reader.GetInt32("LANENUMBER"), reader.GetBoolean("OUTDOOR"), reader.GetBoolean("PADELTENNIS"));
                    }

                }
                catch (SqlException sqlExp)
                {
                    Console.WriteLine("Database error" + sqlExp.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }


            return null;
        }




        public bool CreateLane(Lane lane)
        {
            if (GetLaneByNumber(lane.Id) != null)
                return false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(InsertLaneSQL, connection);
                    command.Parameters.AddWithValue("@ID", lane.Id);
                    command.Parameters.AddWithValue("@OUTDOOR", lane.OutDoor);
                    command.Parameters.AddWithValue("@PADELTENNIS", lane.PadelTennis);
                    command.Connection.Open();
                    int NoOfRows = command.ExecuteNonQuery();
                    return NoOfRows == 1;
                }
                catch (SqlException sqlExp)
                {
                    Console.WriteLine("Database error" + sqlExp.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("generel fejl" + ex.Message);
                }
                return false;
            }
        }

        public bool DeleteLane(int id)
        {
            if (GetLaneByNumber(id) == null)
                return false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(DeleteLaneSQL, connection);
                    command.Parameters.AddWithValue("@ID", id);
                    command.Connection.Open();
                    int NoOfRows = command.ExecuteNonQuery();
                    return NoOfRows == 1;
                }
                catch (SqlException sqlExp)
                {
                    Console.WriteLine("Database error" + sqlExp.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("generel fejl" + ex.Message);
                }
                return false;
            }
        }

        public bool EditLane(Lane lane, int id)
        {
            if (GetLaneByNumber(id) == null)
                return false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(UpdateLaneSQL, connection);
                    command.Parameters.AddWithValue("@OUTDOOR", lane.OutDoor);
                    command.Parameters.AddWithValue("@PADELTENNIS", lane.PadelTennis);
                    command.Parameters.AddWithValue("@ID", id);
                    command.Connection.Open();
                    int NoOfRows = command.ExecuteNonQuery();
                    return NoOfRows == 1;
                }
                catch (SqlException sqlExp)
                {
                    Console.WriteLine("Database error" + sqlExp.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("generel fejl" + ex.Message);
                }
                return false;
            }
        }


    }
}

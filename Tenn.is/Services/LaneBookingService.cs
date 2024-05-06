﻿using Microsoft.Data.SqlClient;
using System.Data;
using Tennis.Helpers;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Services
{
    public class LaneBookingService : Connection, ILaneBookingService
    {
        public LaneBookingService()
        {
            connectionString = Secret.ConnectionString;
        }
        public LaneBookingService(bool test)
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


        string GetAllLaneBookingSQL = "SELECT * FROM LANEBOOKINGS";
        string GetLaneBookingByIdSQL = "SELECT * FROM LANEBOOKINGS WHERE BOOKINGID = @BookingID";
        string CreateLaneBookingSQL = "INSERT INTO LANEBOOKINGS ( LaneNumber, Cancelled, DateStart, DateEnd, UserID,  MateID , TrainingTeamID) VALUES ( @LaneNumber, @cancelled, @DateStart, @DateEnd, @UserID,  @MateID, @TrainingTeamID ) ";
        string DeleteLaneBookingSQL = "DELETE FROM LANEBOOKINGS WHERE BOOKINGID = @BookingID";
        string UpdateLaneBookingSQL = "UPDATE LANEBOOKINGS SET LaneNumber = @LaneNumber, DateStart = @DateStart, DateEnd = @DateEnd WHERE @ID = BOOKINGID";
        string CancelLaneBookingSQL = "UPDATE LANEBOOKINGS SET Cancelled = 'TRUE' WHERE BOOKINGID = @BookingID";





        public List<T> GetAllLaneBookings<T>() where T : LaneBooking
        {
            string getAllUserLaneBookingSQL = GetAllLaneBookingSQL;
            List<T> LaneBookingList = new List<T>();
            if (typeof(UserLaneBooking) == typeof(T))
                getAllUserLaneBookingSQL += " WHERE TrainingTeamID IS NULL";
            else if (typeof(TrainingTeam) == typeof(T))
                getAllUserLaneBookingSQL += " WHERE UserID IS  NULL AND MateID IS NULL";

            using (SqlConnection connection = new SqlConnection(connectionString))
                try
                {
                    SqlCommand command = new SqlCommand(getAllUserLaneBookingSQL, connection);
                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        
                        TimeBetween timeBetween = new TimeBetween(reader.GetDateTime("DateStart"), reader.GetDateTime("DateEnd"));
                        if (typeof(UserLaneBooking) == typeof(T))
                        {
                            object[] userLaneBooking = new object[] { reader.GetInt32("BookingID"), reader.GetInt32("LaneNumber"), timeBetween, reader.GetInt32("UserID"), reader.GetInt32("MateID"), reader.GetBoolean("Cancelled") };
                            LaneBookingList.Add((T)Activator.CreateInstance(typeof(T), userLaneBooking));
                        }
                        else if (typeof(TrainingLaneBooking) == typeof(T))
                        {
                            //TrainingTeam trainingTeam = trainingTeamservice.GetTrainingTeamByID(reader.GetInt32("TrainingTeamID"));
                            //object[] userLaneBooking = new object[] { reader.GetInt32("LaneNumber"), timeBetween, reader.GetInt32("BookingID"), reader.GetBoolean("Cancelled"), trainingTeam };
                            //LaneBookingList.Add((T)Activator.CreateInstance(typeof(T), userLaneBooking));
                        }


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


            return LaneBookingList;
        }


        public UserLaneBooking GetUserLaneBookingById(int id)
        {
            string GetUserLaneBookingByIdSQL = GetLaneBookingByIdSQL + " AND TrainingTeamID IS NULL";
            using (SqlConnection connection = new SqlConnection(connectionString))
                try
                {
                    SqlCommand command = new SqlCommand(GetUserLaneBookingByIdSQL, connection);
                    command.Parameters.AddWithValue("@BookingID", id);
                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        TimeBetween timeBetween = new TimeBetween(reader.GetDateTime("DateStart"), reader.GetDateTime("DateEnd"));
                        return new UserLaneBooking(reader.GetInt32("BookingID"), reader.GetInt32("LaneNumber"), timeBetween, reader.GetInt32("UserID"), reader.GetInt32("MateID"), reader.GetBoolean("Cancelled"));

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

        public TrainingLaneBooking GetTrainingLaneBookingById(int id)
        {
            string GetUserLaneBookingByIdSQL = GetLaneBookingByIdSQL + " WHERE UserID IS  NULL AND MateID IS NULL";
            using (SqlConnection connection = new SqlConnection(connectionString))
                try
                {
                    SqlCommand command = new SqlCommand(GetUserLaneBookingByIdSQL, connection);
                    command.Parameters.AddWithValue("@BookingID", id);
                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    TimeBetween timeBetween = new TimeBetween(reader.GetDateTime("DateStart"), reader.GetDateTime("DateEnd"));
                    if (reader.Read())
                    {
                        //TrainingTeam trainingTeam = trainingTeamservice.GetTrainingTeamByID(reader.GetInt32("TrainingTeamID"));
                        //TimeBetween timeBetween = new TimeBetween(reader.GetDateTime("DateStart"), reader.GetDateTime("DateEnd"));
                        //return new TrainingLaneBooking(reader.GetInt32("LaneNumber"), timeBetween, reader.GetInt32("BookingID"), reader.GetBoolean("Cancelled"), trainingTeam);
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




        public bool CancelLaneBonking(int id)
        {
            if (GetUserLaneBookingById(id) is null)
                return false;
            using (SqlConnection connection = new SqlConnection(connectionString))
                try
                {
                    SqlCommand command = new SqlCommand(CancelLaneBookingSQL, connection);
                    command.Parameters.AddWithValue("@BookingID", id);
                    command.Connection.Open();
                    int noOfRows = command.ExecuteNonQuery();
                    return noOfRows == 1;
                }
                catch (SqlException sqlExp)
                {
                    Console.WriteLine("Database error" + sqlExp.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            return false;
        }

        public bool CreateLaneBooking(UserLaneBooking laneBooking)
        {
            if (GetUserLaneBookingById(laneBooking.BookingID) is not null)
                return false;
            if (VerifyNewBooking(laneBooking) == false)
                return false;
            using (SqlConnection connection = new SqlConnection(connectionString))
                try
                {
                    SqlCommand command = new SqlCommand(CreateLaneBookingSQL, connection);
                    command.Parameters.AddWithValue("@LaneNumber", laneBooking.LaneNumber);
                    command.Parameters.AddWithValue("@DateStart", laneBooking._timeBetween.StartTime);
                    command.Parameters.AddWithValue("@DateEnd", laneBooking._timeBetween.EndTime);
                    command.Parameters.AddWithValue("@UserID", laneBooking.UserID);
                    command.Parameters.AddWithValue("@MateID", laneBooking.MateID);
                    command.Parameters.AddWithValue("@cancelled", laneBooking.Cancelled); 
                    command.Parameters.AddWithValue("@TrainingTeamID", DBNull.Value);
                    command.Connection.Open();
                    int noOfRows = command.ExecuteNonQuery();
                    return noOfRows == 1;
                }
                catch (SqlException sqlExp)
                {
                    Console.WriteLine("Database error" + sqlExp.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            return false;
        }

        public bool CreateLaneBooking(TrainingLaneBooking laneBooking)
        {
            throw new NotImplementedException();
        }

        public bool VerifyNewBooking(UserLaneBooking laneBooking)
        {
            List<UserLaneBooking> userLaneBookingList = GetAllLaneBookings<UserLaneBooking>();
            if (laneBooking.MateID == laneBooking.UserID)
                return false;
            IEnumerable<UserLaneBooking> UserAlredyBooked = from b in userLaneBookingList where laneBooking._timeBetween.StartTime == b._timeBetween.StartTime && laneBooking.UserID == b.UserID select b ;
            if (UserAlredyBooked.Count() > 0)
                return false;
            if (laneBooking.MateID > 0)
            {
                IEnumerable<UserLaneBooking> MateAlredyBooked = from b in userLaneBookingList where laneBooking._timeBetween.StartTime == b._timeBetween.StartTime && laneBooking.MateID == b.MateID select b;
                if (UserAlredyBooked.Count() > 0)
                    return false;

                Mate4HoursRule(laneBooking, userLaneBookingList);
            }
            IEnumerable<UserLaneBooking> AlreadyBooked = from b in userLaneBookingList where laneBooking._timeBetween.StartTime == b._timeBetween.StartTime && laneBooking.LaneNumber == b.LaneNumber select b;
            if (AlreadyBooked.Count() > 0)
                return false;

            User4HoursRule(laneBooking, userLaneBookingList);

            return true;
        }

        public bool User4HoursRule(UserLaneBooking laneBooking, List<UserLaneBooking> userLaneBookingList )
        {
            IEnumerable<UserLaneBooking> fourthMostRecentBookingUser = from b in userLaneBookingList where laneBooking._timeBetween.StartTime.Value.AddDays(-14) <= b._timeBetween.StartTime && laneBooking._timeBetween.StartTime.Value.AddDays(14) >= b._timeBetween.StartTime && laneBooking.UserID == b.UserID orderby b._timeBetween.StartTime select b;
            List<UserLaneBooking> fourthMostRecentBookingUserList = fourthMostRecentBookingUser.ToList();
            for (int i = 0; i < fourthMostRecentBookingUser.Count() - 3; i++)
            {
                if (fourthMostRecentBookingUserList[i]._timeBetween.StartTime.Value.AddDays(14) > fourthMostRecentBookingUserList[i + 3]._timeBetween.StartTime)
                    return false;
            }
            return true;
        }

        public bool Mate4HoursRule(UserLaneBooking laneBooking, List<UserLaneBooking> userLaneBookingList)
        {
            IEnumerable<UserLaneBooking> fourthMostRecentBookingMate = from b in userLaneBookingList where laneBooking._timeBetween.StartTime.Value.AddDays(-14) <= b._timeBetween.StartTime && laneBooking._timeBetween.StartTime.Value.AddDays(14) >= b._timeBetween.StartTime && laneBooking.MateID == b.MateID orderby b._timeBetween.StartTime select b;
            List<UserLaneBooking> fourthMostRecentBookingMateList = fourthMostRecentBookingMate.ToList();
            for (int i = 0; i < fourthMostRecentBookingMateList.Count() - 3; i++)
            {
                if (fourthMostRecentBookingMateList[i]._timeBetween.StartTime.Value.AddDays(14) > fourthMostRecentBookingMateList[i + 3]._timeBetween.StartTime)
                    return false;
            }
            return true;
        }





        public bool DeleteLaneBooking(int id)
        {
            if (GetUserLaneBookingById(id) is null)
                return false;
            using (SqlConnection connection = new SqlConnection(connectionString))
                try
                {
                    SqlCommand command = new SqlCommand(DeleteLaneBookingSQL, connection);
                    command.Parameters.AddWithValue("@BookingID", id);
                    command.Connection.Open();
                    int noOfRows = command.ExecuteNonQuery();
                    return noOfRows == 1;
                }
                catch (SqlException sqlExp)
                {
                    Console.WriteLine("Database error" + sqlExp.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            return false;
        }

        public bool EditLaneBooking(UserLaneBooking laneBooking, int id)
        {
            if (GetUserLaneBookingById(id) is null)
                return false;
            using (SqlConnection connection = new SqlConnection(connectionString))
                try
                {
                    SqlCommand command = new SqlCommand(UpdateLaneBookingSQL, connection);
                    command.Parameters.AddWithValue("@LaneNumber", laneBooking.LaneNumber);
                    command.Parameters.AddWithValue("@DateStart", laneBooking._timeBetween.StartTime);
                    command.Parameters.AddWithValue("@DateEnd", laneBooking._timeBetween.EndTime); 
                    command.Parameters.AddWithValue("@ID", laneBooking.Cancelled);
                    command.Connection.Open();
                    int noOfRows = command.ExecuteNonQuery();
                    return noOfRows == 1;
                }
                catch (SqlException sqlExp)
                {
                    Console.WriteLine("Database error" + sqlExp.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            return false;
        }

        public bool EditLaneBooking(TrainingLaneBooking laneBooking, int id)
        {
            throw new NotImplementedException();
        }
    }
}

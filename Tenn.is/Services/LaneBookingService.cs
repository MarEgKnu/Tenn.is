using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client.Extensibility;
using System.Data;
using System.Reflection.PortableExecutable;
using System.Reflection;
using Tennis.Exceptions;
using Tennis.Helpers;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Services
{
    public class LaneBookingService : Connection, ILaneBookingService
    {
        public ITrainingTeamService trainingTeamService { get; set; }
        private ILaneService laneService;
        public LaneBookingService(ITrainingTeamService trainingTeamService)
        {
            connectionString = Secret.ConnectionString;
            this.trainingTeamService = trainingTeamService;
            this.laneService = new LaneService();

        }
        public LaneBookingService(bool test, ITrainingTeamService trainingTeamService)
        {
            this.trainingTeamService = trainingTeamService;
            this.laneService = new LaneService(test);
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
        string CreateUserLaneBookingSQL = "INSERT INTO LANEBOOKINGS ( LaneNumber, Cancelled, DateStart, UserID,  MateID , TrainingTeamID) VALUES ( @LaneNumber, @cancelled, @DateStart, @UserID,  @MateID, @TrainingTeamID ) ";
        //string CreateTrainingLaneBookingSQL = "INSERT INTO LANEBOOKINGS(LaneNumber, Cancelled, DateStart, UserID,  MateID , TrainingTeamID) VALUES ( @LaneNumber, @cancelled, @DateStart, @UserID,  @MateID, @TrainingTeamID )";
        string DeleteLaneBookingSQL = "DELETE FROM LANEBOOKINGS WHERE BOOKINGID = @BookingID";
        string UpdateLaneBookingSQL = "UPDATE LANEBOOKINGS SET LaneNumber = @LaneNumber, DateStart = @DateStart WHERE @ID = BOOKINGID";
        string CancelLaneBookingSQL = "UPDATE LANEBOOKINGS SET Cancelled = 'TRUE' WHERE BOOKINGID = @BookingID";
        string DeleteAutomaticLaneBookings = "DELETE FROM LaneBookings\n" +
                                             "WHERE TrainingTeamID = @TrainingTeamID AND Automatic = 1";
        string GetAllOnLaneAndTime = "SELECT * FROM LaneBookings\n" +
                                      "WHERE LaneNumber = @LaneNumber AND DateStart = @DateStart";
        string CreateTrainingLaneBookingSQL = "INSERT INTO LANEBOOKINGS(LaneNumber, Cancelled, DateStart, UserID,  MateID , TrainingTeamID, Automatic) VALUES ( @LaneNumber, @cancelled, @DateStart, @UserID,  @MateID, @TrainingTeamID, @Automatic)";

        string getFirstFreeLane = "SELECT TOP 1 * \n" +
                                   "FROM Lanes\n" +
                                    "WHERE NOT Lanes.LaneNumber IN (SELECT LaneNumber\n" +
                                    "\tFROM LaneBookings\n" +
                                    "\tWHERE DateStart = @DateStart)";
        string getNoOfBookings = "SELECT COUNT(*) AS Bookings\n" +
                                 "FROM LaneBookings\n" +
                                 "WHERE LaneNumber = @LaneNumber AND DateStart >= @minTime AND DateStart <= @maxTime";
        string LaneBookingStatus = "Select * FROM LaneBookings WHERE DateStart = @Date AND LaneNumber = @Lane AND Cancelled = 0";
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
                        if (typeof(UserLaneBooking) == typeof(T))
                        {
                            object[] userLaneBooking = new object[] { reader.GetInt32("BookingID"), reader.GetInt32("LaneNumber"), reader.GetDateTime("DateStart"), reader.GetInt32("UserID"), reader.GetInt32("MateID"), reader.GetBoolean("Cancelled") };
                            LaneBookingList.Add((T)Activator.CreateInstance(typeof(T), userLaneBooking));
                        }
                        else if (typeof(TrainingLaneBooking) == typeof(T))
                        {
                            object[] TrainingLaneBooking = new object[] { reader.GetInt32("LaneNumber"), reader.GetDateTime("DateStart"), reader.GetInt32("BookingID"), reader.GetBoolean("Cancelled"), trainingTeamService.GetTrainingTeamById(reader.GetInt32("TrainingTeamID")) };
                            LaneBookingList.Add((T)Activator.CreateInstance(typeof(T), TrainingLaneBooking));

                        }
                        else if (typeof(LaneBooking) == typeof(T))
                        {
                            if (reader.GetIntOrNull("UserID") != null)
                            {
                                LaneBooking laneBooking = new UserLaneBooking(reader.GetInt32("BookingID"), reader.GetInt32("LaneNumber"), reader.GetDateTime("DateStart"), reader.GetInt32("UserID"), reader.GetInt32("MateID"), reader.GetBoolean("Cancelled"));
                                LaneBookingList.Add((T)laneBooking);
                            }
                            else
                            {
                                LaneBooking laneBooking = new TrainingLaneBooking(reader.GetInt32("LaneNumber"), reader.GetDateTime("DateStart"), reader.GetInt32("BookingID"), reader.GetBoolean("Cancelled"), new TrainingTeam(reader.GetInt32("TrainingTeamID")), reader.GetBoolean("Automatic"));
                                LaneBookingList.Add((T)laneBooking);

                            }
                        }
                    }
                    reader.Close();
                    for (int i = 0; i < LaneBookingList.Count; i++)
                    {
                        if (LaneBookingList[i] is TrainingLaneBooking)
                        {
                            TrainingLaneBooking c = LaneBookingList[i] as TrainingLaneBooking;
                            c.trainingTeam = trainingTeamService.GetTrainingTeamById(c.trainingTeam.TrainingTeamID);
                        }
                    }
                }
                catch (SqlException sqlExp)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }

            return LaneBookingList;
        }

        public List<T> GetRelevantLaneBookings<T>() where T : LaneBooking
        {
            string getAllUserLaneBookingSQL = GetAllLaneBookingSQL;
            getAllUserLaneBookingSQL += " WHERE DateStart >= @DATENOW AND DateStart < @DATEEND AND Cancelled = 0";
            List<T> LaneBookingList = new List<T>();
            if (typeof(UserLaneBooking) == typeof(T))
                getAllUserLaneBookingSQL += " AND TrainingTeamID IS NULL";
            else if (typeof(TrainingTeam) == typeof(T))
                getAllUserLaneBookingSQL += " AND UserID IS  NULL AND MateID IS NULL";

            using (SqlConnection connection = new SqlConnection(connectionString))
                try
                {
                    SqlCommand command = new SqlCommand(getAllUserLaneBookingSQL, connection);
                    command.Parameters.AddWithValue("@DATENOW", DateTime.Now);
                    command.Parameters.AddWithValue("@DATEEND", DateTime.Now.AddDays(14));
                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (typeof(UserLaneBooking) == typeof(T))
                        {
                            object[] userLaneBooking = new object[] { reader.GetInt32("BookingID"), reader.GetInt32("LaneNumber"), reader.GetDateTime("DateStart"), reader.GetInt32("UserID"), reader.GetInt32("MateID"), reader.GetBoolean("Cancelled") };
                            LaneBookingList.Add((T)Activator.CreateInstance(typeof(T), userLaneBooking));
                        }
                        else if (typeof(TrainingLaneBooking) == typeof(T))
                        {
                            object[] TrainingLaneBooking = new object[] { reader.GetInt32("LaneNumber"), reader.GetDateTime("DateStart"), reader.GetInt32("BookingID"), reader.GetBoolean("Cancelled"), trainingTeamService.GetTrainingTeamById(reader.GetInt32("TrainingTeamID")) };
                            LaneBookingList.Add((T)Activator.CreateInstance(typeof(T), TrainingLaneBooking));

                        }
                        else if (typeof(LaneBooking) == typeof(T))
                        {
                            if (reader.GetIntOrNull("UserID") != null)
                            {
                                LaneBooking laneBooking = new UserLaneBooking(reader.GetInt32("BookingID"), reader.GetInt32("LaneNumber"), reader.GetDateTime("DateStart"), reader.GetInt32("UserID"), reader.GetInt32("MateID"), reader.GetBoolean("Cancelled"));
                                LaneBookingList.Add((T)laneBooking);
                            }
                            else
                            {
                                LaneBooking laneBooking = new TrainingLaneBooking(reader.GetInt32("LaneNumber"), reader.GetDateTime("DateStart"), reader.GetInt32("BookingID"), reader.GetBoolean("Cancelled"), new TrainingTeam(reader.GetInt32("TrainingTeamID")), reader.GetBoolean("Automatic"));
                                LaneBookingList.Add((T)laneBooking);

                            }
                        }
                    }
                    reader.Close();
                    for (int i = 0; i < LaneBookingList.Count; i++)
                    {
                        if (LaneBookingList[i] is TrainingLaneBooking)
                        {
                            TrainingLaneBooking c = LaneBookingList[i] as TrainingLaneBooking;
                            c.trainingTeam = trainingTeamService.GetTrainingTeamById(c.trainingTeam.TrainingTeamID);
                        }
                    }
                }
                catch (SqlException sqlExp)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
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
                        return new UserLaneBooking(reader.GetInt32("BookingID"), reader.GetInt32("LaneNumber"), reader.GetDateTime("DateStart"), reader.GetInt32("UserID"), reader.GetInt32("MateID"), reader.GetBoolean("Cancelled"));

                    }
                }
                catch (SqlException sqlExp)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }
            return null;
        }

        public TrainingLaneBooking GetTrainingLaneBookingById(int id)
        {
            string GetUserLaneBookingByIdSQL = GetLaneBookingByIdSQL + " AND UserID IS NULL AND MateID IS NULL";
            using (SqlConnection connection = new SqlConnection(connectionString))
                try
                {
                    SqlCommand command = new SqlCommand(GetUserLaneBookingByIdSQL, connection);
                    command.Parameters.AddWithValue("@BookingID", id);
                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        return new TrainingLaneBooking(reader.GetInt32("LaneNumber"), reader.GetDateTime("DateStart"), reader.GetInt32("BookingID"), reader.GetBoolean("Cancelled"), trainingTeamService.GetTrainingTeamById(reader.GetInt32("TrainingTeamID")), reader.GetBoolean("Automatic"));
                    }

                }
                catch (SqlException sqlExp)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }
            return null;
        }

        public int DeleteAutomaticBookingOnTeam(int teamID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(DeleteAutomaticLaneBookings, connection);
                    command.Parameters.AddWithValue("@TrainingTeamID", teamID);
                    return command.ExecuteNonQuery();

                }
                catch (SqlException ex)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
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
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }
            return false;
        }

        public bool CreateLaneBooking(UserLaneBooking laneBooking)
        {
            //if (GetUserLaneBookingById(laneBooking.BookingID) is not null)
            //    return false;
            if (IsLaneBooked(laneBooking.LaneNumber, laneBooking.DateStart) != null)
            {
                throw new DuplicateBookingException($"Bane {laneBooking.LaneNumber} er allerede booket på det tidspunkt");
            }
            if (VerifyNewBooking(laneBooking) == false)
                return false;
            using (SqlConnection connection = new SqlConnection(connectionString))
                try
                {
                    SqlCommand command = new SqlCommand(CreateUserLaneBookingSQL, connection);
                    command.Parameters.AddWithValue("@LaneNumber", laneBooking.LaneNumber);
                    command.Parameters.AddWithValue("@DateStart", laneBooking.DateStart);
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
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }

            return false;
        }

        public bool CreateLaneBooking(TrainingLaneBooking laneBooking)
        {
            //if (GetTrainingLaneBookingById(laneBooking.BookingID) is not null)
            //    return false;
            if (IsLaneBooked(laneBooking.LaneNumber, laneBooking.DateStart) != null)
            {
                throw new DuplicateBookingException($"Bane {laneBooking.LaneNumber} er allerede booket på det tidspunkt");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
                try
                {
                    SqlCommand command = new SqlCommand(CreateTrainingLaneBookingSQL, connection);
                    command.Parameters.AddWithValue("@DateStart", laneBooking.DateStart);
                    command.Parameters.AddWithValue("@UserID", DBNull.Value);
                    command.Parameters.AddWithValue("@MateID", DBNull.Value);
                    command.Parameters.AddWithValue("@cancelled", laneBooking.Cancelled);
                    command.Parameters.AddWithValue("@TrainingTeamID", laneBooking.trainingTeam.TrainingTeamID);
                    command.Parameters.AddWithValue("@LaneNumber", laneBooking.LaneNumber);
                    command.Parameters.AddWithValue("@Automatic", laneBooking.Automatic);
                    command.Connection.Open();
                    int noOfRows = command.ExecuteNonQuery();
                    return noOfRows == 1;
                }
                catch (SqlException sqlExp)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }

            return false;
        }
        public Lane GetAnyFreeLane(DateTime time)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    Lane lane = null;
                    SqlCommand command = new SqlCommand(getFirstFreeLane, connection);
                    command.Parameters.AddWithValue("@DateStart", time);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        int laneNumber = reader.GetInt32("LaneNumber");
                        bool padel = reader.GetBoolean("PadelTennis");
                        bool outdoors = reader.GetBoolean("Outdoors");
                        lane = new Lane(laneNumber, outdoors, padel);
                    }
                    reader.Close();
                    return lane;
                }
                catch (SqlException sqlExp)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
        public int GetNoOfBookings(int laneID, DateTime minTime, DateTime maxTime)
        {
            if (minTime >= maxTime)
            {
                throw new ArgumentException("minTime kan ikke være større end maxTime");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(getNoOfBookings, connection);
                    command.Parameters.AddWithValue("@minTime", minTime);
                    command.Parameters.AddWithValue("@maxTime", maxTime);
                    command.Parameters.AddWithValue("@LaneNumber", laneID);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    int result = 0;
                    while (reader.Read())
                    {
                        result = reader.GetInt32("Bookings");
                    }
                    reader.Close();
                    return result;
                }
                catch (SqlException sqlExp)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
        public LaneBooking? IsLaneBooked(int laneID, DateTime time)
        {

            //LaneBooking? booking = GetAllLaneBookings<LaneBooking>().Find(b =>
            //{
            //    return laneID == b.LaneNumber && time == b.DateStart && !b.Cancelled;
            //});
            LaneBooking? booking = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(LaneBookingStatus, connection);
                    command.Parameters.AddWithValue("@Date", time);
                    command.Parameters.AddWithValue("@Lane", laneID);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    int result = 0;
                    while (reader.Read())
                    {
                        int? UserId = reader.GetIntOrNull("UserID");
                        if (UserId != null)
                        {
                            booking = new UserLaneBooking(reader.GetInt32("BookingID"), reader.GetInt32("LaneNumber"), reader.GetDateTime("DateStart"), (int)UserId, reader.GetInt32("MateID"), reader.GetBoolean("Cancelled"));
                        } else
                        {
                            booking = new TrainingLaneBooking(reader.GetInt32("LaneNumber"), reader.GetDateTime("DateStart"), reader.GetInt32("BookingID"), reader.GetBoolean("Cancelled"), new TrainingTeam(reader.GetInt32("TrainingTeamID")), reader.GetBoolean("Automatic"));
                        }
                    }
                    reader.Close();
                    return booking;
                }
                catch (SqlException sqlExp)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
        public List<LaneBooking> IsLaneBooked(List<int> laneIDs, DateTime time)
        {
            List<LaneBooking> bookings = GetAllLaneBookings<LaneBooking>().FindAll(b =>
            {
                return laneIDs.Contains(b.LaneNumber) && time == b.DateStart && !b.Cancelled;
            });
          
            return bookings;
        }
        public bool VerifyNewBooking(UserLaneBooking laneBooking)
        {
            List<UserLaneBooking> userLaneBookingList = GetAllLaneBookings<UserLaneBooking>();
            if (laneBooking.MateID == laneBooking.UserID)
                return false;
            IEnumerable<UserLaneBooking> UserAlredyBooked = from b in userLaneBookingList where laneBooking.DateStart == b.DateStart && laneBooking.UserID == b.UserID select b ;
            if (UserAlredyBooked.Count() > 0)
                return false;
            if (laneBooking.MateID > 0)
            {
                IEnumerable<UserLaneBooking> MateAlredyBooked = from b in userLaneBookingList where laneBooking.DateStart == b.DateStart && laneBooking.MateID == b.MateID select b;
                if (UserAlredyBooked.Count() > 0)
                    return false;

                Mate4HoursRule(laneBooking, userLaneBookingList);
            }
            IEnumerable<UserLaneBooking> AlreadyBooked = from b in userLaneBookingList where laneBooking.DateStart == b.DateStart && laneBooking.LaneNumber == b.LaneNumber select b;
            if (AlreadyBooked.Count() > 0)
                return false;

            User4HoursRule(laneBooking, userLaneBookingList);

            return true;
        }

        public bool User4HoursRule(UserLaneBooking laneBooking, List<UserLaneBooking> userLaneBookingList )
        {
            IEnumerable<UserLaneBooking> fourthMostRecentBookingUser = from b in userLaneBookingList where DateTime.Now <= b.DateStart && DateTime.Now.AddDays(14) >= b.DateStart && laneBooking.UserID == b.UserID orderby b.DateStart select b;
            List<UserLaneBooking> fourthMostRecentBookingUserList = fourthMostRecentBookingUser.ToList();
            return !(fourthMostRecentBookingUserList.Count >= 4);
        }

        public bool Mate4HoursRule(UserLaneBooking laneBooking, List<UserLaneBooking> userLaneBookingList)
        {
            IEnumerable<UserLaneBooking> fourthMostRecentBookingMate = from b in userLaneBookingList where DateTime.Now <= b.DateStart && DateTime.Now.AddDays(14) >= b.DateStart && laneBooking.MateID == b.MateID orderby b.DateStart select b;
            List<UserLaneBooking> fourthMostRecentBookingMateList = fourthMostRecentBookingMate.ToList();
            return !(fourthMostRecentBookingMateList.Count >= 4);
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
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }
            return false;
        }

        public bool EditLaneBooking(UserLaneBooking laneBooking, int id)
        {
            if (GetUserLaneBookingById(id) is null)
                return false;
            if (VerifyNewBooking(laneBooking) == false)
                return false;
            using (SqlConnection connection = new SqlConnection(connectionString))
                try
                {
                    SqlCommand command = new SqlCommand(UpdateLaneBookingSQL, connection);
                    command.Parameters.AddWithValue("@LaneNumber", laneBooking.LaneNumber);
                    command.Parameters.AddWithValue("@DateStart", laneBooking.DateStart);
                    command.Parameters.AddWithValue("@ID", laneBooking.Cancelled);
                    command.Connection.Open();
                    int noOfRows = command.ExecuteNonQuery();
                    return noOfRows == 1;
                }
                catch (SqlException sqlExp)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }

            return false;
        }

        public bool EditLaneBooking(TrainingLaneBooking laneBooking, int id)
        {
            throw new NotImplementedException();
        }
    }
}

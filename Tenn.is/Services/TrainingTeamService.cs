using Microsoft.Data.SqlClient;
using System.Data;
using System.Transactions;
using Tennis.Exceptions;
using Tennis.Helpers;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Services
{
    public class TrainingTeamService : Connection, ITrainingTeamService
    {
        private IUserService _userService;
        private ILaneBookingService _laneBookingService;
        private string SqlGetAllTrainingTeams = "SELECT *\r\nFROM TrainingTeams\r\nLEFT JOIN UsersAndTeams ON TrainingTeams.TrainingTeamID=UsersAndTeams.TrainingTeamID";

        private string insertTeamString = "INSERT INTO TrainingTeams (Title, MaxTrainees, SessionDay, WeeklySessionStart, WeeklySessionEnd, Description)\n" +
                                          "OUTPUT INSERTED.TrainingTeamID\n" +
                                          "VALUES (@Title, @MaxTrainees, @SessionDay, @WeeklySessionStart, @WeeklySessionEnd, @Description)";
        private string insertMembersString = "INSERT INTO UsersAndTeams (TrainingTeamID, UserID, IsTrainer)\n" +
                                             "VALUES (@TrainingTeamID, @UserID, @IsTrainer)";
        private string updateTeamString = "UPDATE TrainingTeams\n" +
                                          "SET Title = @Title, MaxTrainees = @MaxTrainees, SessionDay = @SessionDay, WeeklySessionStart = @WeeklySessionStart, WeeklySessionEnd = @WeeklySessionEnd, Description = @Description\n" +
                                          "WHERE TrainingTeamID = @TrainingTeamID";
        private string deleteMembersString = "DELETE FROM UsersAndTeams\n" +
                                             "WHERE TrainingTeamID = @TrainingTeamID";
        private string getTeamByID = "SELECT * FROM TrainingTeams\n" +
                                     "LEFT JOIN UsersAndTeams ON TrainingTeams.TrainingTeamID=UsersAndTeams.TrainingTeamID\n" +
                                     "WHERE TrainingTeams.TrainingTeamID = @TrainingTeamID";

        private string deleteByID = "DELETE FROM TrainingTeams\n" +
                                    "WHERE TrainingTeamID = @TrainingTeamID";
        public event Action<TrainingTeam> OnWeeklySessionEdit;
        public TrainingTeamService(IUserService userService)
        {
            connectionString = Secret.ConnectionString;
            _userService = userService;
            _laneBookingService = new LaneBookingService(this);
        }
        public TrainingTeamService(bool test, IUserService userService)
        {
            _userService = userService;
            
            if (test)
            {
                connectionString = Secret.ConnectionStringTest;
            }
            else
            {
                connectionString = Secret.ConnectionString;
            }
            _laneBookingService = new LaneBookingService(test, this);
        }
        public bool CreateTrainingTeam(TrainingTeam trainingTeam, int overrideBookings = 0)
        {
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                if (trainingTeam == null)
                {
                    return false;
                }

                try
                {
                    
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();
                    SqlCommand command = new SqlCommand(insertTeamString, connection, transaction);
                    
                    
                    command.Parameters.AddWithValue("@Title", trainingTeam.Title);
                    command.Parameters.AddWithValue("@MaxTrainees", trainingTeam.MaxTrainees);
                    command.Parameters.AddWithValueOrNull("@Description", trainingTeam.Description);
                    if (trainingTeam.weeklyTimeBetween != null)
                    {
                        command.Parameters.AddWithValue("@SessionDay", (int)trainingTeam.weeklyTimeBetween.StartDay);
                        command.Parameters.AddWithValue("@WeeklySessionStart", trainingTeam.weeklyTimeBetween.StartTime);
                        command.Parameters.AddWithValue("@WeeklySessionEnd", trainingTeam.weeklyTimeBetween.EndTime);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@SessionDay", DBNull.Value);
                        command.Parameters.AddWithValue("@WeeklySessionStart", DBNull.Value);
                        command.Parameters.AddWithValue("@WeeklySessionEnd", DBNull.Value);
                    }
                    int primaryKey = (int)command.ExecuteScalar();
                    
                    //foreach (User trainer in trainingTeam.Trainers)
                    //{
                    //    SqlCommand addMembersCommand = new SqlCommand(insertMembersString, connection, transaction);
                    //    addMembersCommand.Parameters.AddWithValue("@TrainingTeamID", primaryKey);
                    //    addMembersCommand.Parameters.AddWithValue("@UserID", trainer.UserId);
                    //    addMembersCommand.Parameters.AddWithValue("@IsTrainer", true);
                    //    addMembersCommand.ExecuteNonQuery();
                    //}
                    //foreach (User trainee in trainingTeam.Trainees)
                    //{
                    //    SqlCommand addMembersCommand = new SqlCommand(insertMembersString, connection, transaction);
                    //    addMembersCommand.Parameters.AddWithValue("@TrainingTeamID", primaryKey);
                    //    addMembersCommand.Parameters.AddWithValue("@UserID", trainee.UserId);
                    //    addMembersCommand.Parameters.AddWithValue("@IsTrainer", false);
                    //    addMembersCommand.ExecuteNonQuery();
                    //}
                    foreach (var kvp in trainingTeam.Members)
                    {
                        SqlCommand addMembersCommand = new SqlCommand(insertMembersString, connection, transaction);
                        addMembersCommand.Parameters.AddWithValue("@TrainingTeamID", primaryKey);
                        addMembersCommand.Parameters.AddWithValue("@UserID", kvp.Value.Item1.UserId);
                        addMembersCommand.Parameters.AddWithValue("@IsTrainer", kvp.Value.Item2);
                        addMembersCommand.ExecuteNonQuery();
                    }

                    //_ = (eventBooking.Comment.IsNullOrEmpty()) ? command.Parameters.AddWithValue("@Comment", DBNull.Value) : command.Parameters.AddWithValue("@Comment", eventBooking.Comment);
                    ////command.Parameters.AddWithValue("@Comment", eventBooking.Comment);

                    //OnCreate?.Invoke(GetEventBookingById(primaryKey));
                    transaction.Commit();
                    UpdateAutomaticBookingsInTeam(primaryKey, overrideBookings,3);                   
                    return true;

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

        public bool DeleteTrainingTeam(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(deleteByID, connection);
                    command.Parameters.AddWithValue("@TrainingTeamID", id);
                    if (command.ExecuteNonQuery() != 1)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }

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
            return false;
        }

        public bool EditTrainingTeam(TrainingTeam trainingTeam, int id, int overrideBookings = 0)
        {
          
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    
                    if (trainingTeam == null)
                    {
                        return false;
                    }
                    try
                    {
                        TrainingTeam beforeEdit = GetTrainingTeamById(id);
                        connection.Open();

                        SqlTransaction transaction = connection.BeginTransaction();
                        SqlCommand command = new SqlCommand(updateTeamString, connection, transaction);

                        command.Parameters.AddWithValue("@TrainingTeamID", id);
                        command.Parameters.AddWithValue("@Title", trainingTeam.Title);
                        command.Parameters.AddWithValue("@MaxTrainees", trainingTeam.MaxTrainees);
                        command.Parameters.AddWithValueOrNull("@Description", trainingTeam.Description);
                        if (trainingTeam.weeklyTimeBetween != null)
                        {
                            command.Parameters.AddWithValue("@SessionDay", (int)trainingTeam.weeklyTimeBetween.StartDay);
                            command.Parameters.AddWithValue("@WeeklySessionStart", trainingTeam.weeklyTimeBetween.StartTime);
                            command.Parameters.AddWithValue("@WeeklySessionEnd", trainingTeam.weeklyTimeBetween.EndTime);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@SessionDay", DBNull.Value);
                            command.Parameters.AddWithValue("@WeeklySessionStart", DBNull.Value);
                            command.Parameters.AddWithValue("@WeeklySessionEnd", DBNull.Value);
                        }

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected != 1)
                        {
                            return false;
                        }
                        SqlCommand deleteMembersCommand = new SqlCommand(deleteMembersString, connection, transaction);
                        deleteMembersCommand.Parameters.AddWithValue("@TrainingTeamID", id);
                        deleteMembersCommand.ExecuteNonQuery();
                        foreach (var kvp in trainingTeam.Members)
                        {
                            SqlCommand addMembersCommand = new SqlCommand(insertMembersString, connection, transaction);
                            addMembersCommand.Parameters.AddWithValue("@TrainingTeamID", id);
                            addMembersCommand.Parameters.AddWithValue("@UserID", kvp.Value.Item1.UserId);
                            addMembersCommand.Parameters.AddWithValue("@IsTrainer", kvp.Value.Item2);
                            addMembersCommand.ExecuteNonQuery();
                        }

                        //_ = (eventBooking.Comment.IsNullOrEmpty()) ? command.Parameters.AddWithValue("@Comment", DBNull.Value) : command.Parameters.AddWithValue("@Comment", eventBooking.Comment);
                        ////command.Parameters.AddWithValue("@Comment", eventBooking.Comment);

                        //OnCreate?.Invoke(GetEventBookingById(primaryKey));
                        transaction.Commit();
                        if (beforeEdit.weeklyTimeBetween != (trainingTeam.weeklyTimeBetween))
                        {
                            OnWeeklySessionEdit?.Invoke(GetTrainingTeamById(id));
                            UpdateAutomaticBookingsInTeam(id, overrideBookings, 3);
                        }
                        return true;

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

        public List<TrainingTeam> GetAllTrainingTeams()
        {
            List<TrainingTeam> trainingTeams = new List<TrainingTeam>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(SqlGetAllTrainingTeams, connection);
                    command.Connection.Open();
                    trainingTeams = ProcessReader(command);
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
            return trainingTeams;
        }

        public TrainingTeam GetTrainingTeamById(int id)
        {
            List<TrainingTeam> trainingTeams = new List<TrainingTeam>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(getTeamByID, connection);
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@TrainingTeamID", id);
                    trainingTeams = ProcessReader(command);
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
            return trainingTeams.FirstOrDefault();
        }
        private List<TrainingTeam> ProcessReader(SqlCommand command)
        {

            Dictionary<int, TrainingTeam> trainingTeams = new Dictionary<int, TrainingTeam>();
            SqlDataReader reader = command.ExecuteReader();
            HashSet<int> foundTeamIDs = new HashSet<int>();
            List<Tuple<int, User, bool>> UserIDsAndTeamIDs = new List<Tuple<int, User, bool>>();
            while (reader.Read())
            {
                
                int ID = reader.GetInt32("TrainingTeamID");
                if (foundTeamIDs.Add(ID)) 
                {
                    string title = reader.GetString("Title");
                    int maxTrainees = reader.GetInt32("MaxTrainees");
                    WeeklyTimeBetween time;
                    if (reader.IsDBNull("SessionDay") || reader.IsDBNull("WeeklySessionStart") || reader.IsDBNull("WeeklySessionEnd"))
                    {
                        time = null;
                    }
                    else
                    {
                        TimeOnly startTime = TimeOnly.FromTimeSpan(reader.GetTimeSpan("WeeklySessionStart"));
                        TimeOnly endTime = TimeOnly.FromTimeSpan(reader.GetTimeSpan("WeeklySessionEnd"));
                        DayOfWeek day = (DayOfWeek)reader.GetInt32("SessionDay");
                        time = new WeeklyTimeBetween(startTime, endTime, day);
                    }

                    string description = reader.GetStringOrNull(6);
                    trainingTeams.Add(ID, new TrainingTeam(ID, title, description, null, null, time, maxTrainees));
                }
                int? userID = reader.GetIntOrNull("UserID");
                
                if (userID != null)
                {
                    if (reader.GetBoolean("IsTrainer"))
                    {
                        UserIDsAndTeamIDs.Add(new Tuple<int, User, bool>(ID, new User((int)userID), true));
                        //trainingTeams[ID].AddTrainer(new User((int)userID));
                    }
                    else
                    {
                        UserIDsAndTeamIDs.Add(new Tuple<int, User, bool>(ID, new User((int)userID), false));
                        //trainingTeams[ID].AddTrainee(new User((int)userID));
                    }
                }
            }
            reader.Close();
            foreach (var tuple in UserIDsAndTeamIDs)
            {
                trainingTeams[tuple.Item1].AddMember(_userService.GetUserById(tuple.Item2.UserId), tuple.Item3);
            }
            return trainingTeams.Values.ToList();
        }
        public bool UpdateAutomaticBookingsInTeam(int teamID, int overrideBookings, int weekLimit)
        {
            // if overrideBookings is 0, it will not allow you to change the time if it would override bookings
            // if 1, it will book anything it can that isnt already booked
            //if 2, it will cancel all bookings that conflict
            //using (TransactionScope scope = new TransactionScope())
            //{
                
                    _laneBookingService.DeleteAutomaticBookingOnTeam(teamID);
                    TrainingTeam team = GetTrainingTeamById(teamID);
                    if (team != null && team.weeklyTimeBetween != null)
                    {
                        DateTime startDate = team.weeklyTimeBetween.NextStart;
                        for (int weeks = 0; weeks < weekLimit; weeks++)
                        {
                            for (int hour = team.weeklyTimeBetween.StartTime.Value.Hour; 
                                hour < team.weeklyTimeBetween.EndTime.Value.Hour; hour++)
                            {
                                DateTime bookingTime = startDate.AddDays(weeks * 7).
                                AddHours(hour - team.weeklyTimeBetween.StartTime.Value.Hour);                            
                                Lane freeLane = _laneBookingService.GetAnyFreeLane(bookingTime);
                                if (freeLane != null)
                                {
                                    TrainingLaneBooking booking = new TrainingLaneBooking(freeLane.Id,
                                                                 bookingTime, 0, false, team, true);
                                    _laneBookingService.CreateLaneBooking(booking);
                                }
                                else
                                {
                                    switch (overrideBookings)
                                    {
                                        case 0:
                                            throw new DuplicateBookingException("Der eksisterer allerede en booking på det tidspunkt");
                                        case 1:
                                            continue;
                                        case 2:
                                            LaneBooking? delBooking = _laneBookingService.GetAllLaneBookings<LaneBooking>().Find(b =>
                                            {
                                                return b.DateStart == bookingTime && !b.Cancelled;
                                            });
                                            if (delBooking != null)
                                            {
                                                _laneBookingService.CancelLaneBonking(delBooking.BookingID);
                                                TrainingLaneBooking booking = new TrainingLaneBooking(delBooking.LaneNumber, bookingTime, 0, false, team, true);
                                                _laneBookingService.CreateLaneBooking(booking);
                                            }
                                            else
                                            {
                                                throw new ArgumentNullException("delBooking er null");
                                            }
                                            break;
                                    }
                                }
                            }
                        }
                        //scope.Complete();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    
            //}                                  
        }
    }
}

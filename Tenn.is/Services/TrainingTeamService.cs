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
        private string SqlGetAllTrainingTeams = "SELECT *\r\nFROM TrainingTeams\r\nLEFT JOIN UsersAndTeams ON TrainingTeams.TrainingTeamID=UsersAndTeams.TrainingTeamID";

        private string insertTeamString = "INSERT INTO TrainingTeams (Title, MaxTrainees, SessionDay, WeeklySessionStart, WeeklySessionEnd, Description)\n" +
                                          "OUTPUT INSERTED.TrainingTeamID\n" +
                                          "VALUES (@Title, @MaxTrainees, @SessionDay, @WeeklySessionStart, @WeeklySessionEnd, @Description)";
        private string insertMembersString = "INSERT INTO UsersAndTeams (TrainingTeamID, UserID, IsTrainer)\n" +
                                             "VALUES (@TrainingTeamID, @UserID, @IsTrainer)";

        public TrainingTeamService(IUserService userService)
        {
            connectionString = Secret.ConnectionString;
            _userService = userService;
        }
        public TrainingTeamService(bool test, IUserService userService)
        {
            _userService = userService;
            if(test)
            {
                connectionString = Secret.ConnectionStringTest;
            }
            else
            {
                connectionString = Secret.ConnectionString;
            }
        }
        public bool CreateTrainingTeam(TrainingTeam trainingTeam)
        {
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                

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
                    
                    foreach (User trainer in trainingTeam.Trainers)
                    {
                        SqlCommand addMembersCommand = new SqlCommand(insertMembersString, connection, transaction);
                        addMembersCommand.Parameters.AddWithValue("@TrainingTeamID", primaryKey);
                        addMembersCommand.Parameters.AddWithValue("@UserID", trainer.UserId);
                        addMembersCommand.Parameters.AddWithValue("@IsTrainer", true);
                        addMembersCommand.ExecuteNonQuery();
                    }
                    foreach (User trainee in trainingTeam.Trainees)
                    {
                        SqlCommand addMembersCommand = new SqlCommand(insertMembersString, connection, transaction);
                        addMembersCommand.Parameters.AddWithValue("@TrainingTeamID", primaryKey);
                        addMembersCommand.Parameters.AddWithValue("@UserID", trainee.UserId);
                        addMembersCommand.Parameters.AddWithValue("@IsTrainer", false);
                        addMembersCommand.ExecuteNonQuery();
                    }

                    //_ = (eventBooking.Comment.IsNullOrEmpty()) ? command.Parameters.AddWithValue("@Comment", DBNull.Value) : command.Parameters.AddWithValue("@Comment", eventBooking.Comment);
                    ////command.Parameters.AddWithValue("@Comment", eventBooking.Comment);

                    //OnCreate?.Invoke(GetEventBookingById(primaryKey));
                    transaction.Commit();
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
            throw new NotImplementedException();
        }

        public bool EditTrainingTeam(TrainingTeam trainingTeam, int id)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
        private List<TrainingTeam> ProcessReader(SqlCommand command)
        {

            Dictionary<int, TrainingTeam> trainingTeams = new Dictionary<int, TrainingTeam>();
            SqlDataReader reader = command.ExecuteReader();
            HashSet<int> foundIDs = new HashSet<int>();
            while (reader.Read())
            {
                
                int ID = reader.GetInt32("TrainingTeamID");
                if (foundIDs.Add(ID)) 
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
                    User member = _userService.GetUserById((int)userID);
                    if (reader.GetBoolean("IsTrainer"))
                    {
                        trainingTeams[ID].Trainers.Add(member);
                    }
                    else
                    {
                        trainingTeams[ID].Trainees.Add(member);
                    }
                }
            }
            reader.Close();
            return trainingTeams.Values.ToList();
        }
    }
}

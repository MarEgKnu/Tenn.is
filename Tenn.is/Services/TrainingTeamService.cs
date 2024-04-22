using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Services
{
    public class TrainingTeamService : Connection, ITrainingTeamService
    {
        public TrainingTeamService()
        {
            connectionString = Secret.ConnectionString;
        }
        public TrainingTeamService(bool test)
        {
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public TrainingTeam GetTrainingTeamById(int id)
        {
            throw new NotImplementedException();
        }
    }
}

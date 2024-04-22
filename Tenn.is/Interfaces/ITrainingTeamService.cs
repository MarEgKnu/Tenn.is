using Tennis.Models;

namespace Tennis.Interfaces
{
    public interface ITrainingTeamService
    {
        bool CreateTrainingTeam(TrainingTeam trainingTeam);

        bool DeleteTrainingTeam(int id);

        bool EditTrainingTeam(TrainingTeam trainingTeam, int id);

        List<TrainingTeam> GetAllTrainingTeams();

        TrainingTeam GetTrainingTeamById(int id);

    }

}

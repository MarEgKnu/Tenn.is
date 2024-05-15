using Tennis.Models;

namespace Tennis.Interfaces
{
    public interface ITrainingTeamService
    {
        public event Action<TrainingTeam> OnWeeklySessionEdit;
        bool CreateTrainingTeam(TrainingTeam trainingTeam, int bookingOverride = 0);

        bool DeleteTrainingTeam(int id);

        bool EditTrainingTeam(TrainingTeam trainingTeam, int id, int bookingOverride = 0);

        List<TrainingTeam> GetAllTrainingTeams();

        TrainingTeam GetTrainingTeamById(int id);

    }

}

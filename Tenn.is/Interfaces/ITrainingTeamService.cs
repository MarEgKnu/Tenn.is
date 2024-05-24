using Tennis.Helpers;
using Tennis.Models;

namespace Tennis.Interfaces
{
    public interface ITrainingTeamService
    {
        public event Action<TrainingTeam> OnWeeklySessionEdit;
        bool CreateTrainingTeam(TrainingTeam trainingTeam, int bookingOverride = 0,int weekLimit = 3);

        bool DeleteTrainingTeam(int id);

        bool EditTrainingTeam(TrainingTeam trainingTeam, int id, int bookingOverride = 0, int weekLimit = 3);

        List<TrainingTeam> GetAllTrainingTeams();

        TrainingTeam GetTrainingTeamById(int id);

        bool UpdateAutomaticBookingsInTeam(int teamID, int overrideBookings, int weekLimit);

        List<DateTime>? CheckAutomaticBookingAvailability(WeeklyTimeBetween time, int weekLimit);

    }

}

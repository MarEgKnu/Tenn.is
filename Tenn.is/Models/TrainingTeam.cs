using Tennis.Helpers;

namespace Tennis.Models
{
    public class TrainingTeam
    {
        public int TrainingTeamID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<User> Trainers { get; set; }
        public List<User> Trainees { get; set; }
        public WeeklyTimeBetween weeklyTimeBetween { get; set; }
        public int MaxTrainees { get; set; }

        public TrainingTeam(int traningTeamID, string title, string description, List<User> trainers, List<User> trainees, WeeklyTimeBetween _weeklyTimeBetween, int maxTrainees)
        {
            TrainingTeamID = traningTeamID;
            Title = title;
            Description = description;
            Trainers = trainers;
            Trainees = trainees;
            weeklyTimeBetween = _weeklyTimeBetween;
            MaxTrainees = maxTrainees;
        }
    }
}

using System.ComponentModel.DataAnnotations;
using Tennis.Helpers;

namespace Tennis.Models
{
    public class TrainingTeam
    {
        public int TrainingTeamID { get; set; }
        [Required(ErrorMessage = "Titel er krævet"), MaxLength(60)]
        public string Title { get; set; }
        [MaxLength(2000)]
        public string Description { get; set; }
        public List<User> Trainers { get; set; }
        public List<User> Trainees { get; set; }
        public WeeklyTimeBetween? weeklyTimeBetween { get; set; }
        [Required(ErrorMessage = "Max antal medlemmer er krævet")]
        public int MaxTrainees { get; set; }

        public TrainingTeam(int traningTeamID, string title, string description, List<User> trainers, List<User> trainees, WeeklyTimeBetween _weeklyTimeBetween, int maxTrainees)
        {
            TrainingTeamID = traningTeamID;
            if (title == null)
            {
                throw new ArgumentNullException("Titel kan ikke være null");
            }
            Title = title;
            Description = description;
            Trainers = trainers ?? new List<User>();
            Trainees = trainees ?? new List<User>();
            weeklyTimeBetween = _weeklyTimeBetween;
            MaxTrainees = maxTrainees;
        }
        public TrainingTeam( string title, int maxTrainees)
        {
            if (title == null)
            {
                throw new ArgumentNullException("Titel kan ikke være null");
            }
            Title = title;
            MaxTrainees = maxTrainees;
        }
        public TrainingTeam()
        {
            TrainingTeamID = 0;
            Title = null;
            Description = null;
            Trainers = new List<User>();
            Trainees = new List<User>();
            weeklyTimeBetween = null;
            MaxTrainees = 0;
        }
    }
}

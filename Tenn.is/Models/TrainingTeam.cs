using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Eventing.Reader;
using Tennis.Exceptions;
using Tennis.Helpers;

namespace Tennis.Models
{
    public class TrainingTeam
    {
        public int TrainingTeamID { get; set; }
        [Required(ErrorMessage = "Titel er krævet"), MaxLength(60)]
        public string Title { get; set; }
        [MaxLength(2000)]
        public string? Description { get; set; }
        public List<User> Trainers { get
            {
                var list = new List<User>();
                foreach (var kvp in Members)
                {
                    if (kvp.Value.Item2)
                    {
                        list.Add(kvp.Value.Item1);
                    }
                }
                return list;
            } }
        public List<User> Trainees { get
            {
                var list = new List<User>();
                foreach (var kvp in Members)
                {
                    if (!kvp.Value.Item2)
                    {
                        list.Add(kvp.Value.Item1);
                    }
                }
                return list;
            } }
        public WeeklyTimeBetween? weeklyTimeBetween { get; set; }
        [Required(ErrorMessage = "Max antal medlemmer er krævet")]
        public int MaxTrainees { get; set; }

        private Dictionary<int, Tuple<User, bool>> _members;

        public Dictionary<int, Tuple<User, bool>> Members
        {
            get { return _members; }
            set { _members = value; }
        }



        public TrainingTeam(int traningTeamID, string title, string description, List<User> trainers, List<User> trainees, WeeklyTimeBetween _weeklyTimeBetween, int maxTrainees)
        {
            _members = new Dictionary<int, Tuple<User, bool>>();
            TrainingTeamID = traningTeamID;
            if (title == null)
            {
                throw new ArgumentNullException("Titel kan ikke være null");
            }
            Title = title;
            Description = description;
            if (trainers != null)
            {
                foreach (var trainer in trainers)
                {
                    AddTrainer(trainer);
                }
            }
            if (trainees != null)
            {
                foreach (var trainee in trainees)
                {
                    AddTrainee(trainee);
                }
            }
            weeklyTimeBetween = _weeklyTimeBetween;
            MaxTrainees = maxTrainees;
        }
        public TrainingTeam( string title, int maxTrainees)
        {
            _members = new Dictionary<int, Tuple<User, bool>>();
            if (title == null)
            {
                throw new ArgumentNullException("Titel kan ikke være null");
            }
            Title = title;
            MaxTrainees = maxTrainees;
        }
        public TrainingTeam()
        {
            _members = new Dictionary<int, Tuple<User, bool>>();
            TrainingTeamID = 0;
            Title = null;
            Description = null;
            new WeeklyTimeBetween(new TimeOnly(6, 0), new TimeOnly(7, 0), DayOfWeek.Monday);
            MaxTrainees = 0;
        }
        public bool AddTrainer(User user)
        {
            if (user == null)
            {
                return false;
            }
            if (!_members.TryAdd(user.UserId, new Tuple<User, bool>(user, true)))
            {
                throw new DuplicateUserException("Den samme bruger kan ikke være i en træningshold flere gange");
            }
            else
            {
                return true;
            }

        }
        public bool AddTrainee(User user)
        {
            if (user == null)
            {
                return false;
            }
            if (!_members.TryAdd(user.UserId, new Tuple<User, bool>(user, false)))
            {
                throw new DuplicateUserException("Den samme bruger kan ikke være i en træningshold flere gange");
            }
            else
            {
                return true;
            }
        }
    }
}

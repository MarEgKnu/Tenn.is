using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tennis.Exceptions;
using Tennis.Helpers;
using Tennis.Interfaces;
using Tennis.Models;
using Tennis.Services;

namespace TennisTest
{
    [TestClass]
    public class ITrainingTeamServiceTest
    {
        private ITrainingTeamService trainingTeamService;
        private IUserService userService;

        void TestSetUp()
        {
            userService = new UserService(true);
            trainingTeamService = new TrainingTeamService(true, userService);
          

            using (SqlConnection conn = new SqlConnection(Secret.ConnectionStringTest))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM TrainingTeams", conn);
                SqlCommand cmd2 = new SqlCommand("DELETE FROM Users", conn);


                cmd.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
            }
        }
        [TestMethod]
        public void GetAllTrainingTeams_0Teams()
        {
            TestSetUp();
            int count = trainingTeamService.GetAllTrainingTeams().Count;
            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void CreateTrainingTeam_Sucess_partial()
        {
            TestSetUp();
            TrainingTeam trainingTeam = new TrainingTeam(1, "Jeff's træningshold", null, null, null, null, 5);
            trainingTeamService.CreateTrainingTeam(trainingTeam);
            trainingTeam = trainingTeamService.GetAllTrainingTeams().FirstOrDefault();
            Assert.AreEqual(1, trainingTeamService.GetAllTrainingTeams().Count);
            Assert.AreEqual("Jeff's træningshold", trainingTeam.Title);
            Assert.AreEqual(5, trainingTeam.MaxTrainees);
            Assert.IsNull(trainingTeam.weeklyTimeBetween);
        }
        [TestMethod]
        public void CreateTrainingTeam_Sucess_full()
        {
            TestSetUp();
            TrainingTeam trainingTeam = new TrainingTeam(1, "Jeff's træningshold", "stort træningshold", new List<User>(), new List<User>(), new WeeklyTimeBetween(new TimeOnly(5,0), new TimeOnly(6, 0), DayOfWeek.Thursday), 5);
            trainingTeamService.CreateTrainingTeam(trainingTeam);
            trainingTeam = trainingTeamService.GetAllTrainingTeams().FirstOrDefault();
            Assert.AreEqual(1, trainingTeamService.GetAllTrainingTeams().Count);
            Assert.AreEqual("Jeff's træningshold", trainingTeam.Title);
            Assert.AreEqual(5, trainingTeam.MaxTrainees);
            Assert.AreEqual(5, trainingTeam.weeklyTimeBetween.StartTime.Value.Hour);
            Assert.AreEqual(0, trainingTeam.Trainees.Count);
            Assert.AreEqual(0, trainingTeam.Trainers.Count);
        }
        [TestMethod]
        public void CreateTrainingTeam_Sucess_full_1Trainer_1Trainee()
        {
            TestSetUp();
            userService.CreateUser(new User(1, "john1", "John", "Jensen", "jensen@gmail.com", "xxxjensenxxx", "41414141", false, false));
            userService.CreateUser(new User(2, "john2", "John", "Jensen", "jensen@gmail.com", "xxxjensenxxx", "41414141", false, false));
            List<User> users = userService.GetAllUsers();
            TrainingTeam trainingTeam = new TrainingTeam(1, "Jeff's træningshold", "stort træningshold", new List<User>() { users[0] }, new List<User>() { users[1] }, new WeeklyTimeBetween(new TimeOnly(5, 0), new TimeOnly(6, 0), DayOfWeek.Thursday), 5);
            trainingTeamService.CreateTrainingTeam(trainingTeam);
            trainingTeam = trainingTeamService.GetAllTrainingTeams().FirstOrDefault();
            Assert.AreEqual(1, trainingTeamService.GetAllTrainingTeams().Count);
            Assert.AreEqual("Jeff's træningshold", trainingTeam.Title);
            Assert.AreEqual(5, trainingTeam.MaxTrainees);
            Assert.AreEqual(5, trainingTeam.weeklyTimeBetween.StartTime.Value.Hour);
            Assert.IsTrue(users[0].Equals( trainingTeam.Trainers.First()));
            Assert.IsTrue(users[1].Equals(trainingTeam.Trainees.First()));
        }
        [ExpectedException(typeof(DuplicateUserException))]
        [TestMethod]
        public void CreateTrainingTeam_Failure_full_1Trainer_1Trainee_SameID()
        {
            TestSetUp();
            userService.CreateUser(new User(1, "john1", "John", "Jensen", "jensen@gmail.com", "xxxjensenxxx", "41414141", false, false));
            userService.CreateUser(new User(2, "john2", "John", "Jensen", "jensen@gmail.com", "xxxjensenxxx", "41414141", false, false));
            List<User> users = userService.GetAllUsers();
            TrainingTeam trainingTeam = new TrainingTeam(1, "Jeff's træningshold", "stort træningshold", new List<User>() { users[0] }, new List<User>() { users[0] }, new WeeklyTimeBetween(new TimeOnly(5, 0), new TimeOnly(6, 0), DayOfWeek.Thursday), 5);
            trainingTeamService.CreateTrainingTeam(trainingTeam);
            trainingTeam = trainingTeamService.GetAllTrainingTeams().FirstOrDefault();
            Assert.AreEqual(1, trainingTeamService.GetAllTrainingTeams().Count);
            Assert.AreEqual("Jeff's træningshold", trainingTeam.Title);
            Assert.AreEqual(5, trainingTeam.MaxTrainees);
            Assert.AreEqual(5, trainingTeam.weeklyTimeBetween.StartTime.Value.Hour);
            Assert.IsTrue(users[0].Equals(trainingTeam.Trainers.First()));
            Assert.IsTrue(users[1].Equals(trainingTeam.Trainees.First()));
        }
    }
}

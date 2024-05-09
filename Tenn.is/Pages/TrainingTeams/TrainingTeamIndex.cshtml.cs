using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Tennis.Interfaces;
using Tennis.Models;
using Tennis.Services;

namespace Tennis.Pages.TrainingTeams
{
    public class TrainingTeamIndexModel : PageModel
    {
        public User LoggedInUser { get; set; }
        public List<TrainingTeam> Teams { get; set; }
        private ITrainingTeamService _trainingTeamService;
        private IUserService _userService;
        public TrainingTeamIndexModel(ITrainingTeamService trainingTeamService, IUserService userService)
        {
            _trainingTeamService = trainingTeamService;
            _userService = userService;

        }
        public void OnGet()
        {
            LoggedInUser = _userService.VerifyUser(HttpContext.Session.GetString("Username"), HttpContext.Session.GetString("Password"));
            try
            {
                Teams = _trainingTeamService.GetAllTrainingTeams();


            }
            catch (SqlException sqlEx)
            {
                Teams = new List<TrainingTeam>();
                ViewData["ErrorMessage"] = "Database fejl. Fejlbesked:\n" + sqlEx.Message;
            }
            catch (Exception ex)
            {
                Teams = new List<TrainingTeam>();
                ViewData["ErrorMessage"] = "Generel fejl. Fejlbesked:\n" + ex.Message;
            }
        }
    }
}

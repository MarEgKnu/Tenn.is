using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Tennis.Exceptions;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Pages.TrainingTeams
{
    public class TrainingTeamRegistrationModel : PageModel
    {
        private IUserService _userService;
        private ITrainingTeamService _trainingTeamService;
        public TrainingTeamRegistrationModel(IUserService userService, ITrainingTeamService trainingTeamService)
        {
            _userService = userService;
            _trainingTeamService = trainingTeamService;
        }
        public TrainingTeam Team {  get; set; }
        public User LoggedInUser { get; set; }
        public IActionResult OnGet(int trainingTeamID)
        {
            LoggedInUser = _userService.VerifyUser(HttpContext.Session.GetString("Username"), HttpContext.Session.GetString("Password"));
            if (LoggedInUser == null)
            {
                return RedirectToPage("/Users/Login", "Redirect", new { message = "Du skal være logget ind for at kunne se denne side" });
            }
            else if (LoggedInUser.IsUtilityUser)
            {
                return RedirectToPage("/Users/Login", "Redirect", new { message = "Du kan ikke se denne side som denne bruger" });
            }
            try
            {
                Team = _trainingTeamService.GetTrainingTeamById(trainingTeamID);
                if (Team == null)
                {
                    ViewData["ErrorMessage"] = "Fejl. Kunne ikke finde træningshold";
                }
                else if (Team.AtCapacity)
                {
                    ViewData["ErrorMessage"] = "Holdet har ikke plads til flere deltagere";
                }
            }
            catch (SqlException sqlEx)
            {
                ViewData["ErrorMessage"] = "Database fejl. Fejlbesked:\n" + sqlEx.Message;
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = "Generel fejl. Fejlbesked:\n" + ex.Message;
            }
            return Page();
        }
        public IActionResult OnPost(int trainingTeamID)
        {
            LoggedInUser = _userService.VerifyUser(HttpContext.Session.GetString("Username"), HttpContext.Session.GetString("Password"));
            if (LoggedInUser == null)
            {
                return RedirectToPage("/Users/Login", "Redirect", new { message = "Du skal være logget ind for at kunne se denne side" });
            }
            else if (LoggedInUser.IsUtilityUser)
            {
                return RedirectToPage("/Users/Login", "Redirect", new { message = "Du kan ikke se denne side som denne bruger" });
            }
            try
            {
                Team = _trainingTeamService.GetTrainingTeamById(trainingTeamID);
                if (Team == null)
                {
                    ViewData["ErrorMessage"] = "Fejl. Kunne ikke finde træningshold";
                    return Page();
                }
                else if (Team.AtCapacity)
                {
                    ViewData["ErrorMessage"] = "Holdet har ikke plads til flere deltagere";
                    return Page();
                }
                Team.AddTrainee(LoggedInUser);
                _trainingTeamService.EditTrainingTeam(Team, trainingTeamID);
            }
            catch (SqlException sqlEx)
            {
                ViewData["ErrorMessage"] = "Database fejl. Fejlbesked:\n" + sqlEx.Message;
                return Page();
            }
            catch (DuplicateUserException ex)
            {
                ViewData["ErrorMessage"] = "Fejl: " + ex.Message;
                return Page();
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = "Generel fejl. Fejlbesked:\n" + ex.Message;
                return Page();
            }
            return RedirectToPage("TrainingTeamIndex");
        }
        public IActionResult OnPostCancel(int trainingTeamID)
        {
            return RedirectToPage("TrainingTeamIndex");
        }
    }
}

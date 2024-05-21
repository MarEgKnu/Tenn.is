using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Tennis.Interfaces;
using Tennis.Models;
using Tennis.Services;

namespace Tennis.Pages.TrainingTeams
{
    public class DeleteTrainingTeamModel : PageModel
    {
        public DeleteTrainingTeamModel(ITrainingTeamService trainingTeamService, IUserService userService)
        {
            _trainingTeamService = trainingTeamService;
            _userService = userService;
        }
        private ITrainingTeamService _trainingTeamService { get; set; }
        private IUserService _userService { get; set; }
        public TrainingTeam Team { get; set; }
        public IActionResult OnGet(int trainingTeamID)
        {
            if (!_userService.AdminVerify(HttpContext.Session.GetString("Username"), HttpContext.Session.GetString("Password")))
            {
                return RedirectToPage("/Users/Login", "Redirect", new { message = "Du har ikke tilladelse til at se denne side. Log venligst ind som admin" });
            }
            try
            {
                Team = _trainingTeamService.GetTrainingTeamById(trainingTeamID);
            }
            catch (SqlException sqlEx)
            {
                ViewData["ErrorMessage"] = "Database fejl. Fejlbesked:\n " + sqlEx.Message;
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = "Generel fejl. Fejlbesked:\n " + ex.Message;
            }
            return Page();
        }
        public IActionResult OnPost(int trainingTeamID)
        {
            if (!_userService.AdminVerify(HttpContext.Session.GetString("Username"), HttpContext.Session.GetString("Password")))
            {
                return RedirectToPage("/Users/Login", "Redirect", new { message = "Du har ikke tilladelse til at se denne side. Log venligst ind som admin" });
            }
            try
            {
                bool sucess = _trainingTeamService.DeleteTrainingTeam(trainingTeamID);
                if (!sucess)
                {
                    ViewData["ErrorMessage"] = "Træningshold kunne ikke slettes. Kontakt en administrator.";
                    Team = _trainingTeamService.GetTrainingTeamById(trainingTeamID);
                    return Page();
                }
            }
            catch (SqlException sqlEx)
            {
                ViewData["ErrorMessage"] = "Database fejl. Fejlbesked:\n " + sqlEx.Message;
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = "Generel fejl. Fejlbesked:\n " + ex.Message;
            }
            return RedirectToPage("TrainingTeamIndex");
        }
        public IActionResult OnPostCancel(int trainingTeamID)
        {
            return RedirectToPage("TrainingTeamIndex");
        }
    }
}

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
        [BindProperty(SupportsGet = true)]
        public bool AdvancedSearch { get; set; }
        [BindProperty(SupportsGet = true)]
        public string GenericFilter { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; }
        [BindProperty(SupportsGet = true)]
        public string PrevSortBy { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool Descending {  get; set; }
        [BindProperty(SupportsGet = true)]
        public int? IDFilter { get; set; }
        [BindProperty(SupportsGet = true)]
        public string TitleFilter { get; set; }
        [BindProperty(SupportsGet = true)] 
        public string DescriptionFilter { get; set; }
        private ITrainingTeamService _trainingTeamService;
        private IUserService _userService;
        public TrainingTeamIndexModel(ITrainingTeamService trainingTeamService, IUserService userService)
        {
            _trainingTeamService = trainingTeamService;
            _userService = userService;

        }
        public IActionResult OnGet()
        {
            LoggedInUser = _userService.VerifyUser(HttpContext.Session.GetString("Username"), HttpContext.Session.GetString("Password"));
            if (LoggedInUser == null)
            {
                return RedirectToPage("/Users/Login", "Redirect", new { message = "Du har ikke tilladelse til at se denne side. Log venligst ind som admin eller træner" });
            }
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
            return Page();
        }
    }
}

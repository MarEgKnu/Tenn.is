using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;
using Tennis.Helpers;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Pages.TrainingTeams
{
    public class CreateTrainingTeamModel : PageModel
    {
        [BindProperty(SupportsGet =true)]
        public TrainingTeam Team { get; set; }
        public SelectList DayOptions { get; set; }
        //[BindProperty]
        //public string TrainerUserName { get; set; }
        //[BindProperty]
        //public string TraineeUserName { get; set; }

        public Dictionary<int, User> ValidUsers { get; set; }
        [BindProperty(SupportsGet =true)]
        public List<int> SelectedTrainerIDs { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<int> SelectedTraineeIDs { get; set; }

        [BindProperty(SupportsGet = true)]
        public List<int> ExistingTraineeIDs { get; set; }

        [BindProperty(SupportsGet = true)]
        public List<int> ExistingTrainerIDs { get; set; }


        [BindProperty(SupportsGet = true)]
        public string MemberFilter { get; set; }

        public List<SelectListItem> SelectUser { get; set; }




        private ITrainingTeamService _teamService;
        private IUserService _userService;

        public CreateTrainingTeamModel(ITrainingTeamService teamService, IUserService userService)
        {
            _teamService = teamService;
            DayOptions = new SelectList(LaneBookingHelpers.DayOptions, "Key", "Value");
            _userService = userService;
            ValidUsers = _userService.GetAllUsers().ToDictionary(user => user.UserId, user => user);
            SelectedTrainerIDs = new List<int>();
            SelectedTraineeIDs = new List<int>();

            ExistingTrainerIDs = new List<int>();
            ExistingTraineeIDs = new List<int>();
            SelectUser = ValidUsers.Select(kvp => new SelectListItem($"{kvp.Value.Username} : {kvp.Value.FirstName} {kvp.Value.LastName}", kvp.Value.UserId.ToString())).ToList();

            Team = new TrainingTeam(0,"",null,null,null,new WeeklyTimeBetween(),0);
        }
        public IActionResult OnGet()
        {
            if (!_userService.AdminVerify(HttpContext.Session.GetString("Username"), HttpContext.Session.GetString("Password")))
            {
                return RedirectToPage("/Users/Login", "Redirect", new { message = "Du har ikke tilladelse til at se denne side. Log venligst ind som admin" });
            }
            FilterMembers();
            return Page();
            
        }
        public IActionResult OnPost()
        {
            if (!_userService.AdminVerify(HttpContext.Session.GetString("Username"), HttpContext.Session.GetString("Password")))
            {
                return RedirectToPage("/Users/Login", "Redirect", new { message = "Du har ikke tilladelse til at se denne side. Log venligst ind som admin" });
            }
            if (Team.weeklyTimeBetween.EndTime == null ||
                Team.weeklyTimeBetween.StartTime == null ||
                Team.weeklyTimeBetween.StartDay == null)
            {
                Team.weeklyTimeBetween = null;
            }
            else
            {
                string? error = Team.weeklyTimeBetween.IsValidForLaneBooking;
                if (error != null)
                {
                    ModelState.AddModelError("Team.weeklyTimeBetween.EndTime", error);
                }
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }
            return Page();
        }
        public IActionResult OnPostAddTrainer()
        {
            //if (!_userService.AdminVerify(HttpContext.Session.GetString("Username"), HttpContext.Session.GetString("Password")))
            //{
            //    return RedirectToPage("/Users/Login", "Redirect", new { message = "Du har ikke tilladelse til at se denne side. Log venligst ind som admin" });
            //}
            //User user = _userService.GetUserByUserName(TrainerUserName);
            //if (user == null)
            //{
            //    ViewData["ErrorMessage"] = "Ukendt bruger, vælg venlist en bruger i listen";
            //    return Page();
            //}
            //TrainerIDs.Add(user.UserId);
            OnGet();
            foreach (var id in SelectedTrainerIDs)
            {
                if (!ExistingTrainerIDs.Contains(id))
                {
                    ExistingTrainerIDs.Add(id);
                }
                
            }
            return Page();
        }
        private void FilterMembers()
        {
            if (!MemberFilter.IsNullOrEmpty())
            {
                foreach (var item in ValidUsers.Where(kvp => !kvp.Value.Username.Contains(MemberFilter)))
                {
                    ValidUsers.Remove(item.Key);
                }
            
            }
        }
    }
}

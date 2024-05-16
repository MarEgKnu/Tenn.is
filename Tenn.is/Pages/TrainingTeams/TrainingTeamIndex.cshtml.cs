using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Tennis.Helpers;
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
        [BindProperty(SupportsGet = true)]
        public DayOfWeek? DayFilter { get; set; }
        [BindProperty(SupportsGet = true)]
        public TimeOnly? TimeFilter { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool IsMemberOfFilter { get; set; }

        public SelectList DaySearchOptions { get; set; }
        private ITrainingTeamService _trainingTeamService;
        private IUserService _userService;
        public TrainingTeamIndexModel(ITrainingTeamService trainingTeamService, IUserService userService)
        {
            _trainingTeamService = trainingTeamService;
            _userService = userService;
            DaySearchOptions = new SelectList(LaneBookingHelpers.DayOptions, "Key", "Value");

        }
        public IActionResult OnGet(bool sorting)
        {
            LoggedInUser = _userService.VerifyUser(HttpContext.Session.GetString("Username"), HttpContext.Session.GetString("Password"));
            if (LoggedInUser == null)
            {
                return RedirectToPage("/Users/Login", "Redirect", new { message = "Du har ikke tilladelse til at se denne side. Log venligst ind som admin eller træner" });
            }
            if (sorting)
            {
                if (SortBy == PrevSortBy)
                {
                    Descending = !Descending;
                }
                else
                {
                    Descending = false;
                }
                PrevSortBy = SortBy;
            }
            try
            {
                GetAndFilterTeams();
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
            SortTeams();
            return Page();
        }
        public IActionResult OnGetSort()
        {
            LoggedInUser = _userService.VerifyUser(HttpContext.Session.GetString("Username"), HttpContext.Session.GetString("Password"));
            if (LoggedInUser == null)
            {
                return RedirectToPage("/Users/Login", "Redirect", new { message = "Du har ikke tilladelse til at se denne side. Log venligst ind som admin eller træner" });
            }
            if (SortBy == PrevSortBy)
            {
                Descending = !Descending;
            }
            else
            {
                Descending = false;
            }
            PrevSortBy = SortBy;
            try
            {
                GetAndFilterTeams();
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
        private void FilterTeamsBasic()
        {
            List<Predicate<TrainingTeam>> conditions = new List<Predicate<TrainingTeam>>();
            if (!GenericFilter.IsNullOrEmpty())
            {
                
                conditions.Add(t =>
                {
                    if (t.Description != null)
                    {
                        return t.Description.ToLower().Contains(GenericFilter.ToLower()) ||
                        t.Title.ToLower().Contains(GenericFilter.ToLower());
                    }
                    else
                    {
                        return t.Title.ToLower().Contains(GenericFilter.ToLower());
                    }
                });
            }
            if (IsMemberOfFilter)
            {
                conditions.Add(t => t.IsMember(LoggedInUser));
            }
            Teams = FilterHelpers.GetItemsOnConditions(conditions, Teams);
        }
        private void FilterTeamsAdvanced()
        {
            List<Predicate<TrainingTeam>> conditions = new List<Predicate<TrainingTeam>>();
            if (IsMemberOfFilter)
            {
                conditions.Add(t => t.IsMember(LoggedInUser));
            }
            if (IDFilter != null)
            {
                conditions.Add(t => t.TrainingTeamID == IDFilter);
            }
            if (!TitleFilter.IsNullOrEmpty())
            {
                conditions.Add(t => t.Title.Contains(TitleFilter));
            }
            if (!DescriptionFilter.IsNullOrEmpty())
            {

                conditions.Add(t =>
                {
                    if (t.Description != null)
                    {
                        return t.Description.Contains(DescriptionFilter); ;
                    }
                    else
                    {
                        return true;
                    }
                });
            }
            if (DayFilter != null)
            {
                conditions.Add(t =>
                {
                    if (t.weeklyTimeBetween != null && t.weeklyTimeBetween.StartDay != null)
                    {
                        return t.weeklyTimeBetween.StartDay == DayFilter;
                    }
                    else if (t.weeklyTimeBetween == null && TimeFilter != null)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                });
            }
            if (TimeFilter != null)
            {
                conditions.Add(t =>
                {
                    if (t.weeklyTimeBetween != null && t.weeklyTimeBetween.StartTime != null && t.weeklyTimeBetween.EndTime != null)
                    {
                        return t.weeklyTimeBetween.TimeStateAt(TimeFilter.Value) == RelativeTime.Ongoing;
                       
                    }
                    else
                    {
                        return true;
                    }
                });
            }
            Teams = FilterHelpers.GetItemsOnConditions(conditions, Teams);
        }
        private void GetAndFilterTeams()
        {
            Teams = _trainingTeamService.GetAllTrainingTeams();
            if (AdvancedSearch)
            {
                FilterTeamsAdvanced();
            }
            else
            {
                FilterTeamsBasic();
            }
        }
        private void SortTeams()
        {
            if (Descending)
            {
                switch (SortBy)
                {
                    case "TrainingTeamID":
                        Teams = Teams.OrderByDescending(t => t.TrainingTeamID).ToList();
                        break;
                    case "Title":
                        Teams = Teams.OrderByDescending(t => t.Title).ToList();
                        break;
                    case "Description":
                        Teams = Teams.OrderByDescending(t => t.Description).ToList();
                        break;
                    case "MaxTrainees":
                        Teams = Teams.OrderByDescending(t => t.MaxTrainees).ToList();
                        break;
                    case "weeklyTimeBetween":
                        Teams = Teams.OrderByDescending(t => t.weeklyTimeBetween).ToList();   
                        break;
                    case "Trainers":
                        Teams = Teams.OrderByDescending(t => t.Trainers.Count).ToList();
                        break;
                    case "Trainees":
                        Teams = Teams.OrderByDescending(t => t.Trainees.Count).ToList();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (SortBy)
                {
                    case "TrainingTeamID":
                        Teams = Teams.OrderBy(t => t.TrainingTeamID).ToList();
                        break;
                    case "Title":
                        Teams = Teams.OrderBy(t => t.Title).ToList();
                        break;
                    case "Description":
                        Teams = Teams.OrderBy(t => t.Description).ToList();
                        break;
                    case "MaxTrainees":
                        Teams = Teams.OrderBy(t => t.MaxTrainees).ToList();
                        break;
                    case "weeklyTimeBetween":
                        Teams = Teams.OrderBy(t => t.weeklyTimeBetween).ToList();
                        break;
                    case "Trainers":
                        Teams = Teams.OrderBy(t => t.Trainers.Count).ToList();
                        break;
                    case "Trainees":
                        Teams = Teams.OrderBy(t => t.Trainees.Count).ToList();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}

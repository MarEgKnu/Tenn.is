using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Pages.Lanes
{
    public class BookingCreateTrainingModel : PageModel
    {
        private IUserService _userService;
        private ILaneService _laneService;
        private ITrainingTeamService _trainingTeamService;
        private ILaneBookingService _laneBookingService;
        public BookingCreateTrainingModel(IUserService userService, ILaneService laneService, ITrainingTeamService trainingTeamService, ILaneBookingService laneBookingService)
        {
            _userService = userService;
            _laneService = laneService;
            _trainingTeamService = trainingTeamService;
            _laneBookingService = laneBookingService;
        }
        public Lane Lane {  get; set; }
        public TrainingTeam Team {  get; set; }
        public User LoggedInUser { get; set; }
        public string DateString {  get; set; }
        public DateTime? SelectedDay
        {
            get
            {
                bool succeeded = DateTime.TryParse(DateString, out DateTime result);
                if (succeeded)
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            set { SelectedDay = value; }
        }
        public IActionResult OnGet(int laneid, string datetime, int trainingTeamID)
        {
            try
            {
                Team = _trainingTeamService.GetTrainingTeamById(trainingTeamID);
                LoggedInUser = _userService.VerifyUser(HttpContext.Session.GetString("Username"), HttpContext.Session.GetString("Password"));
                Lane = _laneService.GetLaneByNumber(laneid);
            }
            catch (SqlException sqlExp)
            {
                ViewData["ErrorMessage"] = "Databasefejl: " + sqlExp.Message;
                return Page();
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = "Generel fejl: " + ex.Message;
                return Page();
            }
            
            if (Lane == null || Team  == null)
            {
                ViewData["ErrorMessage"] = "Fejl. Bane eller hold kunne ikke findes";
                return Page();
            }
            if (LoggedInUser == null || (!Team.IsTrainer(LoggedInUser) && !LoggedInUser.Administrator ))
            {
                return RedirectToPage("/Users/Login", "Redirect", new { message = "Du har ikke tilladelse til at se denne side. Log venligst ind som admin eller træner" });
            }
            DateString = datetime;
            return Page();
        }
        public IActionResult OnPost(int laneid, string datetime, int trainingTeamID)
        {
            try
            {
                Team = _trainingTeamService.GetTrainingTeamById(trainingTeamID);
                LoggedInUser = _userService.VerifyUser(HttpContext.Session.GetString("Username"), HttpContext.Session.GetString("Password"));
                Lane = _laneService.GetLaneByNumber(laneid);
                if (Lane == null || Team == null)
                {
                    ViewData["ErrorMessage"] = "Fejl. Bane eller hold kunne ikke findes";
                    return Page();
                }
                if (LoggedInUser == null || (!Team.IsTrainer(LoggedInUser) && !LoggedInUser.Administrator))
                {
                    return RedirectToPage("/Users/Login", "Redirect", new { message = "Du har ikke tilladelse til at se denne side. Log venligst ind som admin eller træner" });
                }
                DateString = datetime;
                _laneBookingService.CreateLaneBooking(new TrainingLaneBooking(laneid, SelectedDay.Value, 0, false, Team, false));
            }
            catch (SqlException sqlExp)
            {
                ViewData["ErrorMessage"] = "Databasefejl: " + sqlExp.Message;
                return Page();
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = "Generel fejl: " + ex.Message;
                return Page();
            }
            return RedirectToPage(("BookingOverview", "Redirect", new { laneid = laneid, datetime = datetime, trainingTeamID = trainingTeamID}));

        }
        public IActionResult OnPostCancel(int laneid, string datetime, int trainingTeamID)
        {
            return RedirectToPage(("BookingOverview", "Redirect", new { laneid = laneid, datetime = datetime, trainingTeamID = trainingTeamID }));
        }
    }
}

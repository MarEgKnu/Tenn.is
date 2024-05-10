using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Pages.Lanes
{
    public class BookingCreateModel : PageModel
    {
        private ILaneBookingService _bookingService;
        private IUserService _userService;
        private ILaneService _laneService;

        public List<UserLaneBooking> ExistingBookings { get; set; }
        [BindProperty]
        public UserLaneBooking CurrentBooking { get; set; }
        public Lane LaneInfo { get; set; }
        public User CurrentUser { get; set; }
        public List<User> AvailableUsers { get; set; }

        [BindProperty(SupportsGet = true)]
        public string DateString { get; set; }
        [BindProperty(SupportsGet =true)]
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
            } set { SelectedDay = value; }
        }

        public BookingCreateModel(ILaneBookingService bookingService, ILaneService laneService, IUserService userService)
        { 
            _bookingService = bookingService;
            _laneService = laneService;
            _userService = userService;
        }
        public IActionResult OnGet(int laneid, string datetime)
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToPage("/Users/Login", "Redirect", new { message = "Du skal være logget ind for at booke en bane"});
            }
            try
            {
                CurrentUser = _userService.VerifyUser(HttpContext.Session.GetString("Username"), HttpContext.Session.GetString("Password"));
                if(CurrentUser.UserId <= 0)
                {
                    return RedirectToPage("/Users/Login", "Redirect", new { message = "Du kan ikke booke en bane som denne bruger. Log ind på din egen bruger" });
                }
                AvailableUsers = _userService.GetAllUsers();
                AvailableUsers.Remove(AvailableUsers.Find(u => u.UserId == CurrentUser.UserId));
                AvailableUsers.Remove(AvailableUsers.Find(u => u.UserId == 0));
                if (datetime != null)
                {
                    DateString = datetime;
                }
                LaneInfo = _laneService.GetLaneByNumber(laneid);
                ExistingBookings = _bookingService.GetAllLaneBookings<UserLaneBooking>();

                if (SelectedDay != null)
                {
                    CurrentBooking = new UserLaneBooking(_bookingService.GetAllLaneBookings<LaneBooking>().Count(), laneid, (DateTime)SelectedDay, CurrentUser.UserId, 0, false);
                } else
                {
                    return RedirectToPage("BookingOverview");
                }
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
            return Page();
        }

        public IActionResult OnPostBook(int partnerid)
        {
            CurrentBooking.MateID = partnerid;
            try { 
                _bookingService.CreateLaneBooking(CurrentBooking);
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
            return RedirectToPage("BookingOverview");
        }
        public bool CheckMaximum(User user)
        {
            List<UserLaneBooking> bookingsWithin14Days = ExistingBookings.Where(b => b.DateStart >= DateTime.Now && b.DateStart <= DateTime.Now.AddDays(14)).ToList();
            List<UserLaneBooking> bookingsWithThisUser = bookingsWithin14Days.Where(b => (b.UserID == user.UserId && b.MateID > 0) || b.MateID == user.UserId).ToList();
            return bookingsWithThisUser.Count >= 4;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Tennis.Interfaces;
using Tennis.Models;
using Tennis.Services;

namespace Tennis.Pages.Events
{
    public class ShowEventBookingsModel : PageModel
    {
        public ShowEventBookingsModel(IEventBookingService eventBookingService, IUserService userService)
        {
            _eventBookingService = eventBookingService;
            _userService = userService;
        }
        private IEventBookingService _eventBookingService;
        private IUserService _userService;

        public User LoggedInUser { get; set; }

        public List<EventBooking> Bookings { get; set; }
        public IActionResult OnGet()
        {
            LoggedInUser = _userService.VerifyUser(HttpContext.Session.GetString("Username"), HttpContext.Session.GetString("Password"));
            if (LoggedInUser == null)
            {
                return RedirectToPage("/Users/Login");
            }
            try
            {
                if (!_eventBookingService.Get)
                {
                    ViewData["ErrorMessage"] = "Fejl. Kunne ikke oprette event";
                    return Page();
                }
            }
            catch (SqlException sqlEx)
            {
                ViewData["ErrorMessage"] = "Database fejl. Fejlbesked:\n " + sqlEx.Message;
                return Page();
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = "Generel fejl. Fejlbesked:\n " + ex.Message;
                return Page();
            }

        }
    }
}

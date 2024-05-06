using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Tennis.Interfaces;
using Tennis.Models;
using Tennis.Services;

namespace Tennis.Pages.Events
{
    public class CreateEventBookingModel : PageModel
    {
        private IUserService _userService;
        private IEventService _eventService;
        private IEventBookingService _eventBookingService;
        [BindProperty]
        public EventBooking Booking { get; set; }
        public Event Event { get; set; }
        public CreateEventBookingModel(IUserService userService, IEventService eventService, IEventBookingService eventBookingService)
        {
            _userService = userService;
            _eventService = eventService;
            _eventBookingService = eventBookingService;
        }
        public User LoggedInUser { get; set; }
        public IActionResult OnGet(int eventID)
        {
            LoggedInUser = _userService.VerifyUser(HttpContext.Session.GetString("Username"), HttpContext.Session.GetString("Password"));
            if (LoggedInUser == null )
            {
                return RedirectToPage("/Users/Login");
            }
            try
            {

                Event = _eventService.GetEventByNumber(eventID);
                if (Event == null)
                {
                    ViewData["ErrorMessage"] = "Fejl, kunne ikke hente event fra database.";
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
            return Page();
        }
        public IActionResult OnPost(int eventID)
        {
            LoggedInUser = _userService.VerifyUser(HttpContext.Session.GetString("Username"), HttpContext.Session.GetString("Password"));
            if (LoggedInUser == null)
            {
                return RedirectToPage("/Users/Login");
            }                
            try
            {
                Event = _eventService.GetEventByNumber(eventID);
                if (Event == null)
                {
                    ViewData["ErrorMessage"] = "Fejl, kunne ikke hente event fra database.";
                    return Page();
                }
                Booking.Event = Event;
                Booking.User = LoggedInUser;
                if (!_eventBookingService.CreateEventBooking(Booking))
                {
                    ViewData["ErrorMessage"] = "Fejl, kunne ikke hente oprette tilmelding i database";
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
            return RedirectToPage("IndexUser");
        }
    }
}

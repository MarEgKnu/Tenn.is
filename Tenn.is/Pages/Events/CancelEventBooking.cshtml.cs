using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Pages.Events
{
    public class CancelEventBookingModel : PageModel
    {
        private IUserService _userService;
        private IEventBookingService _eventBookingService;
        public CancelEventBookingModel(IUserService userService , IEventBookingService eventBookingService)
        {
            _userService = userService;
            _eventBookingService = eventBookingService;

        }
        public EventBooking Booking { get; set; }
        public User LoggedInUser { get; set; }
        public IActionResult OnGet(int bookingID)
        {
            LoggedInUser = _userService.VerifyUser(HttpContext.Session.GetString("Username"), HttpContext.Session.GetString("Password"));
            if (LoggedInUser == null)
            {
                return RedirectToPage("/Users/Login");
            }
            try
            {
                Booking = _eventBookingService.GetEventBookingById(bookingID);
                if (Booking == null)
                {
                    ViewData["ErrorMessage"] = "Fejl, kunne ikke finde booking i database";
                    return Page();
                }
                else if (!Booking.CanBeCancelled)
                {
                    ViewData["ErrorMessage"] = "Denne booking kan ikke slettes, er eventen i fortid eller aflyst?";
                    return Page();
                }
                else if (Booking.User.UserId != LoggedInUser.UserId)
                {
                    return RedirectToPage("IndexUser");
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
        public IActionResult OnPost(int bookingID)
        {
            LoggedInUser = _userService.VerifyUser(HttpContext.Session.GetString("Username"), HttpContext.Session.GetString("Password"));
            if (LoggedInUser == null)
            {
                return RedirectToPage("/Users/Login");
            }
            try
            {
                Booking = _eventBookingService.GetEventBookingById(bookingID);
                if (Booking == null)
                {
                    ViewData["ErrorMessage"] = "Fejl, kunne ikke finde booking i database";
                    return Page();
                }
                else if (!Booking.CanBeCancelled)
                {
                    ViewData["ErrorMessage"] = "Denne booking kan ikke slettes, er eventen i fortid eller aflyst?";
                    return Page();
                }
                else if (Booking.User.UserId != LoggedInUser.UserId)
                {
                    return RedirectToPage("IndexUser");
                }
                if (!_eventBookingService.DeleteEventBooking(bookingID))
                {
                    ViewData["ErrorMessage"] = "Fejl, kunne ikke slette booking i database";
                    return Page();
                }
            }
            catch (SqlException sqlEx)
            {
                ViewData["ErrorMessage"] = "Database fejl. Fejlbesked:\n" + sqlEx.Message;
                return Page();
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = "Generel fejl. Fejlbesked:\n" + ex.Message;
                return Page();
            }
            return RedirectToPage("IndexUser");


        }
    }
}

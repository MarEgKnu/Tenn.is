using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Pages.Users
{
    public class IndexModel : PageModel
    {
        private IEventBookingService _eventBookingService;
        private IUserService _userService;

        public IndexModel(IEventBookingService eventBookingService, IUserService userService)
        {
            _eventBookingService = eventBookingService;
            _userService = userService;
            DateFilter = DateTime.Now;
        }



        [BindProperty(SupportsGet = true)]
        public string TitleFilter { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool CancelledFilter { get; set; }
        [BindProperty(SupportsGet = true), DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? DateFilter { get; set; }

        public User LoggedInUser { get; set; }

        public User CurrentUser { get; set; }

        public List<Event> MyBookings { get; set; }

        public List<LaneBooking> MyLaneBookings { get; set; }


        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("Username") != null)
            {
                try
                {
                    CurrentUser = _userService.VerifyUser(HttpContext.Session.GetString("Username"), HttpContext.Session.GetString("Password"));
                if (CurrentUser != null) {
                        MyBookings = _userService.GetAllEventBookingWithUserId(CurrentUser.UserId);
                        return Page();
                }
                else 
                        return RedirectToPage("Login");
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

            }
                return RedirectToPage("Login");
        }








    }
}

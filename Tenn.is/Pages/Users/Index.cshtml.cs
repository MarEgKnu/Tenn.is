using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using Tennis.Interfaces;
using Tennis.Models;
using Tennis.Services;

namespace Tennis.Pages.Users
{
    public class IndexModel : PageModel
    {
        private IEventBookingService _eventBookingService;
        private IUserService _userService;
        private ILaneBookingService _laneBookingService;

        public IndexModel(IEventBookingService eventBookingService, IUserService userService, ILaneBookingService laneBookingService)
        {
            _eventBookingService = eventBookingService;
            _userService = userService;
            DateFilter = DateTime.Now;
            _laneBookingService = laneBookingService;
        }



        [BindProperty(SupportsGet = true)]
        public string TitleFilter { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool CancelledFilter { get; set; }
        [BindProperty(SupportsGet = true), DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? DateFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool chronologic { get; set; }

        public User CurrentUser { get; set; }

        public User Mate { get; set; }

        public List<EventBooking> MyBookings { get; set; }

        public List<LaneBooking> MyLaneBookings { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; }


        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("Username") != null)
            {
                try
                {
                    CurrentUser = _userService.VerifyUser(HttpContext.Session.GetString("Username"), HttpContext.Session.GetString("Password"));
                if (CurrentUser != null) {
                        //MyBookings = _userService.GetAllEventBookingWithUserId(CurrentUser.UserId);
                        MyBookings = _eventBookingService.GetEventBookingsByUser(CurrentUser.UserId);
                        MyLaneBookings = _laneBookingService.GetAllLaneBookings<LaneBooking>();
                        Sorted();
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



        public void Sorted()
        {
            if (chronologic)
            {
                SortBy = "chronologik";
            }
            else
                SortBy = string.Empty;

            //MyLaneBookings = _laneBookingService.GetAllLaneBookings<UserLaneBooking>();

            switch (SortBy)
            {
                case "chronologik":
                    MyLaneBookings = MyLaneBookings.OrderBy(B => B.DateStart).ToList();
                    break;
                case "mate":
                    break;
                default:
                    break;
            }

        }




    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using Tennis.Helpers;
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
            DateFilter = DateTime.Now;
        }
        private IEventBookingService _eventBookingService;
        private IUserService _userService;


        [BindProperty(SupportsGet = true)]
        public string TitleFilter { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool CancelledFilter { get; set; }
        [BindProperty(SupportsGet = true), DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? DateFilter { get; set; }

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
                Bookings = _eventBookingService.GetEventBookingsByUser(LoggedInUser.UserId).OrderByDescending(b => b.Event.EventTime.StartTime).ToList();
                FilterBookings();
                
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
        private void FilterBookings()
        {
            List<Predicate<EventBooking>> conditions = new List<Predicate<EventBooking>>();
            if (!TitleFilter.IsNullOrEmpty())
            {
                conditions.Add(e => e.Event.Title.ToLower().Contains(TitleFilter.ToLower()));
            }
            if (!CancelledFilter)
            {
                conditions.Add(e => e.Event.Cancelled == CancelledFilter);
            }
            if (DateFilter != null)
            {
                conditions.Add(e => e.Event.EventStateAt(DateFilter.Value) == RelativeTime.Ongoing || e.Event.EventStateAt(DateFilter.Value) == RelativeTime.Future);
            }
            Bookings = _eventBookingService.GetEventBookingsOnConditions(conditions, Bookings);
        }
    }
}

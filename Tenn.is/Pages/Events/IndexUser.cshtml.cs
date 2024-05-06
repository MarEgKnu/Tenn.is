using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using Tennis.Helpers;
using Tennis.Interfaces;
using Tennis.Models;
using Tennis.Services;

namespace Tennis.Pages.Events
{
    public class IndexUserModel : PageModel
    {
        private IUserService _userService;
        private IEventService _eventService;
        private IEventBookingService _eventBookingService;
        [BindProperty(SupportsGet = true)]
        public bool CancelledFilter { get; set; }

        [BindProperty(SupportsGet = true), DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? DateFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string StringFilter { get; set; }

        public User LoggedInUser { get; set; }
        public List<Event> Events { get; set; }
        public IndexUserModel(IUserService userService, IEventService eventService, IEventBookingService eventBookingService)
        {
            _eventService = eventService;
            _userService = userService;
            _eventBookingService = eventBookingService;
            DateFilter = DateTime.Now;
            
        }
        public void OnGet()
        {
            LoggedInUser = _userService.VerifyUser(HttpContext.Session.GetString("Username"), HttpContext.Session.GetString("Password"));
            try
            {
                Events = _eventService.GetAllEvents().OrderBy(e => e.EventTime.StartTime).ToList();
                FilterEvents();

                
            }
            catch (SqlException sqlEx)
            {
                Events = new List<Event>();
                ViewData["ErrorMessage"] = "Database fejl. Fejlbesked:\n" + sqlEx.Message;
            }
            catch (Exception ex)
            {
                Events = new List<Event>();
                ViewData["ErrorMessage"] = "Generel fejl. Fejlbesked:\n" + ex.Message;
            }
        }




        public void FilterEvents()
        {
            List<Predicate<Event>> conditions = new List<Predicate<Event>>();
            if (!StringFilter.IsNullOrEmpty())
            {
                conditions.Add(e => e.Title.ToLower().Contains(StringFilter.ToLower()) ||
                                    e.Description.ToLower().Contains(StringFilter.ToLower()));
            }
            if (!CancelledFilter)
            {
                conditions.Add(e => e.Cancelled == CancelledFilter);
            }
            if (DateFilter != null)
            {
                conditions.Add(e => e.EventStateAt(DateFilter.Value) == RelativeTime.Ongoing || e.EventStateAt(DateFilter.Value) == RelativeTime.Future);
            }
            Events = _eventService.GetEventsOnConditions(conditions, Events);
        }
    }
}

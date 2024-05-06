using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using Tennis.Helpers;
using Tennis.Interfaces;
using Tennis.Models;
using Tennis.Services;

namespace Tennis.Pages.Events
{
    public class IndexAdminModel : PageModel
    {
        private IUserService _userService;
        private IEventService _eventService;
        [BindProperty(SupportsGet = true)]
        public string GenericFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? EventIDFilter { get; set; }
        [BindProperty(SupportsGet = true)]
        public string TitleFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string DescriptionFilter { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool? CancelledFilter { get; set; }

        [BindProperty(SupportsGet = true), DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? DateFilter { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? CancellationThresholdFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool AdvancedSearch { get; set ; } 
        public List<Event> Events { get; set; }

        public SelectList CancelelledFilterOptions { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; }

        [BindProperty(SupportsGet = true)]
        public string PrevSortBy { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool Descending { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool ViewOldEventsFilter { get; set; }


        public IndexAdminModel(IEventService eventService, IUserService userService)
        {
            _eventService = eventService;
            CancelelledFilterOptions = new SelectList(BoolHelpers.CancelledBoolKeyValuePair, "Key", "Value");
            _userService = userService;
        }
        public IActionResult OnGet()
        {
            if (!_userService.AdminVerify(HttpContext.Session.GetString("Username"), HttpContext.Session.GetString("Password")))
            {
                return RedirectToPage("AccessDenied");
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
                Events = _eventService.GetAllEvents();
                if (AdvancedSearch)
                {
                    FilterEventsAdvanced();
                }
                else
                {
                    FilterEventsBasic();
                   
                }
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
            SortEvents();
            return Page();
        }
        public void OnPost()
        {

        }
        private void SortEvents()
        {
            if (Descending)
            {
                switch (SortBy)
                {
                    case "EventID":
                        Events = Events.OrderByDescending(e => e.EventID).ToList();
                        break;
                    case "Title":
                        Events = Events.OrderByDescending(e => e.Title).ToList();
                        break;
                    case "Description":
                        Events = Events.OrderByDescending(e => e.Description).ToList();
                        break;
                    case "Cancelled":
                        Events = Events.OrderByDescending(e => e.Cancelled).ToList();
                        break;
                    case "StartTime":
                        Events = Events.OrderByDescending(e => e.EventTime.StartTime).ToList();
                        break;
                    case "EndTime":
                        Events = Events.OrderByDescending(e => e.EventTime.EndTime).ToList();
                        break;
                    case "CancellationThreshold":
                        Events = Events.OrderByDescending(e => e.CancellationThresholdMinutes).ToList();
                        break;
                    case "EventState":
                        Events = Events.OrderByDescending(e => e.EventState).ToList();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (SortBy)
                {
                    case "EventID":
                        Events = Events.OrderBy(e => e.EventID).ToList();
                        break;
                    case "Title":
                        Events = Events.OrderBy(e => e.Title).ToList();
                        break;
                    case "Description":
                        Events = Events.OrderBy(e => e.Description).ToList();
                        break;
                    case "Cancelled":
                        Events = Events.OrderBy(e => e.Cancelled).ToList();
                        break;
                    case "StartTime":
                        Events = Events.OrderBy(e => e.EventTime.StartTime).ToList();
                        break;
                    case "EndTime":
                        Events = Events.OrderBy(e => e.EventTime.EndTime).ToList();
                        break;
                    case "CancellationThreshold":
                        Events = Events.OrderBy(e => e.CancellationThresholdMinutes).ToList();
                        break;
                    case "EventState":
                        Events = Events.OrderBy(e => e.EventState).ToList();
                        break;
                    default:
                        break;
                }
            }
        }
        public void FilterEventsBasic()
        {
            List<Predicate<Event>> conditions = new List<Predicate<Event>>();
            if (!GenericFilter.IsNullOrEmpty())
            {
                conditions.Add(e => e.Title.ToLower().Contains(GenericFilter.ToLower()) ||
                                e.Description.ToLower().Contains(GenericFilter.ToLower()));
            }
            if (!ViewOldEventsFilter)
            {
                conditions.Add(e => e.EventState != RelativeTime.Past);
            }
            Events = _eventService.GetEventsOnConditions(conditions, Events);
        }
        public void FilterEventsAdvanced()
        {
            List<Predicate<Event>> conditions = new List<Predicate<Event>>();
            if (EventIDFilter != null) 
            {
                conditions.Add(e => e.EventID == EventIDFilter);   
            }
            if (!ViewOldEventsFilter)
            {
                conditions.Add(e => e.EventState != RelativeTime.Past);
            }
            if (!TitleFilter.IsNullOrEmpty())
            {
                conditions.Add(e => e.Title.ToLower().Contains(TitleFilter.ToLower()));
            }
            if (!DescriptionFilter.IsNullOrEmpty())
            {
                conditions.Add(e => e.Description.ToLower().Contains(DescriptionFilter.ToLower()));
            }
            if (CancelledFilter != null)
            {
                conditions.Add(e => e.Cancelled == CancelledFilter);
            }
            if (DateFilter != null)
            {
                conditions.Add(e => e.EventStateAt(DateFilter.Value) == RelativeTime.Ongoing);
            }
            if (CancellationThresholdFilter != null)
            {
                conditions.Add(e => e.CancellationThresholdMinutes == CancellationThresholdFilter);
            }
            Events = _eventService.GetEventsOnConditions(conditions, Events);
        }
    }
}

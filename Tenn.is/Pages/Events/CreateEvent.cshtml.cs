using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Tennis.Helpers;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Pages.Events
{
    public class CreateEventModel : PageModel
    {
        private IEventService _eventService;
        private IUserService _userService;
        public CreateEventModel(IEventService eventService, IUserService userService)
        {
            _eventService = eventService;
            _userService = userService;
            Event = new Event(1, "", 0, "", new Helpers.TimeBetween(DateTime.Now, DateTime.Now.AddHours(1)), false);
        }
        [BindProperty]
        public Event Event { get; set; }
        public IActionResult OnGet()
        {
            if (!_userService.AdminVerify(HttpContext.Session.GetString("Username"), HttpContext.Session.GetString("Password"))) 
            {
                return RedirectToPage("AccessDenied"); 
            }
            return Page();
        }
        public IActionResult OnPost()
        {
            if (!_userService.AdminVerify(HttpContext.Session.GetString("Username"), HttpContext.Session.GetString("Password")))
            {
                return RedirectToPage("AccessDenied");
            }
            if (Event.EventState == RelativeTime.Past)
            {   
                ModelState.AddModelError("Event.EventTime.EndTime", "Kan ikke oprette events i fortid");
            }
            
            if (!ModelState.IsValid)
            {
                ModelState.ExceptionToErrorMessage();
                return Page();
            }
            
            try
            {
                if (!_eventService.CreateEvent(Event))
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
            return RedirectToPage("IndexAdmin");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Tennis.Helpers;
using Tennis.Interfaces;
using Tennis.Models;
using Tennis.Services;

namespace Tennis.Pages.Events
{
    public class EditEventModel : PageModel
    {
        [BindProperty]
        public Event Event {  get; set; }
        private IEventService eventService;
        private IUserService userService;
        public EditEventModel(IEventService eventService, IUserService userService)
        {
            this.eventService = eventService;
            this.userService = userService;
        }
        public IActionResult OnGet(int eventID)
        {
            if (!userService.AdminVerify(HttpContext.Session.GetString("Username"), HttpContext.Session.GetString("Password")))
            {
                return RedirectToPage("AccessDenied");
            }
            try
            {

                Event = eventService.GetEventByNumber(eventID);
                if (Event == null)
                {
                    ViewData["ErrorMessage"] = "Fejl, kunne ikke hente event.";
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
            if (!userService.AdminVerify(HttpContext.Session.GetString("Username"), HttpContext.Session.GetString("Password")))
            {
                return RedirectToPage("AccessDenied");
            }
            if (Event.EventState == RelativeTime.Past)
            {
                //TODO: better error messages for if starttime is bigger than endtime
                ModelState.AddModelError("Event.EventTime.EndTime", "Kan ikke redigere events til at være i fortid");
            }
            if (!ModelState.IsValid)
            {
                ModelState.ExceptionToErrorMessage();
                return Page();
            }
            try
            {
                if (eventService.EditEvent(Event, eventID))
                {

                    return RedirectToPage("IndexAdmin");
                }
                ViewData["ErrorMessage"] = "Generel fejl.";
                return Page();
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

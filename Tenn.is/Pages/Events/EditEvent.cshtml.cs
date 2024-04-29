using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Tennis.Helpers;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Pages.Events
{
    public class EditEventModel : PageModel
    {
        [BindProperty]
        public Event Event {  get; set; }
        private IEventService eventService;
        public EditEventModel(IEventService eventService)
        {
            this.eventService = eventService;
        }
        public void OnGet(int eventID)
        {
            //TODO: check if admin
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
        }
        public IActionResult OnPost(int eventID)
        {
            //TODO: check if admin
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

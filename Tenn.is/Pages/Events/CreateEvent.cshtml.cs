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
        public CreateEventModel(IEventService eventService)
        {
            _eventService = eventService;
            Event = new Event(1, "", 0, "", new Helpers.TimeBetween(DateTime.Now, DateTime.Now.AddHours(1)), false);
        }
        [BindProperty]
        public Event Event { get; set; }
        public void OnGet()
        {
            //TODO: check if admin
        }
        public IActionResult OnPost()
        {
            //TODO: check if admin
            if (Event.EventState == RelativeTime.Past)
            {
                //TODO: better error messages for if starttime is bigger than endtime
                ModelState.AddModelError("Event.EventTime.EndTime", "Kan ikke oprette events i fortid");
            }
            ModelState.ExceptionToErrorMessage();
            if (!ModelState.IsValid)
            {
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

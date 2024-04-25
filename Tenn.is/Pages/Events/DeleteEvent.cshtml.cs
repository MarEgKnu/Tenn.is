using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Pages.Events
{

   
    public class DeleteEventModel : PageModel
    {
        public Event Event { get; set; }

        IEventService eventService;
        public DeleteEventModel(IEventService eventService)
        {
            this.eventService = eventService;
        }
        public void OnGet(int eventID)
        {
            //TODO: check if admin!
            
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
            try
            {
                bool result = eventService.DeleteEvent(eventID);
                if (!result)
                {
                    ViewData["ErrorMessage"] = "Fejl, kunne ikke finde event";
                    return Page();
                }
                // back to the event controlpanel
                return RedirectToPage("IndexAdmin");
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

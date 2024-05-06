using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Tennis.Interfaces;
using Tennis.Models;
using Tennis.Services;

namespace Tennis.Pages.Events
{

   
    public class DeleteEventModel : PageModel
    {
        public Event Event { get; set; }

        private IEventService eventService;
        private IUserService userService;
        public DeleteEventModel(IEventService eventService, IUserService userService)
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

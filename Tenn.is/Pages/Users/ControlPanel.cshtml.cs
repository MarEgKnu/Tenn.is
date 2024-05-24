using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Pages.Users
{
    public class ControlPanelModel : PageModel
    {
        private IUserService _userService;
        private User LoggedInUser { get; set; }
        public ControlPanelModel(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult OnGet()
        {
            LoggedInUser = _userService.VerifyUser(HttpContext.Session.GetString("Username"), HttpContext.Session.GetString("Password"));
            if (LoggedInUser == null || !LoggedInUser.Administrator )
            {
                return RedirectToPage("Login", "Redirect", new { message = "Du har ikke tilladelse til at se denne side. Log venligst ind som admin" });
            }
            else
            {
                return Page();
            }
        }
    }
}

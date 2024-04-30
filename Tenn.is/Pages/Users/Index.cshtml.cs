using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Pages.Users
{
    public class IndexModel : PageModel
    {
        private IUserService _userService;

        public User CurrentUser { get; set; }

        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("Username") != null)
            {
                CurrentUser = _userService.VerifyUser(HttpContext.Session.GetString("Username"), HttpContext.Session.GetString("Password"));
                if (CurrentUser != null) { 
                return Page();
                }
            }
                return RedirectToPage("Login");
        }
    }
}

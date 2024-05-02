using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
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
                try
                {
                    CurrentUser = _userService.VerifyUser(HttpContext.Session.GetString("Username"), HttpContext.Session.GetString("Password"));
                if (CurrentUser != null) { 
                return Page();
                }
                }
                catch (SqlException sqlExp)
                {
                    ViewData["ErrorMessage"] = "Databasefejl: " + sqlExp.Message;
                    return Page();
                }
                catch (Exception ex)
                {
                    ViewData["ErrorMessage"] = "Generel fejl: " + ex.Message;
                    return Page();
                }
            }
                return RedirectToPage("Login");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Pages.Users
{
    public class FirstSetupModel : PageModel
    {
        private IUserService _userService;

        public User CurrentUser { get; set; }
        [BindProperty]
        public string OldPassword { get; set; }
        [BindProperty]
        public string NewPassword { get; set; }
        public string PasswordMessage { get; set; }

        public FirstSetupModel(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult OnGet()
        {
            CurrentUser = _userService.VerifyUser(HttpContext.Session.GetString("Username"), HttpContext.Session.GetString("Password"));
            OldPassword = CurrentUser.Password;
            NewPassword = CurrentUser.Password;
            return Page();
        }

        public IActionResult OnPost()
        {
            try
            {
                CurrentUser = _userService.VerifyUser(HttpContext.Session.GetString("Username"), HttpContext.Session.GetString("Password"));
                if (OldPassword != NewPassword)
                {
                    CurrentUser.RandomPassword = false;
                    CurrentUser.Password = NewPassword;
                    if (_userService.EditUser(CurrentUser, CurrentUser.UserId))
                    {
                        HttpContext.Session.SetString("Password", NewPassword);
                        return RedirectToPage("Index");
                    }
                    else
                    {
                        PasswordMessage = "Noget gik galt ved opdatering. Prøv at naviger tilbage til denne side vha. login";
                        return Page();
                    }
                } else
                {
                    PasswordMessage = "Ja, vi har lavet et flot password. Men det er en god idé at bruge dit eget i stedet.";
                    return Page();
                }
                
            }
            catch (SqlException sqlExp)
            {
                CurrentUser = new User();
                ViewData["ErrorMessage"] = "Databasefejl: " + sqlExp.Message;
                return Page();
            }
            catch (Exception ex)
            {
                CurrentUser = new User();
                ViewData["ErrorMessage"] = "Generel fejl: " + ex.Message;
                return Page();
            }
        }
    }
}

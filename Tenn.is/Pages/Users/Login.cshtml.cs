using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Pages.Users
{
    public class LoginModel : PageModel
    {
        private IUserService _userService;

        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }
        public string Message { get; set; }

        public LoginModel(IUserService userService)
        {
            _userService = userService;
        }

        public void OnGet()
        {

        }

        public IActionResult OnGetRedirect(string message)
        {
            Message = message;
            return Page();
        }

        public IActionResult OnPost() 
        {
            bool valid = true;
            if (Username == null)
            {
                valid = false;
                Message = "Husk at skrive brugernavn";
            }
            if (Password == null)
            {
                if (valid == false)
                {
                    Message = "Indtast brugernavn og password nedenfor";
                }
                else
                {
                    valid = false;
                    Message = "Husk at skrive password";
                }
            }
            if (valid)
            {
                try
                {
                    User loginUser = _userService.VerifyUser(Username, Password);
                    if (loginUser != null)
                    {
                        HttpContext.Session.SetString("Username", Username);
                        HttpContext.Session.SetString("Password", Password);
                        if (loginUser.RandomPassword)
                        {
                            return RedirectToPage("FirstSetup");
                        }
                        return RedirectToPage("Index");
                    }
                    else
                    {
                        Message = "Forkert brugernavn eller password";
                        Username = "";
                        Password = "";
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
                return Page();

        }
        public void OnGetLogout()
        {
            _userService.LogOut(HttpContext);
        }
    }
}

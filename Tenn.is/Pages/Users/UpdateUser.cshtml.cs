using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Pages.Users
{
    public class UpdateUserModel : PageModel
    {
        private IUserService _userService;

        [BindProperty]
        public User UserToUpdate { get; set; }
        [BindProperty]
        public string OldPassword { get; set; }
        public string Message { get; set; }
        public string PhoneString { get; set; }
        private bool _isOtherUser;
        public bool Admin { get; set; }

        public UpdateUserModel(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult OnGetAdmin(int userid)
        {
            _isOtherUser = true;
            try
            {
                if (_userService.AdminVerify(HttpContext.Session.GetString("Username"), HttpContext.Session.GetString("Password"))) { 
                UserToUpdate = _userService.GetUserById(userid);
                OldPassword = UserToUpdate.Password;
                    Admin = true;
                return Page();
                } else
                {
                    return RedirectToPage("Login");
                }
            }
            catch (SqlException sqlExp)
            {
                UserToUpdate = new User();
                ViewData["ErrorMessage"] = "Databasefejl: " + sqlExp.Message;
                return Page();
            }
            catch (Exception ex)
            {
                UserToUpdate = new User();
                ViewData["ErrorMessage"] = "Generel fejl: " + ex.Message;
                return Page();
            }
        }
        public IActionResult OnGet()
        {
            _isOtherUser = false;
            try
            {
                if (HttpContext.Session.GetString("Username") == null)
                {
                    return RedirectToPage("Login");
                }
                UserToUpdate = _userService.VerifyUser(HttpContext.Session.GetString("Username"), HttpContext.Session.GetString("Password"));
                OldPassword = UserToUpdate.Password;
                return Page();
            }
            catch (SqlException sqlExp)
            {
                UserToUpdate = new User();
                ViewData["ErrorMessage"] = "Databasefejl: " + sqlExp.Message;
                return Page();
            }
            catch (Exception ex)
            {
                UserToUpdate = new User();
                ViewData["ErrorMessage"] = "Generel fejl: " + ex.Message;
                return Page();
            }
        }

        public IActionResult OnPostUpdate(int userid)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                if (!UserToUpdate.RandomPassword || UserToUpdate.Password != OldPassword)
                {
                    UserToUpdate.RandomPassword = false;
                }
                if (!_userService.ValidatePhoneLength(UserToUpdate.Phone))
                {
                    PhoneString = "Telefonnummer ikke formateret korrekt. Du behøver ikke skrive +45. Internationale telefonnumre er beklageligvis ikke gyldige";
                    return Page();
                }
                if (_userService.EditUser(UserToUpdate, userid))
                {
                    if (_isOtherUser) {
                    return RedirectToPage("Overview");
                    } else
                    {
                        return RedirectToPage("Index");
                    }
                }
                else
                {
                    Message = "Noget gik galt ved opdatering. Prøv at naviger tilbage til denne side vha. oversigt.";
                    return Page();
                }
            }
            catch (SqlException sqlExp)
            {
                UserToUpdate = new User();
                ViewData["ErrorMessage"] = "Databasefejl: " + sqlExp.Message;
                return Page();
            }
            catch (Exception ex)
            {
                UserToUpdate = new User();
                ViewData["ErrorMessage"] = "Generel fejl: " + ex.Message;
                return Page();
            }
        }
        public IActionResult OnPostCancel()
        {
            return RedirectToPage("Overview");
        }
    }
}

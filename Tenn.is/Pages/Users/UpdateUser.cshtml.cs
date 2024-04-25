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
        public string OldPassword { get; set; }
        public string Message { get; set; }

        public UpdateUserModel(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult OnGet(int userid)
        {
            try
            {
                UserToUpdate = _userService.GetUserById(userid);
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
                if (_userService.EditUser(UserToUpdate, userid))
                {
                    return RedirectToPage("Overview");
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

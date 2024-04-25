using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Pages.Users
{
    public class DeleteUserModel : PageModel
    {
        private IUserService _userService;

        public User UserToDelete { get; set; }

        public string DbMessage { get; set; }

        public DeleteUserModel(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult OnGet(int userid)
        {
            try { 
            UserToDelete = _userService.GetUserById(userid);
            return Page();
            }
            catch (SqlException sqlExp)
            {
                UserToDelete = new User();
                ViewData["ErrorMessage"] = "Databasefejl: " + sqlExp.Message;
                return Page();
            }
            catch (Exception ex)
            {
                UserToDelete = new User();
                ViewData["ErrorMessage"] = "Generel fejl: " + ex.Message;
                return Page();
            }
        }

        public IActionResult OnPostDelete(int userid)
        {
            try { 
            if (_userService.DeleteUser(userid))
            {
                return RedirectToPage("Overview");
            } else
            {
                DbMessage = "Brugeren kunne ikke slettes. Tjek om du har tilladelse til at fjerne denne bruger (NB: SystemAdmin kan ikke fjernes)";
                UserToDelete = _userService.GetUserById(userid);
                return Page();
            }
            }
            catch (SqlException sqlExp)
            {
                UserToDelete = new User();
                ViewData["ErrorMessage"] = "Databasefejl: " + sqlExp.Message;
                return Page();
            }
            catch (Exception ex)
            {
                UserToDelete = new User();
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

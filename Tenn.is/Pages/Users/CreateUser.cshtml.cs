using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Pages.Users
{
    public class CreateUserModel : PageModel
    {
        private IUserService _userService;

        public string DatabaseString { get; set; }

        [BindProperty]
        public User NewUser { get; set; }

        public CreateUserModel(IUserService userservice)
        {
            _userService = userservice;
        }

        public IActionResult OnGet()
        {
            NewUser = new User(_userService.RandomPassword());

            return Page();
        }
        public IActionResult OnPost() 
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                NewUser.UserId = _userService.GetAllUsers().Count();
                NewUser.Administrator = false;
                NewUser.RandomPassword = true;
                if (_userService.CreateUser(NewUser))
                {
                    return RedirectToPage("Overview");
                }
                else
                {
                    DatabaseString = "Bruger findes allerede i systemet";
                    return Page();
                }
            } catch (SqlException sqlExp)
            {
                NewUser = new User(_userService.RandomPassword());
                ViewData["ErrorMessage"] = "Databasefejl: " + sqlExp.Message;
                return Page();
            }
            catch (Exception ex)
            {
                NewUser = new User(_userService.RandomPassword());
                ViewData["ErrorMessage"] = "Generel fejl: " + ex.Message;
                return Page();
            }
        }
    }
}

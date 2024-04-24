using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public void OnGet()
        {

        }
        public IActionResult OnPost() 
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            NewUser.Administrator = false;
            NewUser.UserId = _userService.GetAllUsers().Count();
            if (_userService.CreateUser(NewUser))
            {
                return RedirectToPage("Overview");
            }
            else
            {
                DatabaseString = "Bruger findes allerede i systemet";
                return Page();
            }
        }
    }
}

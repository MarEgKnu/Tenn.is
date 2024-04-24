using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Pages.Users
{
    public class OverviewModel : PageModel
    {

        private IUserService _userService;

        public List<User> Users { get; set; }

        public OverviewModel(IUserService userService)
        {
            _userService = userService;
        }

        public void OnGet()
        {
            Users = _userService.GetAllUsers();
        }
    }
}

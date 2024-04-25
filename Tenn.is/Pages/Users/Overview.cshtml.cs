using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
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
            try { 
            Users = _userService.GetAllUsers();
            } catch (SqlException sqlExp)
            {
                Users = new List<User>();
                ViewData["ErrorMessage"] = "Databasefejl: " + sqlExp.Message;
            }
            catch (Exception ex)
            {
                Users = new List<User>();
                ViewData["ErrorMessage"] = "Generel fejl: " + ex.Message;
            }
        }
    }
}

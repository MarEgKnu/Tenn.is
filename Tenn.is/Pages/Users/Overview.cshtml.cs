using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Tennis.Helpers;
using Tennis.Interfaces;
using Tennis.Models;
using Tennis.Services;

namespace Tennis.Pages.Users
{
    public class OverviewModel : PageModel
    {

        private IUserService _userService;

        [BindProperty(SupportsGet = true)]
        public string GenericFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string UsernameFilter { get; set; }
        [BindProperty(SupportsGet = true)]
        public string NameFilter { get; set; }
        [BindProperty(SupportsGet = true)]
        public string EmailFilter { get; set; }
        [BindProperty(SupportsGet = true)]
        public string PhoneFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool AdvancedSearch { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool AdminSearch { get; set; }

        public List<User> Users { get; set; }

        public OverviewModel(IUserService userService)
        {
            _userService = userService;
        }

        public void OnGet()
        {
            try { 
                Users = _userService.GetAllUsers();
                if (AdvancedSearch)
                {
                    FilterEventsAdvanced();
                } else
                {
                    FilterEventsBasic();
                }
                if (AdminSearch)
                {
                    Users = Users.FindAll(e => e.Administrator);
                }
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

        public void FilterEventsBasic()
        {
            if (!GenericFilter.IsNullOrEmpty())
            {
                Users = Users.FindAll(e => e.Username.ToLower().Contains(GenericFilter.ToLower()) ||
                                e.FirstName.ToLower().Contains(GenericFilter.ToLower()) ||
                                e.LastName.ToLower().Contains(GenericFilter.ToLower()) || 
                                e.Email.ToLower().Contains(GenericFilter.ToLower()) || 
                                e.Phone.ToLower().Contains(GenericFilter.ToLower()));
            }
        }

        public void FilterEventsAdvanced()
        {
            List<Predicate<User>> conditions = new List<Predicate<User>>();
            if (UsernameFilter != null)
            {
                conditions.Add(e => e.Username.ToLower().Contains(UsernameFilter.ToLower()));
            }
            if (!NameFilter.IsNullOrEmpty())
            {
                conditions.Add(e => e.FirstName.ToLower().Contains(NameFilter.ToLower()) || e.LastName.ToLower().Contains(NameFilter.ToLower()));
            }
            if (!EmailFilter.IsNullOrEmpty())
            {
                conditions.Add(e => e.Email.ToLower().Contains(EmailFilter.ToLower()));
            }
            if (!PhoneFilter.IsNullOrEmpty())
            {
                conditions.Add(e => e.Phone.Contains(PhoneFilter));
            }
            Users = FilterHelpers.GetItemsOnConditions(conditions, Users);
        }
    }
}

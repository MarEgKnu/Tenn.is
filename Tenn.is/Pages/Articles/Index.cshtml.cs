using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using Tennis.Interfaces;
using Tennis.Models;
using Tennis.Services;

namespace Tennis.Pages.Articles
{
    public class IndexModel : PageModel
    {

        public bool IsLoggedIn = false;
        public bool IsAdmin = false;
        public string Username = string.Empty;

        public List<Article> AllArticles { get; set; }
        [BindProperty(SupportsGet = true)] public string Sorting { get; set; } = "Initial";

        [BindProperty(SupportsGet = true)] public string SearchFilter { get; set; }
        [BindProperty(SupportsGet = true)] public bool IsChecked { get; set; }




        public UserService _userService {  get; set; }

        private IArticleService _articleService;
        public IndexModel(IArticleService articleService)
        {
            _articleService = new ArticleService(false);
        }


        public void OnGet()
        {
            if (SearchFilter.IsNullOrEmpty())
            {
                DisplayAllArticles();
            }
            else
            {
                if (IsChecked)
                {
                    FilterBySearchPlusContent();
                }
                else
                {
                    FilterBySearch();
                }
            }
            #region User check (TODO)
            //    if (_userService.VerifyUser(HttpContext.Session.GetString("Username"), HttpContext.Session.GetString("Password")) != null)
            //    {
            //        IsLoggedIn = true;
            //        Username = HttpContext.Session.GetString("Username");

            //        if (_userService.AdminVerify(Username, HttpContext.Session.GetString("Password"))) 
            //        {
            //            IsAdmin = true;
            //        }
            //        else { IsAdmin = false; }
            //    } 
            //    else { IsLoggedIn = false; }
            #endregion
        }
        public void DisplayAllArticles()
        {
            AllArticles = _articleService.GetAllArticles().OrderByDescending(a => a.TimeStamp).ToList();
        }


        public void FilterBySearch()
        {
            AllArticles = _articleService.SearchArticlesDefault(SearchFilter);
        }
        public void FilterBySearchPlusContent()
        {
            AllArticles = _articleService.SearchArticlesContent(SearchFilter);
        }

        public void OrderArticles()
        {
            AllArticles.OrderByDescending(a => a.TimeStamp);
        }

        public IActionResult OnPostSorting()
        {
            return RedirectToPage("Index");
        }


        public IActionResult OnPost(int id)
        {
            _articleService.DeleteArticle(id);
            return RedirectToPage("Index");
        }
        
    }
}

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

        [BindProperty(SupportsGet = true)] public string SearchFilter { get; set; }
        [BindProperty(SupportsGet = true)] public bool AlsoSearchContent { get; set; }

        public UserService _userService {  get; set; }

        private IArticleService _articleService;
        public IndexModel(IArticleService articleService)
        {
            _articleService = new ArticleService(true);
        }

        public void OnGet()
        {
            if (SearchFilter.IsNullOrEmpty()) {
                AllArticles = _articleService.GetAllArticles();
            }
            else {
                FilterBySearch();
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

        public void FilterBySearch()
        {
            AllArticles = _articleService.SearchArticlesDefault(SearchFilter);
        }

        public IActionResult OnPost(int id)
        {
            _articleService.DeleteArticle(id);
            return RedirectToPage("Index");
        }
    }
}

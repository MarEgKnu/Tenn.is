using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tennis.Interfaces;
using Tennis.Models;
using Tennis.Services;

namespace Tennis.Pages.Articles
{
    public class IndexModel : PageModel
    {
        public bool IsLoggedIn = false;
        public bool IsAdmin = false;

        public List<Article> AllArticles { get; set; }
        public UserService _userService {  get; set; }

        private IArticleService _articleService;
        public IndexModel(IArticleService articleService, UserService userService)
        {
            _articleService = new ArticleService(true);
            _userService = userService;
        }

        public void OnGet()
        {
            AllArticles = _articleService.GetAllArticles();

            if (_userService.VerifyUser(HttpContext.Session.GetString("Username"), HttpContext.Session.GetString("Password")) != null)
            {
                IsLoggedIn = true;
                //if (_userService.AdminVerify() { }
            } 
        }
    }
}

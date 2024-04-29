using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tennis.Interfaces;
using Tennis.Models;
using Tennis.Services;

namespace Tennis.Pages.Articles
{
    public class IndexModel : PageModel
    {
        public List<Article> AllArticles { get; set; }

        private IArticleService _articleService;
        public IndexModel(IArticleService articleService)
        {
            _articleService = new ArticleService(true);
        }

        public void OnGet()
        {
            AllArticles = _articleService.GetAllArticles();
        }
    }
}

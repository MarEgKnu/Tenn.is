using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tennis.Interfaces;
using Tennis.Models;
using Tennis.Services;

namespace Tennis.Pages.Articles
{
    public class CreateArticleModel : PageModel
    {
        [BindProperty]
        public string ArticleTitle { get; set; }
        [BindProperty]
        public string ArticleContent { get; set; }

        public ArticleService _articleService;
        public CreateArticleModel(IArticleService articleService)
        {
            _articleService = new ArticleService(false);
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            Article newArticle = new Article(ArticleTitle, ArticleContent);
            _articleService.CreateArticle(newArticle);
            return RedirectToPage("/Articles/Index");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tennis.Interfaces;
using Tennis.Models;
using Tennis.Services;

namespace Tennis.Pages.Articles
{
    public class EditArticleModel : PageModel
    {
        [BindProperty]
        public string ArticleTitle { get; set; }

        [BindProperty]
        public string ArticleContent { get; set; }

        public Article ArticleToEdit { get; set; }
        private IArticleService _articleService {  get; set; }
        public EditArticleModel(IArticleService articleService)
        {
            _articleService = new ArticleService(true);
        }

        public void OnGet(int id)
        {
            ArticleToEdit = _articleService.GetArticleById(id);
        }

        public void OnPost()
        {
            Article editedArticle = new Article(ArticleTitle, ArticleContent);
            _articleService.EditArticle(editedArticle, ArticleToEdit.ArticleID);
        }
    }
}

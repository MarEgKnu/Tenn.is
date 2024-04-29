using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tennis.Interfaces;
using Tennis.Models;
using Tennis.Services;

namespace Tennis.Pages.Articles
{
    public class EditArticleModel : PageModel
    {
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
    }
}

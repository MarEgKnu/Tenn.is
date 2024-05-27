using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tennis.Interfaces;
using Tennis.Models;
using Tennis.Services;

namespace Tennis.Pages.Articles
{
    public class EditArticleModel : PageModel
    {
        [BindProperty] public int ArticleID { get; set; }

        [BindProperty] public Article NewArticle { get; set; }


        private IArticleService _articleService {  get; set; }
        public EditArticleModel(IArticleService articleService)
        {
            _articleService = new ArticleService(false);
        }

        public void OnGet(int id)
        {
            NewArticle = _articleService.GetArticleById(id);
        }

        public IActionResult OnPost(int id)
        {

            _articleService.EditArticle(NewArticle, id);


            return RedirectToPage("Index");
        }

    }
}

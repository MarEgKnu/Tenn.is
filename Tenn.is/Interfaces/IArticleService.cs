using Tennis.Models;

namespace Tennis.Interfaces
{
    public interface IArticleService
    {
        bool CreateArticle(Article article);

        bool DeleteArticle(int id);

        bool EditArticle(Article article, int id);

        List<Article> GetAllArticles();

        Article? GetArticleById(int id);

        List<Article> SearchArticlesDefault(string searchFilter);
    }
}

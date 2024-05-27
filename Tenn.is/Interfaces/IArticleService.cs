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
        List<Article> SearchArticlesContent(string searchFilter);

        //Sorting
        public bool SortingByTimeStamp {  get; set; }
        public bool SortingByLastEdited { get; set; }
        public bool SortingByAuthor {  get; set; }
        public int SortingCounter { get; set; }
    }
}

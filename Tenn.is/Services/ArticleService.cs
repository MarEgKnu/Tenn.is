using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Services
{
    public class ArticleService : Connection, IArticleService
    {
        public ArticleService()
        {
            connectionString = Secret.ConnectionString;
        }
        public ArticleService(bool test)
        {
            if (test)
            {
                connectionString = Secret.ConnectionStringTest;
            }
            else
            {
                connectionString = Secret.ConnectionString;
            }
        }
        public bool CreateArticle(Article article)
        {
            throw new NotImplementedException();
        }

        public bool DeleteArticle(int id)
        {
            throw new NotImplementedException();
        }

        public bool EditArticle(Article article, int id)
        {
            throw new NotImplementedException();
        }

        public List<Article> GetAllArticles()
        {
            throw new NotImplementedException();
        }

        public Article GetArticleById(int id)
        {
            throw new NotImplementedException();
        }
    }
}

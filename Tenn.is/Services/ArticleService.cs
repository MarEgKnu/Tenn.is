using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.SqlTypes;
using Tennis.Helpers;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Services
{
    public class ArticleService : Connection, IArticleService
    {
        private string sqlCreateArticleWithImg = "INSERT INTO Articles (Title, Content, TimeStamp, ImgPath) VALUES (@title, @content, CURRENT_TIMESTAMP, @imgPath";
        private string sqlCreateArticle = "INSERT INTO Articles (Title, Content, TimeStamp) VALUES (@title, @content, CURRENT_TIMESTAMP)";
        private string sqlGetAllArticles = "SELECT ArticleID, Title, Content, AuthorID, TimeStamp, LastEdited, ImgPath FROM Articles";
        private string sqlGetArticleById = "SELECT * FROM Articles WHERE ArticleID = @articleId";
        private string sqlEditArticleWithImg = "UPDATE Articles SET Title = @newTitle, Content = @newContent, LastEdited = CURRENT_TIMESTAMP, ImgPath = @newImgPath WHERE ArticleID = @articleId";
        private string sqlEditArticle = "UPDATE Articles SET Title = @newTitle, Content = @newContent, LastEdited = CURRENT_TIMESTAMP WHERE ArticleID = @articleId";
        private string sqlDeleteArticle = "DELETE FROM Articles WHERE ArticleID = @articleId";
        

        // KOLONNERNES NUMMER - INT SOM KOLONNENAVN
        private int colArticleID = 0;
        private int colTitle = 1;
        private int colContent = 2;
        private int colAuthorID = 3;
        private int colTimeStamp = 4;
        private int colLastEdited = 5;
        private int colImgPath = 6;


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
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(sqlCreateArticle, connection);
                    command.Parameters.AddWithValue("@title", article.Title);
                    command.Parameters.AddWithValue("@content", article.Content);
                    command.Connection.Open();
                    int noOfRows = command.ExecuteNonQuery();
                    return noOfRows == 1;
                }
                catch (SqlException sqlExp)
                {
                    Console.WriteLine("Database error" + sqlExp.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Generel fejl: " + ex.Message);
                }
                finally { }
            }
            return false;
        }

        public List<Article> GetAllArticles()
        {
            List<Article> allArticles = new List<Article>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(sqlGetAllArticles, connection);
                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int a_Id = reader.GetInt32("ArticleID");
                        string a_Title = reader.GetString("Title");
                        string a_Content = reader.GetString("Content");
                        int? a_AuthorId = DBReaderHelper.GetIntOrNull(reader, colAuthorID);
                        DateTime a_TimeStamp = reader.GetDateTime("TimeStamp");
                        DateTime? a_LastEdited = DBReaderHelper.GetDateTimeOrNull(reader, colLastEdited);
                        string? a_ImgPath = DBReaderHelper.GetStringOrNull(reader, colImgPath);
                        Article article = new Article(a_Id, a_Title, a_Content, a_AuthorId, a_TimeStamp, a_LastEdited, a_ImgPath);
                        allArticles.Add(article);
                    }
                    reader.Close();
                }
                catch (SqlException sqlExp)
                {
                    Console.WriteLine("Database error" + sqlExp.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Generel fejl: " + ex.Message);
                }
                finally { }
            }
            return allArticles;
        }

        public Article? GetArticleById(int id)
        {
            Article? articleFound = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(sqlGetArticleById, connection);
                    command.Parameters.AddWithValue("@articleID", id);
                    command.Connection.Open();

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        int a_ArticleID = reader.GetInt32("ArticleID");
                        string a_Title = reader.GetString("Title");
                        string a_Content = reader.GetString("Content");
                        int? a_AuthorID = DBReaderHelper.GetIntOrNull(reader, colAuthorID);
                        DateTime a_TimeStamp = reader.GetDateTime("TimeStamp");
                        DateTime? a_LastEdited = DBReaderHelper.GetDateTimeOrNull(reader, colLastEdited);
                        string? a_ImgPath = DBReaderHelper.GetStringOrNull(reader, colImgPath);
                        articleFound = new Article(a_ArticleID, a_Title, a_Content, a_AuthorID, a_TimeStamp, a_LastEdited, a_ImgPath);
                    }
                    reader.Close();
                }
                catch (SqlException sqlExp)
                {
                    Console.WriteLine("Database error (sql Exception): " + sqlExp.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Generel fejl (Exception): " + ex.Message);
                }
                finally
                {

                }
            }
            return articleFound;
        }

        public bool EditArticle(Article article, int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(sqlEditArticle, connection);
                    command.Parameters.AddWithValue("@newTitle", article.Title);
                    command.Parameters.AddWithValue("@newContent", article.Content);
                    command.Parameters.AddWithValue("@articleId", id);
                    command.Connection.Open();
                    int noOfRows = command.ExecuteNonQuery();
                    return noOfRows == 1;
                }
                catch (SqlException sqlExp)
                {
                    Console.WriteLine("Database error" + sqlExp.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Generel fejl: " + ex.Message);
                }
                finally
                {

                }
            }
            return false;
        }

        public bool DeleteArticle(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(sqlDeleteArticle, connection);
                    command.Parameters.AddWithValue("@articleID", id);
                    command.Connection.Open();
                    int noOfRows = command.ExecuteNonQuery();
                    return (noOfRows == 1);
                }
                catch (SqlException sqlExp)
                {
                    Console.WriteLine("Database error (sql Exception): " + sqlExp.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Generel fejl (Exception): " + ex.Message);
                }
                finally
                {

                }
            }
            return false;
        }
    }
}

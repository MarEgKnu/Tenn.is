using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tennis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tennis.Models;

namespace Tennis.Services.Tests
{

    [TestClass()]
    public class ArticleServiceTests
    {
        [TestMethod()]
        public void CreateArticleTest()
        {
            //Arrange
            ArticleService testService = new ArticleService(true);
            Article createTest = new Article("Test: CreateArticle", "Simpel artikel oprettet i unittest for CreateArticle.");
            int articleCount1 = testService.GetAllArticles().Count();

            //Act
            testService.CreateArticle(createTest);
            int articleCount2 = testService.GetAllArticles().Count();

            //Assert
            Assert.IsTrue(articleCount1 == (articleCount2 - 1));
        }

        [TestMethod()]
        public void GetAllArticlesTest()
        {
            //Arrange
            ArticleService testService = new ArticleService(true);
            int expectedResult = 5;
            //Act
            int articleCountResult = testService.GetAllArticles().Count();

            //Assert
            Assert.AreEqual(expectedResult, articleCountResult);
        }

        [TestMethod()]
        public void GetArticleByIdTest()
        {
            //Arrange
            ArticleService testService = new ArticleService(true);

            //Act
            Article? a = testService.GetArticleById(3);

            //Assert
            Assert.IsNotNull(a);
        }

        [TestMethod()]
        public void EditArticleTest()
        {
            //Arrange
            ArticleService testService = new ArticleService(true);
            int articleToEdit = 3;
            Article editedArticle = new Article("Edittttt", "ddddg har edited den her artikel");
            string? originalTitle = testService.GetArticleById(articleToEdit).Title;

            //Act
            bool returnedBool = testService.EditArticle(editedArticle, 3);
            string? newTitle = testService.GetArticleById(articleToEdit).Title;

            //Assert
            Assert.IsTrue((returnedBool) && (newTitle != originalTitle));
        }

        [TestMethod()]
        public void DeleteArticleTest()
        {
            //Arrange
            ArticleService testService = new ArticleService(true);
            Article article = testService.GetAllArticles().Last();
            int articleCount1 = testService.GetAllArticles().Count();

            //Act
            int articleToDeleteID = article.ArticleID;
            testService.DeleteArticle(articleToDeleteID);
            int articleCount2 = testService.GetAllArticles().Count();

            //Assert
            Assert.IsTrue(articleCount1 == (articleCount2 + 1));
        }
    }
}
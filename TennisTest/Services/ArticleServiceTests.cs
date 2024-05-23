﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            
            //Act
            int articleCountResult = testService.GetAllArticles().Count();

            //Assert
            Assert.IsTrue(articleCountResult > 0);
        }

        //[TestMethod()]
        //public void GetArticleByIdTest()
        //{
        //    //Arrange
        //    ArticleService testService = new ArticleService(true);

            //Act
            //Article? a = testService.GetArticleById(29);

        //    //Assert
        //    Assert.IsNotNull(a);
        //}

        //[TestMethod()]
        //public void EditArticleTest()
        //{
        //    //Arrange
        //    ArticleService testService = new ArticleService(true);
        //    int articleToEdit = 29;
        //    Article editedArticlea = new Article("aa", "aa");
        //    Article editedArticleb = new Article("bb", "bb");

        //    string? originalTitle = testService.GetArticleById(articleToEdit).Title;
        //    bool returnedBool = false;
        //    string? newTitle = null;

            //Act
            //if (originalTitle == "aa") {
            //    returnedBool = testService.EditArticle(editedArticleb, articleToEdit);
            //    newTitle = testService.GetArticleById(articleToEdit).Title;
            //}
            //else {
            //    returnedBool = testService.EditArticle(editedArticlea, articleToEdit);
            //    newTitle = testService.GetArticleById(articleToEdit).Title;
            //}

        //    //Assert
        //    Assert.IsTrue((returnedBool) && (newTitle != originalTitle));
        //}

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

        [TestMethod()]
        public void SearchArticlesDefaultTest()
        {
            //Arrange
            ArticleService testService = new ArticleService(true);
            List<Article> searchResults = new List<Article>();
            string searchFilter = "hej";
            int expectedCount = 1;

            //Act
            searchResults = testService.SearchArticlesDefault(searchFilter);
            int actualCount = searchResults.Count;

            //Assert
            Assert.AreEqual(expectedCount, actualCount);
        }
    }
}
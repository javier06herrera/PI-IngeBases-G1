using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proyecto.Controllers;
using Proyecto.Models;
using System.Web.Script.Serialization;
using System.Runtime.InteropServices;
using System.IO;
namespace Proyecto.Tests.Controllers
{
    [TestClass]
    public class ReviewsControllerTest
    {
        [TestMethod]
        public void ReviewFormTest()
        {
            //Arrange
            var review = new ReviewController();
            var articleModel = new ArticleModel();
            articleModel.articleId = 1;
            //Act
            var result = review.ReviewForm(articleModel) as ViewResult;
            
            //Assert
            Assert.IsNotNull(result.Model);
        }
      
    }
}

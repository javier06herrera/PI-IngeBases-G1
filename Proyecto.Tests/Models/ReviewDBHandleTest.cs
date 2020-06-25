using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proyecto.Models;

namespace Proyecto.Tests.Models
{
    [TestClass]
    public class ReviewDBHandleTest
    {
        [TestMethod]
        public void checkReviewersTest()
        {
            var dbh = new ReviewDBHandle();
            int articleId = 1;

            bool result = dbh.checkReviewers(articleId);
            Assert.IsTrue(result);
        }
    }
}

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

        [TestMethod]
        public void fetchMeritsTest()
        {
            var dbh = new ReviewDBHandle();
            string memberEmail = "barKev@puchimail.com";
            bool result;
            if (dbh.fetchMerits(memberEmail) >= 0)
            {
                result = true;
            }
            else
            {
                result = false;
            }
             
            Assert.IsTrue(result);
        }
    }
}

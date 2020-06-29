using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proyecto.Controllers;
using Proyecto.Models;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Proyecto.Tests.Controllers
{
    [TestClass]
    public class ReportControllerTest
    {
        [TestMethod]
        public void TestReportUsersGet()
        {
            var controller = new ReportController();
            var result = controller.ReportUsers() as ViewResult;
            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public void TestReportUsersPost()
        {
            var controller = new ReportController();
            var result = controller.ReportUsers("Country");
            Assert.AreNotEqual("",result);
        }
    }
}

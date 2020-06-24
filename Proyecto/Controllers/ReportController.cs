using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proyecto.Models;

namespace Proyecto.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult ReportUsers()
        {
            ReportModel model = new ReportModel();
            model.CategoryList = new List<SelectListItem>();
            model.CategoryList.Add(new SelectListItem() { Text = "Country", Value = "1" });
            model.CategoryList.Add(new SelectListItem() { Text = "Skills", Value = "2" });
            model.CategoryList.Add(new SelectListItem() { Text = "Languages", Value = "3" });
            model.CategoryList.Add(new SelectListItem() { Text = "Hobbies", Value = "4" });
            return View(model);
        }

        [HttpPost]
        public ActionResult ReportUsers(ReportModel model)
        {
            return View();
        }
        //http://localhost:54007/Report/ReportUsers
    }
}
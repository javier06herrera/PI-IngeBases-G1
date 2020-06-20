using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proyecto.Models;

namespace Proyecto.Controllers
{
    public class ReviewController : Controller
    {
        // GET: Review
        public ActionResult reviewForm()
        {
            List<SelectListItem> lst = new List<SelectListItem>();

            lst.Add(new SelectListItem() { Text = "1", Value = "1" });
            lst.Add(new SelectListItem() { Text = "2", Value = "2" });
            lst.Add(new SelectListItem() { Text = "3", Value = "3" });
            lst.Add(new SelectListItem() { Text = "4", Value = "4" });
            lst.Add(new SelectListItem() { Text = "5", Value = "5" });

            ViewBag.Options = lst;

            return View();
        }


        public ActionResult recordReview(ReviewsModel model)
        {
            //Console.WriteLine(model.options);
            return View("reviewForm");
        }
    }
}
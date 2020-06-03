using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proyecto.Models;




namespace Proyecto.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        [HttpPost]
        public ActionResult Registration(ProfileModel pmodel)
        {
            ProfileDBHandle sdb = new ProfileDBHandle();
            sdb.AddProfile(pmodel);
            ViewBag.Message = "Profile added successfully";

            return RedirectToAction("HomePage","Article",null);
        }

        public ActionResult Registration()
        {
            return View();
        }



        

        public ActionResult Profile()
        {
            ProfileDBHandle sdb = new ProfileDBHandle();
            ArticleDBHandle sdb1 = new ArticleDBHandle();

            ViewData["Articles"] = sdb1.GetArticle();
            ViewData["Profile"] = sdb.getMemberProfile(1);

            return View();
        }

    }
}
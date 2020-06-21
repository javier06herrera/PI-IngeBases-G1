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
            try
            {
                if (ModelState.IsValid)
                {
                    ProfileDBHandle sdb = new ProfileDBHandle();
                    sdb.AddProfile(pmodel);
                    ViewBag.Message = "We successfully created your profile!";
                    return RedirectToAction("HomePage", "Article", null);
                }
            }
            catch
            {
                ViewBag.Message = "Please, provide your information";
            }
            return View();
        }

        public ActionResult Registration()
        {
            return View();
        }

        public ActionResult Profile()
        {
            ProfileDBHandle sdb = new ProfileDBHandle();
            //ArticleDBHandle sdb1 = new ArticleDBHandle();

            ViewData["Articles"] = sdb.fetchMyArticles("barrKev@puchimail.com");
            ViewData["Profile"] = sdb.getMemberProfile("barrKev@puchimail.com");

            return View();
        }

        
        public ActionResult Login()
        {

            return View();

        }

        [HttpPost]
        public ActionResult Login(ProfileModel pmodel)
        {

            ProfileDBHandle sdb = new ProfileDBHandle();
            bool result = sdb.attemptLogin(pmodel);

            if (result == true)
            {
                Session["user"] = pmodel.email;
                Session["rank"] = pmodel.memberRank;
                ViewBag.Message = "Login succesfull, Welcome!";
                return RedirectToAction("HomePage", "Article", null);
            }
            else {
                ViewBag.Message = "Please, provide valid credentials.";
                return View();
            }
        }

    }
}
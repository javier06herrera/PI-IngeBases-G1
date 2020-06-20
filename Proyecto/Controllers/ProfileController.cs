using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proyecto.Models;
using System.Web.Script.Serialization;

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

        public ActionResult Report()
        {
            return View(new ProfileModel());
        }

        [HttpPost]
        public ActionResult Report(ProfileModel smodel)
        {
            return View();
        }

        public List<SelectListItem> GetValuesCategory(string id)
        {
            //get the products from the repository
            var products = new List<SelectListItem>();

            if(id == "1")
            {
                products.Add(new SelectListItem() { Text = "Costa Rica", Value = "1" });
                products.Add(new SelectListItem() { Text = "Mexico", Value = "2" });
            }

            else if (id == "2")
            {
                products.Add(new SelectListItem() { Text = "Networking", Value = "1" });
                products.Add(new SelectListItem() { Text = "DataBases", Value = "2" });
            }

            else if(id == "3")
            {
                products.Add(new SelectListItem() { Text = "English", Value = "1" });
                products.Add(new SelectListItem() { Text = "Italian", Value = "2" });
            }
            else if(id == "4")
            {
                products.Add(new SelectListItem() { Text = "Sports", Value = "1" });
                products.Add(new SelectListItem() { Text = "Movies", Value = "2" });
            }
            //return new JavaScriptSerializer().Serialize(products);
            ViewBag.DropdownList = products;
            return products;
        }

    }
}
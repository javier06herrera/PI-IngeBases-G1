using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proyecto.Models;
using System.Web.Script.Serialization;
using System.Runtime.InteropServices;
using System.IO;


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
            string user;

            
            if (!(Session["user"] is null)) //If someone has already sign in
            {
                user = Session["user"].ToString();               
            }
            else //If no one is signed up (for developers testing) ToBeRemoved
            {
                user = "barrKev@puchimail.com";
            }

            ViewData["Articles"] = sdb.fetchMyArticles(user);
            ViewData["Profile"] = sdb.getMemberProfile(user);

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

        public JsonResult GetValuesCategory(string id)
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
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult Login()
        {

            return View();

        }

        [HttpPost]
        public ActionResult Login(ProfileModel pmodel)
        {

            ProfileDBHandle sdb = new ProfileDBHandle();

            try
            {
                //ModelState.Remove("memberId");
                ModelState.Remove("name");
                ModelState.Remove("lastName");
                ModelState.Remove("birthDate");
                ModelState.Remove("age");
                ModelState.Remove("addressCity");
                ModelState.Remove("addressCountry");
                ModelState.Remove("hobbies");
                ModelState.Remove("languages");
                ModelState.Remove("mobile");
                ModelState.Remove("job");
                ModelState.Remove("memberRank");
                ModelState.Remove("points");
                ModelState.Remove("skills");

                if (ModelState.IsValid)
                {
                    bool result = sdb.attemptLogin(pmodel);

                    if (result == true)
                    {
                        Session["user"] = pmodel.email;
                        Session["rank"] = pmodel.memberRank;
                        ViewData["rank"] = pmodel.memberRank;
                        ViewBag.Message = "Login succesfull, Welcome!";
                        return RedirectToAction("HomePage", "Article", null);
                    }
                    else
                    {
                        ViewBag.Message = "Please, provide valid credentials.";
                    }
                }
            }
            catch(Exception e)
            {                                      
                ViewBag.Message = "Please fill all required fields.";              
            }
            return View();
        }
    }
}
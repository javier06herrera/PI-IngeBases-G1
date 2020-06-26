using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proyecto.Models;


namespace Proyecto.Controllers
{
    public class ColaborationController : Controller
    {
        // GET: Colaboration
        public ActionResult Colaboration()
        {
            ArticleDBHandle dbhandle = new ArticleDBHandle();
            ModelState.Clear();
            return View(dbhandle.GetArticle());
        }


        //public ActionResult askColaboration(ArticleModel model)
        //{

        //    return View();
        //}

        //[HttpPost]
        public ActionResult askColaboration(ArticleModel model)
        {
            ///cosas por hacer 

            return RedirectToAction("Colaboration");
        }


    }
}
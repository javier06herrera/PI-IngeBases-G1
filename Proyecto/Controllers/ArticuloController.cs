using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proyecto.Models;

namespace Proyecto.Controllers
{
    public class ArticuloController : Controller
    {
        public ActionResult Index()
        {
            ArticuloDBHandle dbhandle = new ArticuloDBHandle(); //Estos llamados son innecesarios, ya que los metodos de Handle podrian estar aqui
            ModelState.Clear();
            return View(dbhandle.GetArticulo());
        }

        // 2. *************ADD NEW Articulo ******************
        // GET: Articulo/Create
        //Obtener datos
        public ActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        //Pasar datos (eso explica el post
        [HttpPost]
     
        public ActionResult Create(ArticuloModel smodel)
        {
            try
            {
                if (ModelState.IsValid) //Si los datos que me pasaron son validos
                {
                    ArticuloDBHandle sdb = new ArticuloDBHandle();
                    if (sdb.AddArticulo(smodel))
                    {
                        ViewBag.Message = "Student Details Added Successfully";
                        ModelState.Clear();
                    }
                }
                else
                {
                    ViewBag.Message = "Falle por la fecha";
                }
                return View();
            }
            catch
            {
                ViewBag.Message = "Failed";
                return View();
            }
        }

        // 3. ************* EDIT Articulo DETAILS ******************
        // GET: Articulo/Edit/5
        public ActionResult Edit(int id)
        {
            ArticuloDBHandle sdb = new ArticuloDBHandle();
            return View(sdb.GetArticulo().Find(smodel => smodel.Id == id));
        }

        // POST: Articulo/Edit/5
        [HttpPost]
        //public ActionResult Edit(int id, ArticuloModel smodel)
        public ActionResult Edit(int id, ArticuloModel smodel)
        {
            try
            {
                ArticuloDBHandle sdb = new ArticuloDBHandle();
                sdb.UpdateDetails(smodel);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // 4. ************* DELETE Articulo DETAILS ******************
        // GET: Articulo/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                ArticuloDBHandle sdb = new ArticuloDBHandle();
                if (sdb.DeleteArticulo(id))
                {
                    ViewBag.AlertMsg = "Article Deleted Successfully";
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public static ArticuloModel cMain;
        public ActionResult SearchTopic()
        {
            ArticuloDBHandle dbHandle = new ArticuloDBHandle();
            ArticuloModel c = new ArticuloModel();
            cMain = new ArticuloModel();
            c.TopicsList = cMain.TopicsList = dbHandle.PopulateArticles();
            ViewBag.LblCountry = "";
            return View(c);
        }

        [HttpPost]
        public ActionResult SearchTopic(ArticuloModel country)
        {
            ArticuloDBHandle dbHandle = new ArticuloDBHandle();
            var g = cMain.TopicsList;
            var selectedCountry = g.Find(p => p.Value == country.Topic.ToString()); // SelectListItem
            //Arriba, tiene que escoger el .Topic para que la lista agarre los valores que le interesan
            country.TopicsList = dbHandle.PopulateArticles();
            //return View(dbHandle.GetResultado(Convert.ToString(country.TopicsList)));
            ViewBag.LblCountry = "You selected " + selectedCountry.Text.ToString();
            TempData["Topic"] = selectedCountry.Text.ToString();
            return RedirectToAction("showResult");
        }

        public ActionResult ShowResult()
        {
            ArticuloDBHandle dbHandle = new ArticuloDBHandle();
            string topic = Convert.ToString(TempData["Topic"]);
            return View(dbHandle.GetResultado(topic));
        }
    }
}
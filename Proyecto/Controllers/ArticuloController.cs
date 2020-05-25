using System;
using System.IO;
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
                    if (sdb.AddArticulo(smodel, false))
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

        public ActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file, ArticuloModel smodel)
        {
            try
            {
                if (file != null && file.ContentLength > 0)
                    try
                    {
                        //string path = Path.Combine(Server.MapPath("/Files"), Path.GetFileName(file.FileName));
                        smodel.content = Path.Combine(Server.MapPath("/Files"), Path.GetFileName(file.FileName));
                        file.SaveAs(smodel.content);
                        ViewBag.Message = "File uploaded successfully";
                        //smodel.content = "Cualquier otra cochinada";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    }
                else
                {
                    ViewBag.Message = "You have not specified a file.";
                }

                ModelState.Remove("content");
                if (ModelState.IsValid) //Si los datos que me pasaron son validos
                {
                    ArticuloDBHandle sdb = new ArticuloDBHandle();
                    if (sdb.AddArticulo(smodel, true))
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
        public ActionResult Edit(int articleId)
        {
            ArticuloDBHandle sdb = new ArticuloDBHandle();
            return View(sdb.GetArticulo().Find(smodel => smodel.articleId == articleId));
        }

        // POST: Articulo/Edit/5
        [HttpPost]
        //public ActionResult Edit(int id, ArticuloModel smodel)
        public ActionResult Edit(int articleId, ArticuloModel smodel)
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

        // Metodo para  editar articulos largos
        public ActionResult EditLong(int articleId)
        {
            ArticuloDBHandle sdb = new ArticuloDBHandle();
            return View(sdb.GetArticulo().Find(smodel => smodel.articleId == articleId));
        }

        // POST: Articulo/Edit/5
        [HttpPost]
        //public ActionResult Edit(int id, ArticuloModel smodel)
        public ActionResult EditLong(int articleId, HttpPostedFileBase file, ArticuloModel smodel)
        {
            //try
            //{
            //    ArticuloDBHandle sdb = new ArticuloDBHandle();
            //    sdb.UpdateDetails(smodel);
            //    return RedirectToAction("Index");
            //}
            try
            {
                if (file != null && file.ContentLength > 0)
                    try
                    {
                        //string path = Path.Combine(Server.MapPath("/Files"), Path.GetFileName(file.FileName));
                        smodel.content = Path.Combine(Server.MapPath("/Files"), Path.GetFileName(file.FileName));
                        file.SaveAs(smodel.content);
                        ViewBag.Message = "File uploaded successfully";
                        //smodel.content = "Cualquier otra cochinada";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    }
                else
                {
                    ViewBag.Message = "You have not specified a file.";
                }

                ModelState.Remove("content");
                if (ModelState.IsValid) //Si los datos que me pasaron son validos
                {
                    ArticuloDBHandle sdb = new ArticuloDBHandle();
                    sdb.UpdateDetails(smodel);
                    return RedirectToAction("Index");
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

        // 4. ************* DELETE Articulo DETAILS ******************
        // GET: Articulo/Delete/5
        public ActionResult Delete(int articleId)
        {
            try
            {
                ArticuloDBHandle sdb = new ArticuloDBHandle();
                if (sdb.DeleteArticulo(articleId))
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
        public ActionResult SearchTopic(ArticuloModel smodel)
        {
            ArticuloDBHandle dbHandle = new ArticuloDBHandle();
            var g = cMain.TopicsList;
            var selectedArticle = g.Find(p => p.Value == smodel.topic.ToString()); // SelectListItem
            //Arriba, tiene que escoger el .Topic para que la lista agarre los valores que le interesan
            smodel.TopicsList = dbHandle.PopulateArticles();
            //return View(dbHandle.GetResultado(Convert.ToString(country.TopicsList)));
            ViewBag.LblCountry = "You selected " + selectedArticle.Text.ToString();
            TempData["Topic"] = selectedArticle.Text.ToString();
            return RedirectToAction("showResult");
        }

        public ActionResult ShowResult()
        {
            ArticuloDBHandle dbHandle = new ArticuloDBHandle();
            string topic = Convert.ToString(TempData["Topic"]);
            return View(dbHandle.GetResultado(topic));
        }

        public ActionResult ShowFAQ()
        {
            ArticuloDBHandle dbhandle = new ArticuloDBHandle(); //Estos llamados son innecesarios, ya que los metodos de Handle podrian estar aqui
            ModelState.Clear();
            return View(dbhandle.GetQuestion(false));
        }


        public ActionResult SendFaq()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendFaq(FaqModel smodel)
        {
            try
            {
                ModelState.Remove("answer");
                if (ModelState.IsValid) //Si los datos que me pasaron son validos
                {
                    ArticuloDBHandle sdb = new ArticuloDBHandle();
                    if (sdb.AddQuestion(smodel, false))
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
        public ActionResult ModeratorFAQ()
        {
            ArticuloDBHandle dbhandle = new ArticuloDBHandle(); //Estos llamados son innecesarios, ya que los metodos de Handle podrian estar aqui
            ModelState.Clear();
            return View(dbhandle.GetQuestion(true));
        }

        public ActionResult PublishFaq(int questionId)
        {
            ArticuloDBHandle sdb = new ArticuloDBHandle();
            return View(sdb.GetQuestion(true).Find(smodel => smodel.questionId == questionId));
        }

        [HttpPost]
        public ActionResult PublishFaq(int questionId, FaqModel smodel)
        {
            try
            {
                ArticuloDBHandle sdb = new ArticuloDBHandle();
                sdb.UpdateQuestion(smodel);
                return RedirectToAction("ModeratorFAQ");
            }
            catch
            {
                return View();
            }
        }


    }
}
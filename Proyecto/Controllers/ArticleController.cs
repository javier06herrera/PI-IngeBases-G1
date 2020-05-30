using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proyecto.Models;

namespace Proyecto.Controllers
{
    public class ArticleController : Controller
    {
        public ActionResult Index()
        {
            ArticleDBHandle dbhandle = new ArticleDBHandle(); //Estos llamados son innecesarios, ya que los metodos de Handle podrian estar aqui
            ModelState.Clear();
            return View(dbhandle.GetArticle());
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

        public ActionResult Create(ArticleModel smodel)
        {
            try
            {
                if (ModelState.IsValid) //Si los datos que me pasaron son validos
                {
                    ArticleDBHandle sdb = new ArticleDBHandle();
                    if (sdb.AddArticle(smodel, false))
                    {
                        ViewBag.Message = "Article Details Added Successfully";
                        ModelState.Clear();
                    }
                }
                else
                {
                    ViewBag.Message = "Please complete the remaining fields";
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
        public ActionResult Upload(HttpPostedFileBase file, ArticleModel smodel)
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
                    ViewBag.Message = "Please specify a file.";
                }

                ModelState.Remove("content");
                if (ModelState.IsValid) //Si los datos que me pasaron son validos
                {
                    ArticleDBHandle sdb = new ArticleDBHandle();
                    if (sdb.AddArticle(smodel, true))
                    {
                        ViewBag.Message = "Article Details Added Successfully";
                        ModelState.Clear();
                    }
                }
                else
                {
                    ViewBag.Message = "Please complete the remaining fields";
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
            ArticleDBHandle sdb = new ArticleDBHandle();
            return View(sdb.GetArticle().Find(smodel => smodel.articleId == articleId));
        }

        // POST: Articulo/Edit/5
        [HttpPost]
        //public ActionResult Edit(int id, ArticuloModel smodel)
        public ActionResult Edit(int articleId, ArticleModel smodel)
        {
            //try
            {
                if (ModelState.IsValid) //Si los datos que me pasaron son validos
                {
                    ArticleDBHandle sdb = new ArticleDBHandle();
                    
                    if(sdb.UpdateDetails(smodel))
                    {
                        ViewBag.Message = "Article Details Added Successfully";
                        ModelState.Clear();
                    }
                }
                else
                {
                    ViewBag.Message = "Please complete the remaining fields";
                }
                return View();
            }
            //catch
            //{
            //    ViewBag.Message = "Failed";
            //    return View();
            //}
        }

        // Metodo para  editar articulos largos
        public ActionResult EditLong(int articleId)
        {
            ArticleDBHandle sdb = new ArticleDBHandle();
            return View(sdb.GetArticle().Find(smodel => smodel.articleId == articleId));
        }

        // POST: Articulo/Edit/5
        [HttpPost]
        //public ActionResult Edit(int id, ArticuloModel smodel)
        public ActionResult EditLong(int articleId, HttpPostedFileBase file, ArticleModel smodel)
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
                    ViewBag.Message = "Please complete the remaining fields";
                }

                ModelState.Remove("content");
                if (ModelState.IsValid) //Si los datos que me pasaron son validos
                {
                    ArticleDBHandle sdb = new ArticleDBHandle();
                    sdb.UpdateDetails(smodel);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "Please complete the remaining fields";
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
                ArticleDBHandle sdb = new ArticleDBHandle();
                if (sdb.DeleteArticle(articleId))
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

        public static ArticleModel cMain;
        public ActionResult SearchTopic()
        {
            ArticleDBHandle dbHandle = new ArticleDBHandle();
            ArticleModel c = new ArticleModel();
            cMain = new ArticleModel();
            c.TopicsList = cMain.TopicsList = dbHandle.PopulateArticles();
            ViewBag.LblCountry = "";
            return View(c);
        }

        [HttpPost]
        public ActionResult SearchTopic(ArticleModel smodel)
        {
            ArticleDBHandle dbHandle = new ArticleDBHandle();
            var g = cMain.TopicsList;
            var selectedArticle = g.Find(p => p.Value == smodel.topic.ToString()); // SelectListItem
            //Arriba, tiene que escoger el .Topic para que la lista agarre los valores que le interesan
            smodel.TopicsList = dbHandle.PopulateArticles();
            //return View(dbHandle.GetResults(Convert.ToString(country.TopicsList)));
            ViewBag.LblCountry = "You selected " + selectedArticle.Text.ToString();
            TempData["Topic"] = selectedArticle.Text.ToString();
            return RedirectToAction("showResult");
        }

        public ActionResult ShowResult()
        {
            ArticleDBHandle dbHandle = new ArticleDBHandle();
            string topic = Convert.ToString(TempData["Topic"]);
            return View(dbHandle.GetResults(topic));
        }

        public ActionResult ShowFAQ()
        {
            ArticleDBHandle dbhandle = new ArticleDBHandle(); //Estos llamados son innecesarios, ya que los metodos de Handle podrian estar aqui
            ModelState.Clear();
            return View(dbhandle.GetQuestion(false));
        }


        public ActionResult SendFaq()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendFaq(QuestionModel smodel)
        {
            try
            {
                ModelState.Remove("answer");
                if (ModelState.IsValid) //Si los datos que me pasaron son validos
                {
                    ArticleDBHandle sdb = new ArticleDBHandle();
                    if (sdb.AddQuestion(smodel, false))
                    {
                        ViewBag.Message = "Student Details Added Successfully";
                        ModelState.Clear();
                    }
                }
                else
                {
                    ViewBag.Message = "Please complete the remaining fields";
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
            ArticleDBHandle dbhandle = new ArticleDBHandle(); //Estos llamados son innecesarios, ya que los metodos de Handle podrian estar aqui
            ModelState.Clear();
            return View(dbhandle.GetQuestion(true));
        }

        public ActionResult PublishFaq(int questionId)
        {
            ArticleDBHandle sdb = new ArticleDBHandle();
            return View(sdb.GetQuestion(true).Find(smodel => smodel.questionId == questionId));
        }

        [HttpPost]
        public ActionResult PublishFaq(int questionId, QuestionModel smodel)
        {
            try
            {
                ArticleDBHandle sdb = new ArticleDBHandle();
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
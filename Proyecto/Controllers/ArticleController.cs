﻿using System;
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
        public ActionResult HomePage()
        {
            return View();
        }

        public ActionResult CommunityArticles()
        {
            ArticleDBHandle dbhandle = new ArticleDBHandle(); 
            ModelState.Clear();
            return View(dbhandle.GetArticle());
        }
        // 2. *************ADD NEW Articulo ******************
        // GET: Articulo/Create
        //Obtener datos
        public static ArticleModel mMain;

        public ActionResult Create()
        {
            mMain = new ArticleModel();
            ArticleModel c = new ArticleModel();
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Novel:Survival", Value = "Novel:Survival"});
            list.Add(new SelectListItem { Text = "Science Fiction:Robotics", Value = "Science Fiction:Robotics" });
            list.Add(new SelectListItem { Text = "Science Fiction:Space", Value = "Science Fiction:Space"});
            list.Add(new SelectListItem { Text = "Novel:Banana Republic", Value = "Novel:Banana Republic" });
            c.TopicsList = mMain.TopicsList = list;
            ViewBag.LblCountry = "";
            return View(c);
        }

        [HttpPost]
        public ActionResult Create(ArticleModel smodel)
        {
            try
            {

                ModelState.Remove("type");
                if (ModelState.IsValid) //Si los datos que me pasaron son validos
                {
                    ArticleDBHandle sdb = new ArticleDBHandle();
                    if (sdb.AddArticle(smodel, "short"))
                    {
                        ViewBag.Message = "Article Details Added Successfully";
                        ModelState.Clear();
                    }
                }
                else
                {
                    ViewBag.Message = "xyz";
                }
                return RedirectToAction("CommunityArticles");
            }
            catch
            {
                ViewBag.Message = "Failed";
                return View();
            }
        }

        public ActionResult Upload()
        {
            mMain = new ArticleModel();
            ArticleModel c = new ArticleModel();
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Novel:Survival", Value = "Novel:Survival"});
            list.Add(new SelectListItem { Text = "Science Fiction:Robotics", Value = "Science Fiction:Robotics" });
            list.Add(new SelectListItem { Text = "Science Fiction:Space", Value = "Science Fiction:Space"});
            list.Add(new SelectListItem { Text = "Novel:Banana Republic", Value = "Novel:Banana Republic" });
            c.TopicsList = mMain.TopicsList = list;
            ViewBag.LblCountry = "";
            return View(c);
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

                ModelState.Remove("type");
                ModelState.Remove("content");
                ModelState.Remove("type");
                if (ModelState.IsValid) //Tell if the data is valid 
                {
                    ArticleDBHandle sdb = new ArticleDBHandle();
                    if (sdb.AddArticle(smodel, "long"))
                    {
                        ViewBag.Message = "Article Details Added Successfully";
                        ModelState.Clear();
                    }
                }
                else
                {
                    ViewBag.Message = "Please complete the remaining fields";
                }
                 return RedirectToAction("CommunityArticles");
            }
            catch
            {
                ViewBag.Message = "Failed";
                return View();
            }
        }

        // 3. ************* EDIT Articulo DETAILS ******************
        // GET: Articulo/Edit/5
        public static ArticleModel lMain;
        public ActionResult Edit(int articleId)
        {
            lMain = new ArticleModel();
            ArticleDBHandle sdb = new ArticleDBHandle();
            ArticleModel model = sdb.GetArticle().Find(smodel => smodel.articleId == articleId);            
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Novel:Survival", Value = "Novel:Survival" });
            list.Add(new SelectListItem { Text = "Science Fiction:Robotics", Value = "Science Fiction:Robotics" });
            list.Add(new SelectListItem { Text = "Science Fiction:Space", Value = "Science Fiction:Space" });
            list.Add(new SelectListItem { Text = "Novel:Banana Republic", Value = "Novel:Banana Republic" });
            model.TopicsList =  list;
            ViewBag.LblCountry = "";
            return View(model);
        }

        // POST: Articulo/Edit/5
        [HttpPost]
        //public ActionResult Edit(int id, ArticuloModel smodel)
        public ActionResult Edit(int articleId, ArticleModel smodel)
        {
            try
            {
            
                ModelState.Remove("type");
                if (ModelState.IsValid) //Tell if the data is valid 
                {
                    ArticleDBHandle sdb = new ArticleDBHandle();
                    
                    if(sdb.UpdateDetails(smodel,"short"))
                    {
                        ViewBag.Message = "Article Details Added Successfully";
                        ModelState.Clear();
                    }
                }
                else
                {
                    ViewBag.Message = "Please complete the remaining fields";
                }
                return RedirectToAction("CommunityArticles");
            }
            catch
            {
                ViewBag.Message = "Failed";
                return View();
            }
        }

        //[HttpPost]
        //public ActionResult PreviewArticle(int articleId, ArticleModel smodel)
        //{

        //    ArticleDBHandle sdb = new ArticleDBHandle();

        //    sdb.UpdateAccess(smodel);

        //    return View();
        //}

        public ActionResult PreviewArticle(int articleId, ArticleModel artModel)
        {
        
            ArticleDBHandle sdb = new ArticleDBHandle();
            sdb.UpdateAccess(artModel);

            return View(sdb.GetArticle().Find(smodel => smodel.articleId == articleId));
        }

        public ActionResult isALike(int articleId)
        {
            ArticleDBHandle sdb = new ArticleDBHandle();
            bool like = true;
            return View(sdb.updateLikes(articleId, like));
        }

        public ActionResult isADisLike(int articleId)
        {
            ArticleDBHandle sdb = new ArticleDBHandle();
            bool like = false;
            return View(sdb.updateLikes(articleId, like));
        }
        //public ActionResult HtmlRaw(ArticleModel smodel)
        //{
        //    ViewBag.message = smodel.content; 
        //    return View();
        //}

        // Metodo para  editar articulos largos
        public ActionResult EditLong(int articleId)
        {
            lMain = new ArticleModel();
            ArticleDBHandle sdb = new ArticleDBHandle();
            ArticleModel model = sdb.GetArticle().Find(smodel => smodel.articleId == articleId);
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Novel:Survival", Value = "Novel:Survival" });
            list.Add(new SelectListItem { Text = "Science Fiction:Robotics", Value = "Science Fiction:Robotics" });
            list.Add(new SelectListItem { Text = "Science Fiction:Space", Value = "Science Fiction:Space" });
            list.Add(new SelectListItem { Text = "Novel:Banana Republic", Value = "Novel:Banana Republic" });
            model.TopicsList = list;
            ViewBag.LblCountry = "";
            return View(model);
        }

        // POST: Articulo/Edit/5
        [HttpPost]
        //public ActionResult Edit(int id, ArticuloModel smodel)
        public ActionResult EditLong(int articleId, HttpPostedFileBase file, ArticleModel smodel)
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
                    ViewBag.Message = "Please complete the remaining fields";
                }

                ModelState.Remove("content");
                ModelState.Remove("type");
                if (ModelState.IsValid) //Si los datos que me pasaron son validos
                {
                    ArticleDBHandle sdb = new ArticleDBHandle();
                    sdb.UpdateDetails(smodel,"long");
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
        private object sdb;

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
            var selectedArticle = g.Find(p => p.Value == smodel.topicName.ToString()); // SelectListItem
            //Arriba, tiene que escoger el .Topic para que la lista agarre los valores que le interesan
            smodel.TopicsList = dbHandle.PopulateArticles();
            //return View(dbHandle.GetResults(Convert.ToString(country.TopicsList)));
            ViewBag.LblCountry = "You selected " + selectedArticle.Text.ToString();
            TempData["topicName"] = selectedArticle.Text.ToString();
            return RedirectToAction("showResult");
        }

        public ActionResult ShowResult()
        {
            ArticleDBHandle dbHandle = new ArticleDBHandle();
            string topic = Convert.ToString(TempData["topicName"]);
            return View(dbHandle.GetResults(topic));
        }

        public ActionResult ShowQuestion()
        {
            ArticleDBHandle dbhandle = new ArticleDBHandle(); //Estos llamados son innecesarios, ya que los metodos de Handle podrian estar aqui
            ModelState.Clear();
            return View(dbhandle.GetQuestion(false));
        }


        public ActionResult SendQuestion()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendQuestion(QuestionModel smodel)
        {
            try
            {
                ModelState.Remove("answer");//To avoid the answer check. It cant be null but in this case it is
                if (ModelState.IsValid) //If the data is valid
                {
                    ArticleDBHandle sdb = new ArticleDBHandle();
                    if (sdb.AddQuestion(smodel, false))
                    {
                        ViewBag.Message = "Question Added Successfully";
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
        public ActionResult ModeratorQuestion()
        {
            ArticleDBHandle dbhandle = new ArticleDBHandle(); 
            ModelState.Clear();
            return View(dbhandle.GetQuestion(true));
        }

        public ActionResult PublishQuestion(int questionId)
        {
            ArticleDBHandle sdb = new ArticleDBHandle();
            return View(sdb.GetQuestion(true).Find(smodel => smodel.questionId == questionId));
        }

        [HttpPost]
        public ActionResult PublishQuestion(int questionId, QuestionModel smodel)
        {
            try
            {
                ArticleDBHandle sdb = new ArticleDBHandle();
                sdb.UpdateQuestion(smodel);
                return RedirectToAction("ModeratorQuestion");
            }
            catch
            {
                return View();
            }
        }




    }
}
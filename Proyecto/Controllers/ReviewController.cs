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

        public ActionResult recordReview()
        {
            return View();
        }


        //Iteración 3
        [HttpPost]
        public ActionResult recordReview(ReviewsModel model)
        {
            model.articleId = 1;
            //model.email = "alvAnt@puchimail.com";
            //model.comments = "Sin comentarios";
            //model.generalOpinion = 5;
            //model.communityContribution = 5;
            //model.articleStructure = 5;
            //model.totalGrade = 5;
            //model.state = "reviewed";


            ReviewDBHandle sdb = new ReviewDBHandle();
            EmailController eController = new EmailController();
            EmailModel eModel = new EmailModel();
            ProfileDBHandle pdh = new ProfileDBHandle();
            ArticleDBHandle adh = new ArticleDBHandle();
            ArticleModel aModel = new ArticleModel();


            //Saves the review on the database
            sdb.AddReview(model);

      
            string aAuthor = pdh.getArticleAuthor(model.articleId);
            string cMail = pdh.getCoordinatorMail();
            aModel = adh.getOneArticle(model.articleId);

            eModel.subject = "Revisión de artículo completada";
            eModel.message = "El artículo " + aModel.name +
                " publicado el " + aModel.publishDate +
                " por " + aAuthor +
                "ya ha sido revisado por sus revisores";
            eModel.mail = cMail;

            if (sdb.checkReviewers(model.articleId))
            {
                eController.SendMail(eModel);
            }

            return View();

        }
    }
}
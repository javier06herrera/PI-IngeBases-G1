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


        //[HttpPost]
        public ActionResult askColaboration(ArticleModel model)
        {
            EmailModel eModel = new EmailModel();
            ProfileDBHandle pdh = new ProfileDBHandle();
            List<string> emails = new List<string>();
            ArticleDBHandle adh = new ArticleDBHandle();
            EmailController eController = new EmailController();

            emails = pdh.getCoreMemberEmails();

            string aAuthor = pdh.getArticleAuthor(model.articleId);

            foreach(string email in emails)
            {
                eModel.subject = "We ask your collaboration to review an article: ";
                eModel.message = "The article " + model.name +
                    " published on " + model.publishDate +
                    " by " + aAuthor +
                    " was added to your available articles tray to review";
                eModel.mail = email;
                eController.SendMail(eModel);
            }

            adh.updateArticleState(model);

            return RedirectToAction("Colaboration");
        }


    }
}
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proyecto.Models;
using Newtonsoft.Json;

namespace Proyecto.Controllers
{
    public class AssingReviewersController : Controller
    {
        // GET: AssingReviewers
        public ActionResult AssingReviewers()
        {
            ArticleDBHandle dbhandle = new ArticleDBHandle();
            ModelState.Clear();
            return View(dbhandle.GetArticle());
        }

        public ActionResult AssingReviewersForm(ArticleModel model)
        {
            //ProfileDBHandle pdh = new ProfileDBHandle();
            //List<IsNominatedModel> nomenees = pdh.fetchNomenees(model.articleId);

            //List<string> nomenees_emails = new List<string>();
            //foreach (IsNominatedModel nomenee in nomenees)
            //{
            //    nomenees_emails.Add(nomenee.email);
            //}         
            return View(model);
        }

        [HttpPost]
        public string nomeneeEmails(int articleId)
        {
            ProfileDBHandle pdh = new ProfileDBHandle();
            List<IsNominatedModel> nomenees = pdh.fetchNomenees(articleId);

            List<string> nomenees_emails = new List<string>();
            foreach (IsNominatedModel nomenee in nomenees)
            {
                nomenees_emails.Add(nomenee.email);
            }
            return JsonConvert.SerializeObject(nomenees_emails);
        }

        [HttpPost]
        public void AssingReviewersForm(string[] checkedEmails, int articleId)
        {
            ReviewDBHandle rdh = new ReviewDBHandle();

            foreach (string email in checkedEmails)
            {
                rdh.insertReview(email, articleId);
            }

            rdh.deleteNomenees(articleId);            
        }
    }
}
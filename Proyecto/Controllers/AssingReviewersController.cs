using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proyecto.Models;

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
            ProfileDBHandle pdh = new ProfileDBHandle();
            List<IsNominatedModel> nomenees = pdh.fetchNomenees(model.articleId);

            IsNominatedModelDetail adapNomenees = new IsNominatedModelDetail();

            adapNomenees.NominatedDetails = new List<IsNominatedModel>();

            foreach (IsNominatedModel nomination in nomenees)
            {
                adapNomenees.NominatedDetails.Add(nomination);
            }

            return View(adapNomenees);
        }

        [HttpPost]
        public ActionResult AssingReviewersForm(IsNominatedModelDetail model)
        {
            ReviewDBHandle rdh = new ReviewDBHandle();

            foreach (IsNominatedModel nomination in model.NominatedDetails)
            {
                rdh.insertReview(nomination);
            }

            rdh.deleteFromTable(model.NominatedDetails.First().articleId, "IS_NOMINATED");
            return View();
        }

    }
}
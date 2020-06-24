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
        //I3: Get method to control review form, this method provides a model
        public ActionResult ReviewForm(ArticleModel article)
        {
            
            ReviewsModel reviews = new ReviewsModel();
            reviews.articleId = article.articleId;
            ViewData["Reviews"] = reviews;
            ViewData["Article"] = article;
            return View(reviews);
        }

        //I3: Post method to control review form
        [HttpPost]
        public ActionResult ReviewForm(ReviewsModel model)
        {
            string user;
            //Fetching user credentials
            if (!(Session["user"] is null)) //If someone has already sign in
            {
                user = Session["user"].ToString();
            }
            else //If no one is signed up (for developers testing) ToBeRemoved
            {
                user = "barrKev@puchimail.com";
            }
            ReviewDBHandle dbh = new ReviewDBHandle();
            model.email = user;
            dbh.registerGrades(model);
            return RedirectToAction("PendingReviews");
        }



        //I3: Controller of Pending Reviews View
        public ActionResult PendingReviews()
        {
            string user;
            ReviewDBHandle dbh = new ReviewDBHandle();

            //Fetching user credentials
            if (!(Session["user"] is null)) //If someone has already sign in
            {
                user = Session["user"].ToString();
            }
            else //If no one is signed up (for developers testing) ToBeRemoved
            {
                user = "barrKev@puchimail.com";
            }

            ViewData["PendingArticles"] = dbh.fetchPendingArticles(user);

            return View();
        }
    }
}
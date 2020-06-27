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

            EmailController eController = new EmailController();
            EmailModel eModel = new EmailModel();
            ProfileDBHandle pdh = new ProfileDBHandle();
            ArticleDBHandle adh = new ArticleDBHandle();
            ArticleModel aModel = new ArticleModel();

            string aAuthor = pdh.getArticleAuthor(model.articleId);
            string cMail = pdh.getCoordinatorMail();
            aModel = adh.getOneArticle(model.articleId);

            eModel.subject = "Article review completed: ";
            eModel.message = "The article " + aModel.name +
                " published on " + aModel.publishDate +
                " by " + aAuthor +
                "has been reviewed";
            eModel.mail = cMail;

            if (dbh.checkReviewers(model.articleId))
            {
                eController.SendMail(eModel);
            }

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

        //I3 Collects all articles that are ready to be veredicted
        public ActionResult PendingVeredicts()
        {
            ReviewDBHandle reviewDBHandle = new ReviewDBHandle();
            ViewData["PendingVeredicts"] = reviewDBHandle.fetchVeredictArticles();

            return View();
        }

        //I3 Returns view with all reviews for this article
        public ActionResult GiveVeredict(ArticleModel aModel)
        {
            ReviewDBHandle reviewDBHandle = new ReviewDBHandle();
            ViewData["CurrentVeredict"] = reviewDBHandle.collectReviews(aModel.articleId);
            ViewData["VeredictId"] = aModel.articleId;

            return View();
        }

        //I3 Accept a veredict to set article into published
        public ActionResult acceptVeredict(ReviewsModel rModel)
        {
            //Cast parameter
            int artId = rModel.articleId;

            //Set the article status to 'published' 
            ArticleDBHandle aDBH = new ArticleDBHandle();
            aDBH.acceptArticle(artId);

            //Removes old reviews for this article
            ReviewDBHandle rDBH = new ReviewDBHandle();
            rDBH.removeReviews(artId);

            //Author gets merits according to the average of its grade, plus extra from its member rank if applies


            //Send accepted notification to authors
            sendAuthorMails(artId, "accepted");

            return RedirectToAction("HomePage", "Article", null);
        }

        //I3 Accept veredict with modifications to article sets it to on edition
        public ActionResult acceptModVeredict(ReviewsModel rModel)
        {
            //Cast Parameter
            int artId = rModel.articleId;

            ArticleDBHandle aDBH = new ArticleDBHandle();
            aDBH.rejectArticle(artId);

            //Removes old reviews for this article
            ReviewDBHandle rDBH = new ReviewDBHandle();
            rDBH.resetReviews(artId);

            //Send accepted with modification notification to authors
            sendAuthorMails(artId, "accepted with modifications");

            //Update merits for published article
            grantMerits(artId);

            return RedirectToAction("HomePage", "Article", null);
        }

        //I3 Rejection veredict
        public ActionResult rejectVeredict(ReviewsModel rModel)
        {
            //Cast Parameter
            int artId = rModel.articleId;

            //Resets checkStatus for this article to 'on edition'
            ArticleDBHandle aDBH = new ArticleDBHandle();
            aDBH.rejectArticle(artId);
    
            //Removes old reviews for this article
            ReviewDBHandle rDBH = new ReviewDBHandle();
            rDBH.removeReviews(artId);

            //Send reject notification to authors
            sendAuthorMails(artId, "rejected");

            return RedirectToAction("HomePage", "Article", null);
        }

        //I3: Sends notification to all authors when article is accepted
        public void sendAuthorMails(int artId, string veredict)
        { 
            EmailModel eModel = new EmailModel();
            EmailController eController = new EmailController();
            ArticleDBHandle aDBH = new ArticleDBHandle();
            ArticleModel aModel = aDBH.getOneArticle(artId);

            //Send notification mail to separate authors
            List<string> authors = aDBH.getAuthors(artId);

                foreach (string author in authors)
                {
                    eModel.subject = "One of your articles has been " + veredict + "!";
                    eModel.mail = author;
                    eModel.message = "Your article " + aModel.name + " has been "+ veredict + 
                        "by the community coordinator, go check it out in your profile!";

                    eController.SendMail(eModel);
                }
        }

        //I3: Grants merits to all writers of a given accepted article
        public void grantMerits(int artId)
        {
            //Collect authors list
            ArticleDBHandle aDBH = new ArticleDBHandle();
            List<string> authors = aDBH.getAuthors(artId);

            int meritsGiven = 0;
            
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proyecto.Models;
using Proyecto.Controllers;

namespace Proyecto.Controllers
{
    public class ReviewController : Controller
    {
        //I3: Get method to control review form, this method provides a model
        public ActionResult ReviewForm(ArticleModel article)
        {
            ReviewsModel reviews = new ReviewsModel();
            reviews.articleId = article.articleId;
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

            ModelState.Remove("options");
            ModelState.Remove("state");
            ModelState.Remove("totalGrade");
            ModelState.Remove("optionList");
            ModelState.Remove("email");
            try {
                if (ModelState.IsValid) {
                    dbh.registerGrades(model); }
                else {
                    ViewBag.Message = "Creation of review failed";
                    return View(model);
                }
            }
            catch {
                ViewBag.Message = "Creation of review failed";
                return View(model);
            }


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

            return RedirectToAction("PendingReviews", "Review");
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
            aDBH.updateArticleStatus(artId, "published");
   
            //Author gets merits according to the average of its grade, plus extra from its member rank if applies
            grantMerits(artId);

            //Removes old reviews for this article
            ReviewDBHandle rDBH = new ReviewDBHandle();
            rDBH.deleteFromTable(artId, "REVIEWS");

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
            aDBH.updateArticleStatus(artId, "resend");

            //Removes old reviews for this article
            ReviewDBHandle rDBH = new ReviewDBHandle();
            rDBH.resetReviews(artId);

            //Send accepted with modification notification to authors
            sendAuthorMails(artId, "accepted with modifications");

            return RedirectToAction("HomePage", "Article", null);
        }

        //I3 Rejection veredict
        public ActionResult rejectVeredict(ReviewsModel rModel)
        {
            //Cast Parameter
            int artId = rModel.articleId;

            //Resets checkStatus for this article to 'on edition'
            ArticleDBHandle aDBH = new ArticleDBHandle();
            aDBH.updateArticleStatus(artId, "on edition");

            //Removes old reviews for this article
            ReviewDBHandle rDBH = new ReviewDBHandle();
            rDBH.deleteFromTable(artId, "REVIEWS");

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
            eModel.subject = "One of your articles has been " + veredict + "!";
            eModel.message = "Your article " + aModel.name + " has been " + veredict +
                             " by the community coordinator, go check out your profile!\n";

            //If article was accepted with modifications, also add reviewers comments 
            if (veredict == "accepted with modifications")
            {
                eModel.message += fetchReviewerComments(artId);
            }

            //For each destinatary notify veredict result
            foreach (string author in authors)
            {
                eModel.mail = author;
                eController.SendMail(eModel);
            }
        }

        //I3: Calculates and updates merits for given accepted article
        public void grantMerits(int artId)
        {
            //Collect authors list
            ArticleDBHandle aDBH = new ArticleDBHandle();
            ReviewDBHandle rDBH = new ReviewDBHandle();
            List<string> authors = aDBH.getAuthors(artId);

            //Numerator (contains the grade * member merits)
            int meritsGiven = rDBH.sumTotalGrades(authors.Count(), artId);

            //Denominator (contains the sum of all reviewers merits)
            int sumOfMerits = 0;
          
            foreach (string author in authors)
            {
                sumOfMerits += rDBH.fetchMerits(author);
            }

            //if (sumOfMerits == 0)
            //    sumOfMerits = 1;

            meritsGiven = meritsGiven/sumOfMerits;

            //For each author of this article, update their merits
            ProfileDBHandle pDBH = new ProfileDBHandle();
            foreach (string author in authors)
            {
                pDBH.updateMerits(author, meritsGiven);
            }

            //Finally,set the article base grade to the calculated grade 
            aDBH.updateBaseGrade(artId, meritsGiven);

        }

        public string fetchReviewerComments(int artId)
        {
            string comments = "\n";
            ReviewDBHandle rDBH = new ReviewDBHandle();

            comments += rDBH.addReviewerComments(comments, artId);
            
            return comments;
        }

        public ActionResult ReviewRequests()
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

            ViewData["ArticleRequest"] = dbh.fetchArticleReviewRequest(user);

            return View();

        }

        public ActionResult AnswerReviewRequest(ArticleModel article)
        {
            IsNominatedModel nomination = new IsNominatedModel();
            nomination.articleId = article.articleId;
            //Fetching user credentials
            if (!(Session["user"] is null)) //If someone has already sign in
            {
                nomination.email = Session["user"].ToString();
            }
            else //If no one is signed up (for developers testing) ToBeRemoved
            {
                nomination.email = "barrKev@puchimail.com";
            }

            ViewData["Article"] = article;
            return View(nomination);
        }

        //I3: Mthod that controls wath the answer view retuns
        [HttpPost]
        public ActionResult AnswerReviewRequest(IsNominatedModel nomination)
        {
            ReviewDBHandle dbh = new ReviewDBHandle();

            if (string.IsNullOrEmpty(nomination.comments))
            {
                nomination.comments = "No comments";
            }
            //Fetching user credentials
            if (!(Session["user"] is null)) //If someone has already sign in
            {
                nomination.email = Session["user"].ToString();
            }
            else //If no one is signed up (for developers testing) ToBeRemoved
            {
                nomination.email = "barrKev@puchimail.com";
            }
            dbh.registerRequestAnswer(nomination);
            return RedirectToAction("ReviewRequests");

        }

        public ActionResult EnrollOnReviewProcess(int articleId, string checkedStatus)
        {
            
            ReviewDBHandle dbh = new ReviewDBHandle();
            dbh.setNewArticleStatus(articleId, checkedStatus);

            return Redirect("/Profile/Profile");
        }

    }
}
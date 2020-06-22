using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace Proyecto.Models
{
    public class ReviewDBHandle
    {
        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["ArticleConn"].ToString();
            con = new SqlConnection(constring);
        }

        // Se agrega un nuevo articulo
        public bool AddReview(ReviewsModel smodel)
        {
            connection();
            string AddNewArticle = "INSERT INTO REVIEWS " +
                                   "VALUES (@articleId, @email, @comments ,@generalOpinion ,@communityContribution,@articleStructure,@totalGrade,@state)";
            SqlCommand cmd = new SqlCommand(AddNewArticle, con); // Nombre procedimiento, 

            cmd.Parameters.AddWithValue("@articleId", smodel.articleId);
            cmd.Parameters.AddWithValue("@email", smodel.email);
            cmd.Parameters.AddWithValue("@comments", smodel.comments);
            cmd.Parameters.AddWithValue("@generalOpinion", smodel.generalOpinion);
            cmd.Parameters.AddWithValue("@communityContribution", smodel.communityContribution);
            cmd.Parameters.AddWithValue("@articleStructure", smodel.communityContribution);
            cmd.Parameters.AddWithValue("@articleStructure", smodel.articleStructure);
            cmd.Parameters.AddWithValue("@totalGrade", smodel.totalGrade);
            cmd.Parameters.AddWithValue("@state", smodel.state);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

     
            if (i >= 1)
                return true;
            else
                return false;
        }

        public bool checkReviewers(int articleId)
        {
            List<ReviewsModel> allReviewsList = new List<ReviewsModel>();
            List<ReviewsModel> reviewedReviewsList = new List<ReviewsModel>();

  

            //Fetch of the entire list of reviews
            connection();
            string fetchReviews = "SELECT R.articleId, R.email, R.state " +
                                   "FROM REVIEWS R " +
                                   "WHERE R.articleId = @articleId";

            SqlDataAdapter sd1 = new SqlDataAdapter(fetchReviews, con);
            DataTable reviewTable = new DataTable();

            sd1.InsertCommand.Parameters.Add("@articleId",
            SqlDbType.BigInt, articleId, "articleId");

            con.Open();
            sd1.Fill(reviewTable);
            foreach (DataRow review in reviewTable.Rows)
            {
                allReviewsList.Add(
                    new ReviewsModel
                    {
                        articleId = Convert.ToInt32(review["articleId"]),
                        email = Convert.ToString(review["email"]),
                        state = Convert.ToString(review["state"])
                    });
            }
            con.Close();

            //Fetch the list of reviews where the state

            connection();
            fetchReviews = "SELECT R.articleId, R.email, R.state " +
                                   "FROM REVIEWS R " +
                                   "WHERE R.articleId = @articleId " +
                                   "AND R.state = 'reviewed'";

            sd1 = new SqlDataAdapter(fetchReviews, con);
            reviewTable = new DataTable();

            sd1.InsertCommand.Parameters.Add("@articleId",
            SqlDbType.BigInt, articleId, "articleId");

            con.Open();
            sd1.Fill(reviewTable);
            foreach (DataRow review in reviewTable.Rows)
            {
                reviewedReviewsList.Add(
                    new ReviewsModel
                    {
                        articleId = Convert.ToInt32(review["articleId"]),
                        email = Convert.ToString(review["email"]),
                        state = Convert.ToString(review["state"])
                    });
            }
            con.Close();

            if (allReviewsList.Count == reviewedReviewsList.Count)
                return true;
            else
                return false;

        }

        //I3: This method is used to merge all the topics of a file into a single string
        public string topicMerge(int articleId, DataTable topicList)
        {
            string topicsLine = "";
            foreach (DataRow topic in topicList.Rows)
            {
                if (Convert.ToInt32(topic["articleId"]) == articleId)
                {
                    topicsLine = topicsLine + topic["category"] + ":" + topic["topicName"] + ", ";
                }
            }
            topicsLine.Remove(topicsLine.Length - 2, 1);
            return topicsLine;
        }



        //I3: This method returns all the pending articles to be reviewed by a core member
        public List<ArticleModel> fetchPendingArticles(string reviewerEmail)
        {
            //Stablishes a connection string
            connection();
            List<ArticleModel> articleList = new List<ArticleModel>();

            //Fetching Query of a list composed by articles pending to be reviewed for a specific member
            string fetchArticles = "SELECT * " +
                                   "FROM Article A " +
                                   "JOIN Reviews R ON A.articleId = R.ArticleId " +
                                   "WHERE R.email = @email " +
                                   "AND R.state = 'not reviewed' ";
            //DB connection arrangement
            SqlCommand cmd = new SqlCommand(fetchArticles, con);
            cmd.Parameters.AddWithValue("@email", reviewerEmail);
            SqlDataAdapter sd1 = new SqlDataAdapter(cmd);
            DataTable articleTable = new DataTable();

            //Open connection with the DB
            con.Open();
            //Buffer of data from the DB
            sd1.Fill(articleTable);
            //Loop that formats data into an array 
            foreach (DataRow article in articleTable.Rows)
            {
                articleList.Add(
                    new ArticleModel
                    {
                        articleId = Convert.ToInt32(article["articleId"]),
                        name = Convert.ToString(article["name"]),
                        topicName = " ",
                        Abstract = Convert.ToString(article["Abstract"]),
                        publishDate = Convert.ToString(article["publishDate"]),
                        content = Convert.ToString(article["content"]),
                        type = Convert.ToString(article["type"]),
                        baseGrade = Convert.ToInt32(article["baseGrade"]),
                        accessCount = Convert.ToInt32(article["accessCount"]),
                        likesCount = Convert.ToInt32(article["likesCount"]),
                        neutralCount = Convert.ToInt32(article["neutralCount"]),
                        dislikesCount = Convert.ToInt32(article["dislikesCount"]),
                        likeBalance = Convert.ToInt32(article["likeBalance"])
                    });
            }
            con.Close();

            //Fetch of the entire list of topics
            string fetchTopics = "SELECT * " +
                                 "FROM   INVOLVES " +
                                 "WHERE  articleId in ( " +
                                            "SELECT articleId " +
                                            "FROM   REVIEWS " +
                                            "WHERE  email = @email " +
                                            "AND    state = 'not reviewed') " +
                                 "ORDER BY articleId";
            cmd.CommandText = fetchTopics;
            cmd.Parameters["@email"].Value =reviewerEmail;
            SqlDataAdapter sd2 = new SqlDataAdapter(cmd);
            DataTable topicList = new DataTable();
            con.Open();
            sd2.Fill(topicList);
            foreach (ArticleModel article in articleList)
            {
                article.topicName = topicMerge(article.articleId, topicList);
            }
            con.Close();
            return articleList;
        }
    }
}
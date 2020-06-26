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

        //I3: Must replace conn and cmd definitions
        private DBConnectHanler conn = new DBConnectHanler();

        
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
            List<ReviewsModel> completedReviewsList = new List<ReviewsModel>();

            //Fetch of the entire list of reviews
            connection();
            string fetchReviews = "SELECT R.articleId, R.email, R.state " +
                                  "FROM REVIEWS R " +
                                  "WHERE R.articleId = @articleId";



            SqlCommand command = new SqlCommand(fetchReviews, con);
            command.Parameters.AddWithValue("@articleId", articleId);
                      

            con.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    allReviewsList.Add(
                    new ReviewsModel
                    {
                        articleId = Convert.ToInt32(reader["articleId"]),
                        email = Convert.ToString(reader["email"]),
                        state = Convert.ToString(reader["state"])
                    });
                }

            }
            con.Close();

            //Fetch the list of reviews where the state

            connection();
            fetchReviews = "SELECT R.articleId, R.email, R.state " +
                                   "FROM REVIEWS R " +
                                   "WHERE R.articleId = @articleId " +
                                   "AND R.state = 'reviewed'";

            command = new SqlCommand(fetchReviews, con);
            command.Parameters.AddWithValue("@articleId", articleId);


            con.Open();
            reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    completedReviewsList.Add(
                    new ReviewsModel
                    {
                        articleId = Convert.ToInt32(reader["articleId"]),
                        email = Convert.ToString(reader["email"]),
                        state = Convert.ToString(reader["state"])
                    });
                }

            }
            con.Close();

            if (allReviewsList.Count == completedReviewsList.Count)
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
            
            SqlDataAdapter sd1 = new SqlDataAdapter(cmd);
            DataTable articleTable = new DataTable();
            cmd.Parameters.AddWithValue("@email", reviewerEmail);
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
                        likeBalance = Convert.ToInt32(article["likeBalance"]),
                        checkedStatus = Convert.ToString(article["checkedStatus"])
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
            cmd.Parameters["@email"].Value = reviewerEmail;
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


        public List<ArticleModel> fetchVeredictArticles()
        {
            //Stablishes a connection string
            connection();
            List<ArticleModel> veredictArticleList = new List<ArticleModel>();

            string fetchArticles = "SELECT * " +
                                   "FROM Article A " +                                  
                                   "WHERE A.checkedStatus = 'not checked' ";

            //DB connection arrangement
            SqlCommand cmd = new SqlCommand(fetchArticles, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable articleTable = new DataTable();

            con.Open();
            sda.Fill(articleTable);

            foreach (DataRow article in articleTable.Rows)
            {

                int artId = Convert.ToInt32(article["articleId"]);
                //Add only articles that are ready to be given a veredict
                if (checkReviewers(artId) == true)
                {
                    veredictArticleList.Add(
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
                            likeBalance = Convert.ToInt32(article["likeBalance"]),
                            checkedStatus = Convert.ToString(article["checkedStatus"])
                        });
                }
            }

            con.Close();

            //Retrieve topics of the list topics
            string fetchTopics = "SELECT * " +
                     "FROM   INVOLVES " +
                     "WHERE  articleId in ( " +
                                "SELECT articleId " +
                                "FROM   REVIEWS " +
                                "WHERE  state = 'reviewed') " +                                
                     "ORDER BY articleId";

            cmd.CommandText = fetchTopics;
            SqlDataAdapter sda2 = new SqlDataAdapter(cmd);
            DataTable topicList = new DataTable();
            con.Open();
            sda2.Fill(topicList);

            foreach (ArticleModel article in veredictArticleList)
            {
               article.topicName = topicMerge(article.articleId, topicList);
            }

            con.Close();
            return veredictArticleList;
		}
        
        public bool registerGrades(ReviewsModel model)
        {

            SqlCommand cmd = conn.setWritingQuery(  "UPDATE Reviews " +
                                                    "SET    comments = @comments, " +
                                                    "       generalOpinion = @generalOpinion, " +
                                                    "       communityContribution = @communityContribution, " +
                                                    "       articleStructure = @articleStructure, " +
                                                    "       totalGrade = @totalGrade, " +
                                                    "       state = @state " +
                                                    "WHERE  articleId = @articleId " +
                                                    "AND    email = @email");
            cmd.Parameters.AddWithValue("@comments",model.comments);
            cmd.Parameters.AddWithValue("@generalOpinion", model.generalOpinion);
            cmd.Parameters.AddWithValue("@communityContribution", model.communityContribution);
            cmd.Parameters.AddWithValue("@articleStructure", model.articleStructure);
            cmd.Parameters.AddWithValue("@totalGrade", model.totalGrade);
            cmd.Parameters.AddWithValue("@state", "reviewed");
            cmd.Parameters.AddWithValue("@articleId",model.articleId);
            cmd.Parameters.AddWithValue("@email",model.email);

            conn.conn.Open();
            cmd.ExecuteNonQuery();
            conn.conn.Close();
            return true;

        }
    }
}
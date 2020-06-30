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

        //I3: Fetches all articles ready to be given a veredict
        public List<ArticleModel> fetchVeredictArticles()
        {
            //Stablishes a connection string
            connection();
            List<ArticleModel> veredictArticleList = new List<ArticleModel>();        

            //DB connection arrangement
            string fetchArticles = "SELECT * " + "FROM Article A " + "WHERE A.checkedStatus = 'not checked' ";
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
            string fetchTopics = "SELECT * " + "FROM   INVOLVES " +
                     "WHERE  articleId in ( " +  "SELECT articleId " +
                    "FROM   REVIEWS " + "WHERE  state = 'reviewed') " + "ORDER BY articleId";

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
            model.totalGrade = fetchMerits(model.email) * (model.generalOpinion + model.communityContribution + model.articleStructure);
            SqlCommand cmd = conn.setSimpleReturnQuery(  "UPDATE Reviews " +
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

        //I3: Collects all reviews for a given article
        public List<ReviewsModel> collectReviews(int articleId)
        {
            connection();
            List<ReviewsModel> reviewsList = new List<ReviewsModel>();

            //Collects all completed reviews from this article
            string fetchReviews = "SELECT * " +
                                  "FROM REVIEWS R " +
                                  "WHERE R.articleId = @articleId " +
                                  "AND R.state = 'reviewed' ";

            SqlCommand cmd = new SqlCommand(fetchReviews, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable reviewsTable = new DataTable();

            //Retrieve data from DB
            cmd.Parameters.AddWithValue("@articleId", articleId);
            con.Open();
            sda.Fill(reviewsTable);

            //Loop to format collected data into table
            foreach (DataRow review in reviewsTable.Rows)
            {
                reviewsList.Add(
                    new ReviewsModel {
                        articleId = Convert.ToInt32(review["articleId"]),
                        email = Convert.ToString(review["email"]),
                        comments = Convert.ToString(review["comments"]),
                        generalOpinion= Convert.ToInt32(review["generalOpinion"]),
                        communityContribution = Convert.ToInt32(review["communityContribution"]),
                        articleStructure = Convert.ToInt32(review["articleStructure"]),
                        totalGrade = Convert.ToInt32(review["totalGrade"]),
                        state = Convert.ToString(review["state"])
                    }                    
                );
            }

            con.Close();
            return reviewsList;
        }

        //I3: When accepted with modification, reviews are restarted for the same reviewers       
        public void resetReviews(int articleId)
        {
            connection();
            String query = "UPDATE REVIEWS " +
                           "SET state = 'not reviewed' " +
                           "WHERE articleId = @articleId ";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@articleId", articleId);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
        }

        //I3: Returns the merits asociated for a given member
        public int fetchMerits(string memberEmail)
        {
            SqlCommand cmd = conn.setSimpleReturnQuery("SELECT points " +
                                                        "FROM CommunityMember " +
                                                        "WHERE email = @email");
            cmd.Parameters.AddWithValue("@email", memberEmail);
            conn.conn.Open();
            int merits = Convert.ToInt32( cmd.ExecuteScalar());
            conn.conn.Close();
            return merits;
        }

        //I3: Returns sum of all totalGrades assigned by reviewers for a given article
        public int sumTotalGrades(int authorCount, int articleId)
        {
            int merits = 0;

            connection();

            string query = "SELECT * " +
                           "FROM REVIEWS " +
                           "WHERE articleId = @articleId";

            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            cmd.Parameters.AddWithValue("@articleId", articleId);
            SqlDataReader reader;

            con.Open();
            reader = cmd.ExecuteReader();
            //For each review sum the given grade
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    merits += Convert.ToInt32(reader["totalGrade"]);
                }
            }

            con.Close();

            return merits;
        }

        public string addReviewerComments(string comments, int articleId)
        {
            connection();

            string query = "SELECT * " +
                           "FROM REVIEWS R JOIN " +
                           "CommunityMember CM ON " +
                           "R.email = CM.email " +
                           "WHERE articleId = @articleId";

            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable reviewsTable = new DataTable();

            cmd.Parameters.AddWithValue("@articleId", articleId);
            con.Open();
            sda.Fill(reviewsTable);

            //For each review, add to the string comments the comment of the respective reviewer
            foreach (DataRow review in reviewsTable.Rows)
            {
                comments += "Reviewer " + Convert.ToString(review["name"]) + " " +
                             Convert.ToString(review["lastName"]) + " says: " +
                             Convert.ToString(review["comments"]) + "\n";
            }

            con.Close();


            return comments;
        }

        public List<ArticleModel> fetchArticleReviewRequest(string reviewerEmail)
        {
            //Stablishes a connection string
            connection();
            List<ArticleModel> articleList = new List<ArticleModel>();

            //Fetching Query of a list composed by articles pending to be reviewed for a specific member
            string fetchArticles = "SELECT * " +
                                   "FROM Article A " +
                                   "JOIN IS_NOMINATED I ON A.articleId = I.articleId " +
                                   "WHERE I.email = @email " +
                                   "AND answer = 'pending' ";
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
                                 "FROM   INVOLVES ";
            cmd.CommandText = fetchTopics;
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

        //I3: This method register the answer of a request provided by a core member
        public bool registerRequestAnswer(IsNominatedModel nomination)
        {
            SqlCommand cmd = conn.setSimpleReturnQuery("UPDATE IS_NOMINATED " +
                                                        "SET answer = @answer, " +
                                                        "comments = @comments " +
                                                        "WHERE articleId = @articleId " +
                                                        "AND email = @email ");
            cmd.Parameters.AddWithValue("@answer", nomination.answer);
            cmd.Parameters.AddWithValue("@comments", nomination.comments);
            cmd.Parameters.AddWithValue("@articleId", nomination.articleId);
            cmd.Parameters.AddWithValue("@email", nomination.email);

            conn.conn.Open();
            cmd.ExecuteNonQuery();
            conn.conn.Close();

            return true;
        }

        //I3: This method turn article status into pending collaboration so the reviewing process can begin
        public void setNewArticleStatus(int articleId, string checkedStatus)
        {
            string status;
            if (checkedStatus == "resend")
            {
                status = "'not checked' ";
            }
            else
            {
                status = "'pending collaboration' ";
            }
            SqlCommand cmd = conn.setSimpleReturnQuery("UPDATE Article " +
                                                        "SET checkedStatus = "+ status  +
                                                        "WHERE articleId = @articleId");
            cmd.Parameters.AddWithValue("@articleid", articleId);

            conn.conn.Open();
            cmd.ExecuteNonQuery();
            conn.conn.Close();

        }

        public void insertReview(IsNominatedModel nomination)
        {
            connection();
            string AddNewArticle = "INSERT INTO REVIEWS " +
                                   "VALUES (@articleId, @email,'Sin comentarios',DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT)";
            SqlCommand cmd = new SqlCommand(AddNewArticle, con);

            cmd.Parameters.AddWithValue("@articleId", nomination.articleId);
            cmd.Parameters.AddWithValue("@email", nomination.email);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

        }

        public void deleteFromTable(int articleId, string tableName)
        {
            connection();
            String query = "DELETE FROM @tableName " +
                           "WHERE articleId = @articleId ";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@articleId", articleId);
            cmd.Parameters.AddWithValue("@tableName", tableName);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}
    
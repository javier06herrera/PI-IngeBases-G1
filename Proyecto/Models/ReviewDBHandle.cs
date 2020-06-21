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
    }
}
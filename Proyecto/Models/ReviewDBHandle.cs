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
    }
}
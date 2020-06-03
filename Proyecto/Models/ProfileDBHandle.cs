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
    public class ProfileDBHandle
    {
        private SqlConnection connection;
        private SqlCommand command;
        private SqlDataReader reader;

        public ProfileDBHandle()
        {
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ArticleConn"].ToString());
        }

        public ProfileModel getMemberProfile(int memberId)
        {
            
            ProfileModel memberProfile = new ProfileModel();
            string query = "SELECT * " +
                            "FROM CommunityMember CM " +
                            "WHERE CM.memberId = @memberId";

            using (connection)
            {
                command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("memberId", memberId);
                connection.Open();
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        memberProfile.memberId = Convert.ToInt32(reader["memberId"]);
                        memberProfile.name = Convert.ToString(reader["name"]);
                        memberProfile.lastName = Convert.ToString(reader["lastName"]);
                        memberProfile.birthDate = Convert.ToString(reader["birthDate"]);
                        memberProfile.age = Convert.ToInt32(reader["age"]);
                        memberProfile.addressCity = Convert.ToString(reader["addressCity"]);
                        memberProfile.addressCountry = Convert.ToString(reader["addressCountry"]);
                        memberProfile.hobbies = Convert.ToString(reader["hobbies"]);
                        memberProfile.languages = Convert.ToString(reader["languages"]);
                        memberProfile.email = Convert.ToString(reader["email"]);
                        memberProfile.mobile = Convert.ToString(reader["mobile"]);
                        memberProfile.job = Convert.ToString(reader["job"]);
                        memberProfile.memberRank = Convert.ToString(reader["memberRank"]);
                        memberProfile.points = Convert.ToInt32(reader["points"]);
                    }
                }

                reader.Close();
            }
            SqlConnection connection1 = new SqlConnection(ConfigurationManager.ConnectionStrings["ArticleConn"].ToString());
            query = "SELECT COUNT(*) " +
                   "FROM  WRITES " +
                   "WHERE memberId = @memberId";

            SqlCommand cmd1 = new SqlCommand(query, connection1);
            cmd1.Parameters.AddWithValue("@memberId", memberId);
            connection1.Open();
            memberProfile.articleCount = Convert.ToInt32(cmd1.ExecuteScalar());
            connection1.Close();

            return memberProfile;

        }


        public ProfileModel AddProfile(ProfileModel pmodel)
        {

            string query = "INSERT INTO CommunityMember (name, lastName, addressCity, addressCountry, email, mobile, job, skills) " +
                           "VALUES(@name, @lastName, @addressCity, @addressCountry, @email, @mobile, @job, @skills)";

            command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@name", pmodel.name);
            command.Parameters.AddWithValue("@lastName", pmodel.lastName);
            command.Parameters.AddWithValue("@addressCity", pmodel.addressCity);
            command.Parameters.AddWithValue("@addressCountry", pmodel.addressCountry);
            command.Parameters.AddWithValue("@mobile", pmodel.mobile);
            command.Parameters.AddWithValue("@email", pmodel.email);
            command.Parameters.AddWithValue("@job", pmodel.job);
            command.Parameters.AddWithValue("@skills", pmodel.skills);

            using (connection)
            {
                connection.Open();
                int codeError = command.ExecuteNonQuery();
                connection.Close();
            }

            return pmodel;
        }
        public bool updateMerits(int articleId, bool valueSign)
        {
            //Brings all the member that are responsible for the article
            string query = "SELECT memberId " +
                           "FROM WRITES " +
                           "WHERE articleId = @articleId";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@articleId", articleId);
            SqlDataAdapter sd1 = new SqlDataAdapter(cmd);
            DataTable membersTemp = new DataTable();
            connection.Open();
            sd1.Fill(membersTemp);
            List<int> members= new List<int>();
            foreach (DataRow dr in membersTemp.Rows)
            {
                members.Add(Convert.ToInt32(dr[0]));
            }
            connection.Close();

            //For each of the members updates the point values
            if (valueSign)
            {
                query = "UPDATE CommunityMember " +
                        "SET points = points + 1 " +
                        "WHERE memberId = @memberId";
            }
            else
            {
                query = "UPDATE CommunityMember " +
                        "SET points = points - 1 " +
                        "WHERE memberId = @memberId";
            }

            SqlCommand cmd1 = new SqlCommand(query, connection);
            cmd1.Parameters.AddWithValue("@memberId", members[0]);
            connection.Open();
            int i;
            i = cmd1.ExecuteNonQuery();
            for (int j = 1; j < members.Count;j++)
            {
                cmd1.Parameters["@memberId"].Value = members[j];
                i = cmd1.ExecuteNonQuery();
                if (i < 0)
                    return false;
            }
            connection.Close();
            return true;
            

        }

        //Combines all the topics of an Article
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


        // Ver resultados de busqueda
        public List<ArticleModel> fetchMyArticles(int memberId)
        {
            List<ArticleModel> articleList = new List<ArticleModel>();


            //Fetch of the entire list of articles without topics
            string fetchArticles = "SELECT * " +
                                   "FROM Article A " +
                                   "JOIN WRITES W ON A.articleId = W.ArticleId "+
                                   "WHERE W.memberId = @memberId "+
                                   "ORDER BY publishDate DESC";
            SqlCommand cmd = new SqlCommand(fetchArticles, connection);
            cmd.Parameters.AddWithValue("@memberId", memberId);
            SqlDataAdapter sd1 = new SqlDataAdapter(cmd);
            DataTable articleTable = new DataTable();
            connection.Open();
            sd1.Fill(articleTable);
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
                        dislikesCount = Convert.ToInt32(article["dislikesCount"]),
                        likeBalance = Convert.ToInt32(article["likeBalance"])
                    });
            }
            connection.Close();

            //Fetch of the entire list of topics
            string fetchTopics = "SELECT * " +
                                 "FROM INVOLVES";
            SqlDataAdapter sd2 = new SqlDataAdapter(fetchTopics, connection);
            DataTable topicList = new DataTable();
            connection.Open();
            sd2.Fill(topicList);
            foreach (ArticleModel article in articleList)
            {
                article.topicName = topicMerge(article.articleId, topicList);
            }
            connection.Close();
            return articleList;
        }
    }
}
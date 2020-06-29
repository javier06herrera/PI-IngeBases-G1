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
        private SqlConnection con;
        private SqlCommand cmd;
        private SqlDataReader reader;

        public ProfileDBHandle()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["ArticleConn"].ToString());
        }

        public ProfileModel getMemberProfile(String email)
        {

            ProfileModel memberProfile = new ProfileModel();
            //string query = "SELECT * " +
            //                "FROM CommunityMember CM " +
            //                "WHERE CM.email = @email";


            cmd = new SqlCommand("PISP_getMemberProfile", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@email", email);
            con.Open();
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    memberProfile.email = Convert.ToString(reader["email"]);
                    memberProfile.name = Convert.ToString(reader["name"]);
                    memberProfile.lastName = Convert.ToString(reader["lastName"]);
                    memberProfile.birthDate = Convert.ToString(reader["birthDate"]);
                    memberProfile.age = Convert.ToInt32(reader["age"]);
                    memberProfile.addressCity = Convert.ToString(reader["addressCity"]);
                    memberProfile.addressCountry = Convert.ToString(reader["addressCountry"]);
                    memberProfile.hobbies = Convert.ToString(reader["hobbies"]);
                    memberProfile.languages = Convert.ToString(reader["languages"]);
                    memberProfile.mobile = Convert.ToString(reader["mobile"]);
                    memberProfile.job = Convert.ToString(reader["job"]);
                    memberProfile.memberRank = Convert.ToString(reader["memberRank"]);
                    memberProfile.points = Convert.ToInt32(reader["points"]);
                    memberProfile.articleCount = Convert.ToInt32(reader["articleCount"]);
                }
            }

            reader.Close();

            //SqlConnection connection1 = new SqlConnection(ConfigurationManager.ConnectionStrings["ArticleConn"].ToString());
            //query = "SELECT COUNT(*) " +
            //       "FROM  WRITES " +
            //       "WHERE email = @email";

            //SqlCommand cmd1 = new SqlCommand(query, connection1);
            //cmd1.Parameters.AddWithValue("@email", email);
            //connection1.Open();
            //memberProfile.articleCount = Convert.ToInt32(cmd1.ExecuteScalar());
            //connection1.Close();

            return memberProfile;

        }


        public ProfileModel AddProfile(ProfileModel pmodel)
        {

            string query = "INSERT INTO CommunityMember (email,name, lastName, addressCity, addressCountry, mobile, job, skills) " +
                           "VALUES(@email,@name, @lastName, @addressCity, @addressCountry, @mobile, @job, @skills)";

            cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@email", pmodel.email);
            cmd.Parameters.AddWithValue("@name", pmodel.name);
            cmd.Parameters.AddWithValue("@lastName", pmodel.lastName);
            cmd.Parameters.AddWithValue("@addressCity", pmodel.addressCity);
            cmd.Parameters.AddWithValue("@addressCountry", pmodel.addressCountry);
            cmd.Parameters.AddWithValue("@mobile", pmodel.mobile);
            cmd.Parameters.AddWithValue("@job", pmodel.job);
            cmd.Parameters.AddWithValue("@skills", pmodel.skills);

            using (con)
            {
                con.Open();
                int codeError = cmd.ExecuteNonQuery();
                con.Close();
            }

            return pmodel;
        }

        //I3: Checks if credentials of login are correct
        public bool attemptLogin(ProfileModel pmodel)
        {
            //Checks if credentials enttered in login page match credentials of any user
            string query = "SELECT * " +
                           "FROM CommunityMember CM " +
                           "WHERE CM.email = @email " +
                           "AND CM.password = @password ";


            bool result = false;                     
            command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@email", pmodel.email);
            command.Parameters.AddWithValue("@password", pmodel.password);
            connection.Open();

            //If password is detected as unsafe, returns false and prevents malicious login
            bool securePassword = checkPasswordSafety(pmodel.password);
            if (!securePassword)
            {
                return false;
            }

           reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    //If query returns a row, then credentials matched in database
                    result = true;
                    pmodel.memberRank = Convert.ToString(reader["memberRank"]);
                }
            }

            reader.Close();
            con.Close();

            return result;
        }

        //I3: Checks if password contains blank space making possible an easy SQLInjection to always login
        public bool checkPasswordSafety(string password)
        {
            bool safe;

            if (password.Contains(" "))
            {
                safe = false;
            }
            else
            {
                safe = true;
            }

            return safe;
        }

        public bool updateMerits(int articleId, bool valueSign)
        {
            //Brings all the member that are responsible for the article
            string query = "SELECT email " +
                           "FROM WRITES " +
                           "WHERE articleId = @articleId";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@articleId", articleId);
            SqlDataAdapter sd1 = new SqlDataAdapter(cmd);
            DataTable membersTemp = new DataTable();
            con.Open();
            sd1.Fill(membersTemp);
            List<string> members = new List<string>();
            foreach (DataRow dr in membersTemp.Rows)
            {
                members.Add(Convert.ToString(dr[0]));
            }
            con.Close();

            //For each of the members updates the point values
            if (valueSign)
            {
                query = "UPDATE CommunityMember " +
                        "SET points = points + 1 " +
                        "WHERE email = @email";
            }
            else
            {
                query = "UPDATE CommunityMember " +
                        "SET points = points - 1 " +
                        "WHERE email = @email";
            }

            SqlCommand cmd1 = new SqlCommand(query, con);
            cmd1.Parameters.AddWithValue("@email", members[0]);
            con.Open();
            int i;
            i = cmd1.ExecuteNonQuery();
            for (int j = 1; j < members.Count; j++)
            {
                cmd1.Parameters["@email"].Value = members[j];
                i = cmd1.ExecuteNonQuery();
                if (i < 0)
                    return false;
            }
            con.Close();
            return true;


        }

        //I3: Updates the merits of a given community member
        public void updateMerits(string author, int merits)
        {
            string query = "UPDATE CommunityMember " +
                           "SET points = points + @merits " +
                           "WHERE email = @author ";


            //If query is detected as unsafe, won't excecute query
            bool safeQuery = checkQuerySafety(query);
            if (safeQuery)
            {
				cmd = new SqlCommand(query, con);
				cmd.Parameters.AddWithValue("@author", author);
				cmd.Parameters.AddWithValue("@merits", merits);
				con.Open();
				cmd.ExecuteNonQuery();
				con.Close();
            }      
        }

        //I3: Combines all the topics of an Article
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


        //I3: Fetches all articles of a given community member
        public List<ArticleModel> fetchMyArticles(String email)
        {
            List<ArticleModel> articleList = new List<ArticleModel>();


            SqlCommand cmd = new SqlCommand("PISP_getMemeberArticle", con);
			
			bool safeQuery = checkQuerySafety(fetchArticles);
            if (!safeQuery)
            {
                return articleList;
            }
			
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@email", email);
            SqlDataAdapter sd1 = new SqlDataAdapter(cmd);
            DataTable articleTable = new DataTable();
            con.Open();
            sd1.Fill(articleTable);
            foreach (DataRow article in articleTable.Rows)
            {
                articleList.Add(
                    new ArticleModel
                    {
                        articleId = Convert.ToInt32(article["articleId"]),
                        name = Convert.ToString(article["name"]),
                        topicName = Convert.ToString(article["topicName"]),
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
            return articleList;
        }

        //I3: Retrieves mail of coordinator member
        public string getCoordinatorMail()
        {

            ProfileModel memberProfile = new ProfileModel();
            string query = "SELECT email " +
                            "FROM CommunityMember " +
                            "WHERE memberRank = 'coordinator'";


            //If query is detected as unsafe, returns error string
            bool safeQuery = checkQuerySafety(query);
            if (!safeQuery)
            {
                return "ERROR: UNSAFE QUERY DETECTED";
            }

            cmd = new SqlCommand(query, con);
            con.Open();
            reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    memberProfile.email = Convert.ToString(reader["email"]);
                }
            }

            reader.Close();

            con.Close();

            return memberProfile.email;

        }

        //I3: Retrieves information of a given article
        public string getArticleAuthor(int articleId)
        {

            string authors = "";


            ProfileModel memberProfile = new ProfileModel();
            string query = "SELECT CM.name, CM.lastName " +
                            "FROM CommunityMember CM " +
                            "JOIN WRITES W " +
                            "ON W.email = CM.email " +
                            "WHERE W.articleId = @articleId";


            //If query is detected as unsafe, returns empty string
            bool safeQuery = checkQuerySafety(query);
            if (!safeQuery)
            {
                return authors;
            }

            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@articleId", articleId);
            con.Open();
            reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    memberProfile.name = Convert.ToString(reader["name"]);
                    memberProfile.lastName = Convert.ToString(reader["lastName"]);
                    authors += memberProfile.name + " " + memberProfile.lastName + ", ";
                }
            }

            reader.Close();

            con.Close();

            return authors;

        }


        //I3: Retrieves list of all emails of core members
        public List<string> getCoreMemberEmails()
        {
            List<string> emails = new List<string>();

            ProfileModel memberProfile = new ProfileModel();
            string query = "SELECT email " +
                            "FROM CommunityMember " +
                            "WHERE memberRank = 'core'";


            //If query is detected as unsafe, returns empty list
            bool safeQuery = checkQuerySafety(query);
            if (!safeQuery)
            {
                return emails;
            }

            cmd = new SqlCommand(query, con);
            con.Open();
            reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    emails.Add(Convert.ToString(reader["email"]));
                }
            }

            reader.Close();

            con.Close();

            return emails;

        }


        //I3: Checks if a given query contains the 'DROP' statement used maliciously in a query where it shouldn't be
        public bool checkQuerySafety(string query)
        {
            bool safe;

            if (query.Contains("DROP"))
            {
                safe = false;
            }
            else
            {
                safe = true;
            }

            return safe;
        }

        //I3: Adds nomination for the potential coremember to review the article
        public void addNomination(string email, int articleId)
        {
            string query = "INSERT INTO IS_NOMINATED VALUES " +
                           " (DEFAULT,DEFAULT,@email,@articleId)";


            bool safeQuery = checkQuerySafety(query);
            //If query is detected as unsafe, won't excecute it
            if (safeQuery)
            {
                command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@articleId", articleId);
                command.Parameters.AddWithValue("@email", email);

                connection.Open();
                int codeError = command.ExecuteNonQuery();
                connection.Close();
            }            
        }
    }
}
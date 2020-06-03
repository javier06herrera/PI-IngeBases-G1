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
                return memberProfile;
            }
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
    }
}
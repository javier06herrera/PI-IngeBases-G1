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
    }
}
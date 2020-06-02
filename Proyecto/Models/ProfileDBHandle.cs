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
                        memberProfile.addressCity = Convert.ToString(reader["address_city"]);
                        memberProfile.addressCountry = Convert.ToString(reader["address_country"]);
                        memberProfile.hobbies = Convert.ToString(reader["hobbies"]);
                        memberProfile.languages = Convert.ToString(reader["languages"]);
                        memberProfile.email = Convert.ToString(reader["email"]);
                        memberProfile.mobile = Convert.ToString(reader["phoneNumber"]);
                        memberProfile.job = Convert.ToString(reader["workInformation"]);
                        memberProfile.typeOfMember = Convert.ToString(reader["typeOfMember"]);
                        memberProfile.totalQualification = Convert.ToInt32(reader["totalQualification"]);
                    }
                }
                
                reader.Close();
                return memberProfile;
            }
        }
    }
}
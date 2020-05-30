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
        private void Conection()
        {
            string constring = ConfigurationManager.ConnectionStrings["ArticleConn"].ToString();
            con = new SqlConnection(constring);
        }

        public List<ProfileModel> getAttributes()
        {

            List<ProfileModel> memberList = new List<ProfileModel>();

            Conection();
            string memberData = "SELECT * " +
                                "FROM CommunityMember ";

            SqlDataAdapter sd1 = new SqlDataAdapter(memberData, con);
            DataTable memberTable = new DataTable();
            con.Open();
            sd1.Fill(memberTable);
            foreach (DataRow member in memberTable.Rows)
            {
                memberList.Add(
                    new ProfileModel
                    {
                        memberId = Convert.ToInt32(member["memberId"]),
                        name = Convert.ToString(member["name"]),
                        lastName = Convert.ToString(member["lastName"]),
                        birthDate = Convert.ToString(member["birthDate"]),
                        age = Convert.ToInt32(member["age"]),
                        addressCity = Convert.ToString(member["addressCity"]),
                        addressCountry = Convert.ToString(member["addressCountry"]),
                        hobbies = Convert.ToString(member["hobbies"]),
                        languages = Convert.ToString(member["languages"]),
                        email = Convert.ToString(member["email"]),
                        mobile = Convert.ToString(member["mobile"]),
                        job = Convert.ToString(member["job"]),
                        typeOfMember = Convert.ToString(member["typeOfMember"]),
                        totalQualification = Convert.ToInt32(member["totalQualification"])

                    });
            }
            con.Close();

            return memberList;
        }

    }
}
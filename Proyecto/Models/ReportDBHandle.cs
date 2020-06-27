using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Proyecto.Models;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace Proyecto.Models
{
    public class ReportDBHandle
    {
        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["ArticleConn"].ToString();
            con = new SqlConnection(constring);
        }
        public List<ReportModel> GetCountryStats(string query)
        {
            connection();
            List<ReportModel> valuesList = new List<ReportModel>();
            //string getResults = "SELECT C.addressCountry, (Count(*)) As 'Count' " +
            //                    " FROM CommunityMember C " +
            //                    " Group By C.addressCountry ";
            SqlCommand cmd = new SqlCommand(query, con);
            //cmd.Parameters.AddWithValue("@topicName", topic);



            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                valuesList.Add(
                    new ReportModel(Convert.ToString(dr["Value"]), Convert.ToInt32(dr["Count"])));                 
            }
            return valuesList;
        }
    }
}
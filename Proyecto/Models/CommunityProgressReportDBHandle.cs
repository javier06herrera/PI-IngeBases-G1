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
    public class CommunityProgressReportDBHandle
    {
        private SqlConnection dbConnection;
        private SqlCommand command;
        private SqlDataReader reader;

        public CommunityProgressReportDBHandle()
        {
            dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ArticleConn"].ToString());
            
        }

        public List<ReportModel> GetFilteredValues(string query)
        {
            List<ReportModel> list = new List<ReportModel>();
            dbConnection.Open();
            command = new SqlCommand(query, dbConnection);
            reader = command.ExecuteReader();

            if(reader.HasRows)
            {
                DataTable table = new DataTable();
                table.Load(reader);
                foreach (DataRow row in table.Rows)
                {
                    list.Add(new ReportModel(Convert.ToString(row["Member rank"]), Convert.ToInt32(row["Count"])));
                }
            }
            
            dbConnection.Close();
            return list;
        }
    }
}
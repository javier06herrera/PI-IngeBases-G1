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

        public DataTable GetFilteredValues(string query)
        {
            dbConnection.Open();
            command = new SqlCommand(query, dbConnection);
            reader = command.ExecuteReader();
            DataTable table = new DataTable();

            if(reader.HasRows)
            {
                table.Load(reader);
            }
            
            dbConnection.Close();
            return table;
        }
    }
}
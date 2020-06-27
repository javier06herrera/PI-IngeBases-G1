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
    public class DBConnectHanler
    {
        //Connection string is fixed here so when you change its name this is the only place you have to change
        public SqlConnection conn;
        public string connectionString = "ArticleConn";

        public void connect()
        {
            string constring = ConfigurationManager.ConnectionStrings[connectionString].ToString();
            this.conn = new SqlConnection(constring);
        }
        public SqlCommand setSimpleReturnQuery(string query)
        {
            connect();
            SqlCommand cmd = new SqlCommand(query, this.conn);
            return cmd;
        }

    }
}
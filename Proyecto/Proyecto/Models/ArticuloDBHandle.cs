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
    public class ArticuloDBHandle
    {
        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["articuloconn"].ToString();
            con = new SqlConnection(constring);
        }

        // Se agrega un nuevo articulo
        public bool AddArticulo(ArticuloModel smodel)
        {
            connection();
            SqlCommand cmd = new SqlCommand("AddNewArticulo", con); // Nombre procedimiento, 
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@name", smodel.name); // 
            cmd.Parameters.AddWithValue("@type", smodel.type);
            cmd.Parameters.AddWithValue("@Abstract", smodel.Abstract);
            cmd.Parameters.AddWithValue("@publishDate", smodel.publishDate);
            cmd.Parameters.AddWithValue("@content", smodel.content);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();



            //Buscar ID
            connection();
            string findId = "select articleId from Article where name = @name";
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataSet ds = new DataSet();
            con.Open();
            SqlCommand cmd1 = new SqlCommand(findId, con); // Nombre procedimiento, 
            cmd1.Parameters.AddWithValue("@name", smodel.name);
            adapter.SelectCommand = cmd1;
            adapter.Fill(ds);            
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                smodel.articleId = (Convert.ToInt32(dr["articleId"]));
            }
            
            con.Close();

            String[] topics = smodel.topic.Split(',');
            String topic = "";
            for (int m = 0; m < topics.Count(); m++)
            {
                topic = topics[m];
                InsertTopics(topic, smodel.articleId);
            }


            if (i >= 1)
                return true;
            else
                return false;
        }

        //Combines all the topics of an Article
        public string topicMerge( int articleId, DataTable topicList)
        {
            string topicsLine = "";
            foreach(DataRow topic in topicList.Rows)
            {
                if (Convert.ToInt32(topic["articleId"]) == articleId)
                {
                    topicsLine = topicsLine + topic["topic"] + ", ";
                }
            }
            return topicsLine;
        }

        // Ver resultados de busqueda
        public List<ArticuloModel> GetArticulo()
        {
            List<ArticuloModel> articulolist = new List<ArticuloModel>();

            //Fetch of the entire list of articles without topics
            connection();
            string fetchArticles = "SELECT * " +
                                   "FROM Article";
            SqlDataAdapter sd1 = new SqlDataAdapter(fetchArticles,con);
            DataTable articleList = new DataTable();
            con.Open();
            sd1.Fill(articleList);
            foreach (DataRow article in articleList.Rows)
            {
                articulolist.Add(
                    new ArticuloModel
                    {
                        articleId = Convert.ToInt32(article["articleId"]),
                        name = Convert.ToString(article["name"]),
                        topic = " ",
                        Abstract = Convert.ToString(article["Abstract"]),
                        publishDate = Convert.ToString(article["publishDate"]),
                        content = Convert.ToString(article["content"]),
                        type = Convert.ToBoolean(article["type"])
                    });
            }
            con.Close();

            //Fetch of the entire list of topics
            connection();
            string fetchTopics =   "SELECT * " +
                                   "FROM ArticleTopic";
            SqlDataAdapter sd2 = new SqlDataAdapter(fetchTopics, con);
            DataTable topicList = new DataTable();
            con.Open();
            sd2.Fill(topicList);
            foreach(ArticuloModel article in articulolist)
            {
                article.topic = topicMerge(article.articleId, topicList);
            }
            con.Close();
            return articulolist;
        }

        public bool UpdateDetails(ArticuloModel smodel)
        {
            //Update of table articles
            connection();
            String updateArticle = "UPDATE Article " +
                                   "SET name = @name, " +
                                   "type = @type, " +
                                   "abstract = @abstract, " +
                                   "publishDate = @publishDate," +
                                   "content = @content " +
                                   "WHERE articleId = @articleId";

            SqlCommand cmd = new SqlCommand(updateArticle, con);
            cmd.Parameters.AddWithValue("@articleId", smodel.articleId);
            cmd.Parameters.AddWithValue("@name", smodel.name);
            cmd.Parameters.AddWithValue("@type", smodel.type);
            cmd.Parameters.AddWithValue("@abstract", smodel.Abstract);
            cmd.Parameters.AddWithValue("@publishDate", Convert.ToDateTime(smodel.publishDate));
            cmd.Parameters.AddWithValue("@content", smodel.content);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i < 1)
                return false;

            //Topics Elimination
            connection();
            String getTopics = "DELETE FROM ArticleTopic " +
                               "WHERE articleId = @articleId";
            SqlCommand cmd1 = new SqlCommand(getTopics, con);
            cmd1.Parameters.AddWithValue("@articleId", smodel.articleId);
            con.Open();
            i = cmd1.ExecuteNonQuery();
            con.Close();

            //if (i < 1)
            //    return false;

            //Topics Update
            String[] topics = smodel.topic.Split(',');
            String topic = "";
            for (int m = 0; m < topics.Count(); m++)
            {
                topic = topics[m];
                InsertTopics(topic, smodel.articleId);
            }

            if (i >= 1)
                return true;
            else
                return false;
        }

        // Para agregar más de un dato
        public void InsertTopics(String topic, int articleId)
        {
            connection();
            String appendTopic = "INSERT INTO ArticleTopic " +
                                "VALUES(@articleId, @topic)";
            SqlCommand cmd2 = new SqlCommand(appendTopic, con);
            cmd2.Parameters.AddWithValue("@articleId", articleId);
            con.Open();
            cmd2.Parameters.AddWithValue("@topic", topic);
            int i = cmd2.ExecuteNonQuery();
            con.Close();

        }

        public bool DeleteArticulo(int id)
        {
            connection();
            SqlCommand cmd = new SqlCommand("DeleteArticulo", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ArtId", id);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }


        // Se obtienen los topicos de la base de datos
        public List<SelectListItem> PopulateArticles()
        {
            var list = new List<SelectListItem>();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("ObtainTopics", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                //if (reader.Read()) //Revisar perdida de valor
                //{
                while (reader.Read())
                {
                    list.Add(new SelectListItem { Text = reader["Topic"].ToString(), Value = reader["Topic"].ToString() }); // Pone todos los temas que
                    // coincidan con la palabra en la lista
                    // Recuerde que se usa la palabra topic tanto en text como en value por que la solicitud solo devuelve una columna
                }
                //}
                //else
                //{
                //    list.Add(new SelectListItem { Text = "No records found", Value = "0" });
                //}

                con.Close();
            }
            catch (Exception ex)
            {
                list.Add(new SelectListItem { Text = ex.Message.ToString(), Value = "0" });
            }

            return list;
        }

        //
        public List<ArticuloModel> GetResultado(string topico)
        {
            connection();
            List<ArticuloModel> articulolist = new List<ArticuloModel>();

            SqlCommand cmd = new SqlCommand("GetResultados", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Topic", topico);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                articulolist.Add(
                    new ArticuloModel
                    {
                        articleId = Convert.ToInt32(dr["articleId"]),
                        name = Convert.ToString(dr["name"]),
                        topic = Convert.ToString(dr["topic"]),
                        Abstract = Convert.ToString(dr["Abstract"]),
                        publishDate = Convert.ToString(dr["publishDate"]),
                        content = Convert.ToString(dr["content"]),
                        type = Convert.ToBoolean(dr["type"])
                    });
            }
            return articulolist;
        }
    }
}
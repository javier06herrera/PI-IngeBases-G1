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




            // Insertar tópico
            String[] topics = smodel.topic.Split(',');
            String appendTopic = "INSERT INTO ArticleTopic VALUES(@articleId, @topic)";
            SqlCommand cmd2 = new SqlCommand(appendTopic, con);
            //adapter.SelectCommand = cmd2;
            con.Open();
            cmd2.Parameters.AddWithValue("@articleId", smodel.articleId);
            foreach (String topic in topics)
            {
                cmd2.Parameters.AddWithValue("@topic", topic);
                i = cmd2.ExecuteNonQuery();
            }
            con.Close();


            if (i >= 1)
                return true;
            else
                return false;
        }

        // Ver resultados de busqueda
        public List<ArticuloModel> GetArticulo()
        {
            connection();
            List<ArticuloModel> articulolist = new List<ArticuloModel>();

            SqlCommand cmd = new SqlCommand("GetArticulo", con);

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
                        Abstract = Convert.ToString(dr["Abstract"]),
                        publishDate = Convert.ToString(dr["publishDate"]),
                        content = Convert.ToString(dr["content"]),
                        type = Convert.ToBoolean(dr["type"])
                    });
            }



            SqlCommand cmd1 = new SqlCommand("GetArticulo", con);
            return articulolist;
        }

        public bool UpdateDetails(ArticuloModel smodel)
        {
            connection();
            SqlCommand cmd = new SqlCommand("UpdateArticuloDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Id", smodel.articleId);
            cmd.Parameters.AddWithValue("@Name", smodel.name);
            cmd.Parameters.AddWithValue("@Topic", smodel.topic);
            cmd.Parameters.AddWithValue("@Abstract", smodel.Abstract);
            cmd.Parameters.AddWithValue("@PublishDate", Convert.ToDateTime(smodel.publishDate));
            cmd.Parameters.AddWithValue("@content", smodel.content);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
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
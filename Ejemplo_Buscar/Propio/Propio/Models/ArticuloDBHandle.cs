using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Mvc;

namespace Propio.Models
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
            SqlCommand cmd = new SqlCommand("AddNewArticulo", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Name", smodel.Name);
            cmd.Parameters.AddWithValue("@Topic", smodel.Topic);
            cmd.Parameters.AddWithValue("@Abstract", smodel.Abstract);
            cmd.Parameters.AddWithValue("@PublishDate", smodel.PublishDate);
            cmd.Parameters.AddWithValue("@Route", smodel.Route);

            con.Open();
            int i = cmd.ExecuteNonQuery();
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
                        Name = Convert.ToString(dr["Name"]),
                        Topic = Convert.ToString(dr["Topic"]),
                        Abstract = Convert.ToString(dr["Abstract"]),
                        PublishDate = Convert.ToString(dr["PublishDate"]),
                        Route = Convert.ToString(dr["Route"])
                    });
            }
            return articulolist;
        }

        public bool UpdateDetails(ArticuloModel smodel)
        {
            connection();
            SqlCommand cmd = new SqlCommand("UpdateArticuloDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ArtId", smodel.Id);
            cmd.Parameters.AddWithValue("@Name", smodel.Name);
            cmd.Parameters.AddWithValue("@Topic", smodel.Topic);
            cmd.Parameters.AddWithValue("@Abstract", smodel.Abstract);
            cmd.Parameters.AddWithValue("@PublishDate", smodel.PublishDate);
            cmd.Parameters.AddWithValue("@Route", smodel.Route);

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

            cmd.Parameters.AddWithValue("@StdId", id);

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

                if (reader.Read())
                {
                    while (reader.Read())
                    { 
                        list.Add(new SelectListItem { Text = reader["Topic"].ToString(), Value = reader["Topic"].ToString() }); // Pone todos los temas que
                        // coincidan con la palabra en la lista
                        // Recuerde que se usa la palabra topic tanto en text como en value por que la solicitud solo devuelve una columna
                    }
                }
                else
                {
                    list.Add(new SelectListItem { Text = "No records found", Value = "0" });
                }

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
                        Name = Convert.ToString(dr["Name"]),
                        Topic = Convert.ToString(dr["Topic"]),
                        Abstract = Convert.ToString(dr["Abstract"]),
                        PublishDate = Convert.ToString(dr["PublishDate"]),
                        Route = Convert.ToString(dr["Route"])
                    });
            }
            return articulolist;
        }
    }
}
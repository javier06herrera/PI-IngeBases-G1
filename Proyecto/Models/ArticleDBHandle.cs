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
    public class ArticleDBHandle
    {
        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["ArticleConn"].ToString();
            con = new SqlConnection(constring);
        }

        // Se agrega un nuevo articulo
        public bool AddArticle(ArticleModel smodel, bool type)
        {
            connection();
            string AddNewArticle = "INSERT INTO Article " +
                                   "VALUES (@name, @type,@Abstract,@publishDate,@content)";
            SqlCommand cmd = new SqlCommand(AddNewArticle, con); // Nombre procedimiento, 

            cmd.Parameters.AddWithValue("@name", smodel.name);
            cmd.Parameters.AddWithValue("@type", type);
            cmd.Parameters.AddWithValue("@Abstract", smodel.Abstract);
            cmd.Parameters.AddWithValue("@publishDate", Convert.ToDateTime(smodel.publishDate));
            cmd.Parameters.AddWithValue("@content", smodel.content);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();



            //Buscar ID
            connection();
            string findId = "SELECT articleId " +
                            "FROM Article " +
                            "WHERE name = @name";
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
        public string topicMerge(int articleId, DataTable topicList)
        {
            string topicsLine = "";
            foreach (DataRow topic in topicList.Rows)
            {
                if (Convert.ToInt32(topic["articleId"]) == articleId)
                {
                    topicsLine = topicsLine + topic["topic"] + ", ";
                }
            }
            return topicsLine;
        }

        // Ver resultados de busqueda
        public List<ArticleModel> GetArticle()
        {
            List<ArticleModel> articleList = new List<ArticleModel>();

            //Fetch of the entire list of articles without topics
            connection();
            string fetchArticles = "SELECT * " +
                                   "FROM Article "+
                                   "ORDER BY publishDate DESC";
            SqlDataAdapter sd1 = new SqlDataAdapter(fetchArticles, con);
            DataTable articleTable = new DataTable();
            con.Open();
            sd1.Fill(articleTable);
            foreach (DataRow article in articleTable.Rows)
            {
                articleList.Add(
                    new ArticleModel
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
            string fetchTopics = "SELECT * " +
                                 "FROM ArticleTopic";
            SqlDataAdapter sd2 = new SqlDataAdapter(fetchTopics, con);
            DataTable topicList = new DataTable();
            con.Open();
            sd2.Fill(topicList);
            foreach (ArticleModel article in articleList)
            {
                article.topic = topicMerge(article.articleId, topicList);
            }
            con.Close();
            return articleList;
        }

        public bool UpdateDetails(ArticleModel smodel)
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
            cmd2.Parameters.AddWithValue("@topic", topic);
            con.Open();
            int i = cmd2.ExecuteNonQuery();
            con.Close();

        }

        public bool DeleteArticle(int id)
        {
            connection();
            string deleteArticle = "DELETE FROM Article" +
                                   "WHERE articleId = @aticleId";
            SqlCommand cmd = new SqlCommand(deleteArticle, con);
            cmd.Parameters.AddWithValue("@aticleId", id);

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
                string obtainTopics = "SELECT DISTINCT topic " +
                                      "FROM ArticleTopic";
                SqlCommand cmd = new SqlCommand(obtainTopics, con);
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
        public List<ArticleModel> GetResults(string topic)
        {
            connection();
            List<ArticleModel> articulolist = new List<ArticleModel>();
            string getResults = "SELECT A.articleId, A.name, A.abstract, A.publishDate, ATo.topic, A.content, A.type " +
                                "FROM Article A " +
                                "JOIN ArticleTopic ATo ON A.articleId = ATo.articleId " +
                                "WHERE ATo.topic = @topic";
            SqlCommand cmd = new SqlCommand(getResults, con);
            cmd.Parameters.AddWithValue("@topic", topic);



            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                articulolist.Add(
                    new ArticleModel
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

        public List<QuestionModel> GetQuestion(bool moderator)
        {
            List<QuestionModel> faqList = new List<QuestionModel>();

            //Fetch of the entire list of articles without topics
            connection();
            string questions = "";
            if (!moderator)
            {
                questions = "SELECT * " +
                            "FROM Faq " +
                            "WHERE status = 'true'";
            }
            else
            {
                questions = "SELECT * " +
                            "FROM Faq";
            }
            SqlDataAdapter sd1 = new SqlDataAdapter(questions, con);
            DataTable faqsList = new DataTable();
            con.Open();
            sd1.Fill(faqsList);
            foreach (DataRow faq in faqsList.Rows)
            {
                faqList.Add(
                    new QuestionModel
                    {
                        questionId = Convert.ToInt32(faq["questionId"]),
                        question = Convert.ToString(faq["question"]),
                        answer = Convert.ToString(faq["answer"]),
                        status = Convert.ToBoolean(faq["status"]),
                    });
            }
            con.Close();

            return faqList;
        }

        public bool AddQuestion(QuestionModel smodel, bool type)
        {
            connection();
            string sendQuestion = "INSERT INTO Faq " +
                                  "VALUES (@question, @status, @answer)";
            SqlCommand cmd = new SqlCommand(sendQuestion, con);
            cmd.Parameters.AddWithValue("@question", smodel.question);
            cmd.Parameters.AddWithValue("@status", type);
            cmd.Parameters.AddWithValue("@answer", "");
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
                return true;
            else
                return false;
        }
        public bool UpdateQuestion(QuestionModel smodel)
        {
            //Update of table articles
            connection();
            String updateQuestion = "UPDATE Faq " +
                                   "SET question = @question, " +
                                   "answer = @answer, " +
                                   "status = @status " +
                                   "WHERE questionId = @questionId";

            SqlCommand cmd = new SqlCommand(updateQuestion, con);
            cmd.Parameters.AddWithValue("@question", smodel.question);
            cmd.Parameters.AddWithValue("@answer", smodel.answer);
            cmd.Parameters.AddWithValue("@status", smodel.status);
            cmd.Parameters.AddWithValue("@questionId", smodel.questionId);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

    }
}
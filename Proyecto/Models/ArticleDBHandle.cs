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
        public bool AddArticle(ArticleModel smodel, string type)
        {
            connection();
            string AddNewArticle = "INSERT INTO Article " +
                                   "VALUES (@name, @type,@Abstract,@publishDate,@content,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT)";
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

            String[] topics = smodel.topicName.Split(',');
            String[] subjects;
  
            for (int m = 0; m < topics.Count(); m++)
            {
                subjects = topics[m].Split(':');
                InsertTopics(smodel.articleId, subjects[0],subjects[1]);
            }


            //Insert a relation with member
            connection();
            AddNewArticle = "INSERT INTO WRITES " +
                            "VALUES ( 1 , @articleId)";
            SqlCommand cmd2 = new SqlCommand(AddNewArticle, con); // Nombre procedimiento, 
            cmd2.Parameters.AddWithValue("@articleId", smodel.articleId);

            con.Open();
            i = cmd2.ExecuteNonQuery();
            con.Close();

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
                    topicsLine = topicsLine + topic["category"] + ":" + topic["topicName"] + ", ";
                }
            }
            topicsLine.Remove(topicsLine.Length - 2, 1);
            return topicsLine;
        }


        internal void UpdateAccess(ArticleModel smodel)
        {
            connection();
            String updateCounts = "UPDATE Article " +
                                  "SET accessCount = accessCount+1 " +
                                  "WHERE articleId = @articleId";

            SqlCommand cmd = new SqlCommand(updateCounts, con);
            cmd.Parameters.AddWithValue("@articleId", smodel.articleId);
            //cmd.Parameters.AddWithValue("@accessCount", smodel.accessCount);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();            
        }



        // Ver resultados de busqueda
        public List<ArticleModel> GetArticle()
        {
            List<ArticleModel> articleList = new List<ArticleModel>();
      
            connection();
            SqlCommand cmd = new SqlCommand("PISP_getArticles", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd1 = new SqlDataAdapter(cmd);
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
                        topicName = Convert.ToString(article["topicName"]) ,
                        Abstract = Convert.ToString(article["Abstract"]),
                        publishDate = Convert.ToString(article["publishDate"]),
                        content = Convert.ToString(article["content"]),
                        type = Convert.ToString(article["type"]),
                        baseGrade = Convert.ToInt32(article["baseGrade"]),
                        accessCount = Convert.ToInt32(article["accessCount"]),
                        likesCount = Convert.ToInt32(article["likesCount"]),
                        neutralCount = Convert.ToInt32(article["neutralCount"]),
                        dislikesCount = Convert.ToInt32(article["dislikesCount"]),
                        likeBalance = Convert.ToInt32(article["likeBalance"]),
                        checkedStatus = Convert.ToString(article["checkedStatus"])
                    });
            }
            con.Close();
            return articleList;
        }

        public bool UpdateDetails(ArticleModel smodel, string type)
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
            cmd.Parameters.AddWithValue("@type", type);
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
            String getTopics = "DELETE FROM INVOLVES " +
                               "WHERE articleId = @articleId";
            SqlCommand cmd1 = new SqlCommand(getTopics, con);
            cmd1.Parameters.AddWithValue("@articleId", smodel.articleId);
            con.Open();
            i = cmd1.ExecuteNonQuery();
            con.Close();

            //if (i < 1)
            //    return false;

            //Topics Update
            String[] topics = smodel.topicName.Split(',');
            String[] subjects;

            for (int m = 0; m < topics.Count(); m++)
            {
                subjects = topics[m].Split(':');
                InsertTopics(smodel.articleId, subjects[0], subjects[1]);
            }

            if (i >= 1)
                return true;
            else
                return false;
        }

        // Para agregar más de un dato
        public void InsertTopics(int articleId, String category, String topicName)
        {
            connection();
            String appendTopic = "INSERT INTO INVOLVES " +
                                 "VALUES(@articleId, @category ,@topicName)";
            SqlCommand cmd2 = new SqlCommand(appendTopic, con);
            cmd2.Parameters.AddWithValue("@articleId", articleId);
            cmd2.Parameters.AddWithValue("@topicName", topicName);
            cmd2.Parameters.AddWithValue("@category", category);
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
                string obtainTopics = "SELECT DISTINCT topicName " +
                                      "FROM INVOLVES";
                SqlCommand cmd = new SqlCommand(obtainTopics, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                //if (reader.Read()) //Revisar perdida de valor
                //{
                while (reader.Read())
                {
                    list.Add(new SelectListItem { Text = reader["topicName"].ToString(), Value = reader["topicName"].ToString() }); // Pone todos los temas que
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
            string getResults = "SELECT A.*, I.topicName" +
                                " FROM Article A " +
                                " JOIN INVOLVES I ON A.articleId = I.articleId " +
                                " WHERE I.topicName = @topicName " +
                                " AND A.checkedStatus = 'published'";
            SqlCommand cmd = new SqlCommand(getResults, con);
            cmd.Parameters.AddWithValue("@topicName", topic);

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
                        topicName = Convert.ToString(dr["topicName"]),
                        Abstract = Convert.ToString(dr["Abstract"]),
                        publishDate = Convert.ToString(dr["publishDate"]),
                        content = Convert.ToString(dr["content"]),
                        type = Convert.ToString(dr["type"]),
                        baseGrade = Convert.ToInt32(dr["baseGrade"]),
                        accessCount = Convert.ToInt32(dr["accessCount"]),
                        likesCount = Convert.ToInt32(dr["likesCount"]),
                        neutralCount = Convert.ToInt32(dr["neutralCount"]),
                        dislikesCount = Convert.ToInt32(dr["dislikesCount"]),
                        likeBalance = Convert.ToInt32(dr["likeBalance"])
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
                            "FROM Question " +
                            "WHERE faq = 'posted' ";
            }
            else
            {
                questions = "SELECT * " +
                            "FROM Question";
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
                        status = Convert.ToString(faq["status"]),
                        faq = Convert.ToString(faq["faq"])
                    });
            }
            con.Close();

            return faqList;
        }

        public bool AddQuestion(QuestionModel smodel, bool type)
        {
            connection();
            string sendQuestion = "INSERT INTO Question (question, faq, answer, status) " +
                                  "VALUES (@question, @faq, @answer, @status)";
            SqlCommand cmd = new SqlCommand(sendQuestion, con);
            cmd.Parameters.AddWithValue("@question", smodel.question);
            cmd.Parameters.AddWithValue("@faq", "not posted");
            cmd.Parameters.AddWithValue("@answer", "no answer");
            cmd.Parameters.AddWithValue("@status", "not checked");
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
            String updateQuestion = "UPDATE Question " +
                                   "SET question = @question, " +
                                   "faq = @faq, " +
                                   "answer = @answer, " +
                                   "status = @status " +
                                   "WHERE questionId = @questionId";

            SqlCommand cmd = new SqlCommand(updateQuestion, con);
            cmd.Parameters.AddWithValue("@question", smodel.question);
            cmd.Parameters.AddWithValue("@answer", smodel.answer);
            cmd.Parameters.AddWithValue("@faq", smodel.faq);
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


        public int[] updateLikes(int articleId, int voteFlag)
        {
            //Update of like or dislike count
            connection();
            String query;
            if (voteFlag == 0)
            {
                query = "UPDATE Article " +
               "SET likesCount = likesCount + 1 " +
               "WHERE articleId = @articleId";
            }
            else if (voteFlag==1)
            {
                query = "UPDATE Article " +
               "SET neutralCount = neutralCount + 1" +
               "WHERE articleId = @articleId";
            }
            else
            {
                query = "UPDATE Article " +
                "SET dislikesCount = dislikesCount + 1 " +
                "WHERE articleId = @articleId";
            }


            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@articleId", articleId);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();


            ////Update of like balance
            //connection();
            //query = "UPDATE Article " +
            //        "SET likeBalance = likesCount - dislikesCount " +
            //        "WHERE articleId = @articleId";


            //SqlCommand cmd1 = new SqlCommand(query, con);
            //cmd1.Parameters.AddWithValue("@articleId", articleId);
            //con.Open();
            //i = cmd1.ExecuteNonQuery();
            //con.Close();

            //Bring likes, neutral and dislikes
            int[] likeData = new int[3];
            query = "SELECT likesCount " +
                    "FROM Article " +
                    "WHERE articleId = @articleId";
            SqlCommand cmd1 = new SqlCommand(query, con);
            cmd1.Parameters.AddWithValue("@articleId", articleId);
            cmd1.CommandText = query;
            //cmd1.Parameters["@articleId"].Value = articleId;
            con.Open();
            likeData[0] = Convert.ToInt32(cmd1.ExecuteScalar());
            con.Close();

            query = "SELECT neutralCount " +
                    "FROM Article " +
                    "WHERE articleId = @articleId";
            cmd1.CommandText = query;
            cmd1.Parameters["@articleId"].Value = articleId;
            con.Open();
            likeData[1] = Convert.ToInt32(cmd1.ExecuteScalar());
            con.Close();

            query = "SELECT dislikesCount " +
                    "FROM Article " +
                    "WHERE articleId = @articleId";
            cmd1.CommandText = query;
            cmd1.Parameters["@articleId"].Value = articleId;
            con.Open();
            likeData[2] = Convert.ToInt32(cmd1.ExecuteScalar());
            con.Close();

            return likeData;
        }

        //Iteración 3
        public ArticleModel getOneArticle(int articleId)
        {
            connection();

            ArticleModel article = new ArticleModel();
            string query = "SELECT * " +
                            "FROM Article A " +
                            "WHERE A.articleId = @articleId";

            SqlCommand command = new SqlCommand(query, con); // Nombre procedimiento, 
            command.Parameters.AddWithValue("@articleId", articleId);
            SqlDataReader rd;

          
            con.Open();
            rd = command.ExecuteReader();
            if (rd.HasRows)
            {
                while (rd.Read())
                {
                    article.articleId = Convert.ToInt32(rd["articleId"]);
                    article.name = Convert.ToString(rd["name"]);
                    article.type = Convert.ToString(rd["type"]);
                    article.Abstract = Convert.ToString(rd["Abstract"]);
                    article.publishDate = Convert.ToString(rd["publishDate"]);
                    article.content = Convert.ToString(rd["content"]);
                    article.baseGrade = Convert.ToInt32(rd["baseGrade"]);
                    article.accessCount = Convert.ToInt32(rd["accessCount"]);
                    article.likesCount = Convert.ToInt32(rd["likesCount"]);
                    article.neutralCount = Convert.ToInt32(rd["neutralCount"]);
                    article.dislikesCount = Convert.ToInt32(rd["dislikesCount"]);
                    article.likeBalance = Convert.ToInt32(rd["likeBalance"]);
                }
            }

            rd.Close();
            con.Close();

            return article;
        }

        //I3: Updates an article status to published after being given a veredict by coordinator member
        public void updateArticleStatus(int articleId, string veredict)
        {
            connection();
            String query = "UPDATE Article " +
                            "SET checkedStatus = '@veredict' " +
                            "WHERE articleId = @articleId ";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@articleId", articleId);
            cmd.Parameters.AddWithValue("@veredict", veredict);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
        }


        //Iteación 3
        public bool updateArticleState(ArticleModel model)
        {
            connection();
            String updateQuestion = "UPDATE Article " +
                                   "SET checkedStatus = 'pending assignation' " +                                  
                                   "WHERE articleId = @articleId";

            SqlCommand cmd = new SqlCommand(updateQuestion, con);
            cmd.Parameters.AddWithValue("@articleId", model.articleId);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        public List<string> getAuthors(int articleId)
        {
            List<string> authors = new List<string>();

            connection();

            string query = "SELECT * " +
                           "FROM WRITES " +
                           "WHERE articleId = @articleId";

            SqlCommand command = new SqlCommand(query, con);
            command.Parameters.AddWithValue("@articleId", articleId);
            SqlDataReader reader;

            con.Open();
            reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    authors.Add( Convert.ToString(reader["email"]));
                }
            }
            return authors;
        }

        //I3: Updates an article base grade after grade is calculated according to reviews ponderation
        public void updateBaseGrade(int articleId, int baseGrade)
        {
            connection();
            String query = "UPDATE Article " +
                           "SET baseGrade = @baseGrade " +
                           "WHERE articleId = @articleId ";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@articleId", articleId);
            cmd.Parameters.AddWithValue("@baseGrade", baseGrade);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}
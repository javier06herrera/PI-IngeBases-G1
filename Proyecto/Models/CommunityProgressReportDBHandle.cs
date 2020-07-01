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

            if (reader.HasRows)
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

        //This method creates a tabla that contain all the topics and categories by memberRanks
        public List<List<string>> getRankTopicCategory(string query)
        {
            //This section deals with the DB
            DataTable dbInfo = new DataTable();
            command = new SqlCommand(query, dbConnection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            dbConnection.Open();
            adapter.Fill(dbInfo);

            //This section creates and fills the table in two stages
            List<string> memberRanks = getMemberRanks(dbInfo);
            List<List<string>> processedInfo = fillRankTopics(dbInfo, memberRanks);
            fillRankCategory(dbInfo, memberRanks, ref processedInfo);

            //Cleaning up
            dbConnection.Close();
            return processedInfo;
        }

        //This method finds out the different types of rank  
        public List<string> getMemberRanks(DataTable dbInfo)
        {
            List<string> memberRanks = new List<string>();
            string temp = "";
            foreach (DataRow item in dbInfo.Rows)
            {
                temp = Convert.ToString(item["memberRank"]);
                if (memberRanks.Contains(temp) == false)
                {
                    memberRanks.Add(temp);
                }
            }

            return memberRanks;
        }

        //This method creates the firts part of the table, the topics by member rank.
        public List<List<string>> fillRankTopics(DataTable dbInfo, List<string> memberRanks)
        {
            //Temp string for operations
            string tempString = "";
            //Temp int for operations
            int tempInt = 0;
            //This int keeps the column of a specific topic
            int topicIndex = 0;
            //This int keeps the row of a specific rank
            int rankIndex = 0;
            //This list holds the table
            List<List<string>> result = new List<List<string>>();

            //This is the list which holds the title row
            result.Add(new List<string>());
            result[0].Add("Ranks");
            //This for adds a new row to the table for every rank in the consult, and also adds the first column of the table
            for (int i = 0; i < memberRanks.Count(); i++)
            {
                result.Add(new List<string>());
                result[i+1].Add(memberRanks[i]);
            }

            //Now we start adding and classifign the results into the table
            foreach (DataRow item in dbInfo.Rows)
            {


                //This section adds 
                tempString = Convert.ToString(item["topicName"]);
                topicIndex = result[0].IndexOf(tempString);
                //If the topic havent been added, a column is created to hold it
                if (topicIndex < 0)
                {
                    //Title row updated
                    result[0].Add(tempString);
                    //Creation of the rest of the column 
                    for (int i = 1; i < result.Count(); i++)
                    {
                        result[i].Add("0");
                    }
                    topicIndex = result[0].IndexOf(tempString);
                }

                //This section classifies 
                tempString = Convert.ToString(item["memberRank"]);
                //We need to add one unit because of the title row
                rankIndex = memberRanks.IndexOf(tempString) + 1;
                //We add one unit to the string in the table
                tempInt = Convert.ToInt32(result[rankIndex][topicIndex]) + 1;
                result[rankIndex][topicIndex] = Convert.ToString(tempInt);


            }
            return result;
        }

        //This method creates the second part of the table, the categories by member rank.
        public List<List<string>> fillRankCategory(DataTable dbInfo, List<string> memberRanks, ref List<List<string>> processedInfo)
        {
            //Temp string for operations
            string tempString = "";
            //Temp int for operations
            int tempInt = 0;
            //This int keeps the column of a specific topic
            int categoryIndex = 0;
            //This int keeps the row of a specific rank
            int rankIndex = 0;

            //Creation of a division
            divisorColumn(ref processedInfo);

            //Now we start adding and classifign the results into the table
            foreach (DataRow item in dbInfo.Rows)
            {


                //This section adds categories
                tempString = Convert.ToString(item["category"]);
                categoryIndex = processedInfo[0].IndexOf(tempString);
                //If the topic havent been added, a column is created to hold it
                if (categoryIndex < 0)
                {
                    //Title row updated
                    processedInfo[0].Add(tempString);
                    //Creation of the rest of the column 
                    for (int i = 1; i < processedInfo.Count(); i++)
                    {
                        processedInfo[i].Add("0");
                    }
                    categoryIndex = processedInfo[0].IndexOf(tempString);
                }

                //This section classifies by rank
                tempString = Convert.ToString(item["memberRank"]);
                //We need to add one unit because of the title row
                rankIndex = memberRanks.IndexOf(tempString) + 1;
                //We add one unit to the string in the table
                tempInt = Convert.ToInt32(processedInfo[rankIndex][categoryIndex]) + 1;
                processedInfo[rankIndex][categoryIndex] = Convert.ToString(tempInt);


            }
            return processedInfo;
        }

        public void divisorColumn(ref List<List<string>> processedInfo)
        {
            int rows = processedInfo.Count();

            //Creattion of a new column in the table
            for (int i = 0; i < rows; i++)
            {
                processedInfo[i].Add("#");
            }
        }

        //---------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------

        //This method creates a tabla that contain all the topics and categories by memberRanks
        public List<List<string>> getViewsTopicCategory(string query)
        {
            //This section deals with the DB
            DataTable dbInfo = new DataTable();
            command = new SqlCommand(query, dbConnection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            dbConnection.Open();
            adapter.Fill(dbInfo);

            //This section creates and fills the table in two stages
            List<string> memberRanks = getMemberRanks(dbInfo);
            List<List<string>> processedInfo = fillTopicViews(dbInfo, memberRanks);
            fillCategoryViews(dbInfo, memberRanks, ref processedInfo);

            //Cleaning up
            dbConnection.Close();
            return processedInfo;
        }


        //This method creates the firts part of the table, the topics by member rank.
        public List<List<string>> fillTopicViews(DataTable dbInfo, List<string> memberRanks)
        {
            //Temp string for operations
            string tempString = "";
            //Temp int for operations
            int tempInt = 0;
            //This int keeps the column of a specific topic
            int topicIndex = 0;
            //This int keeps the row of a specific rank
            int rankIndex = 0;
            //This list holds the table
            List<List<string>> result = new List<List<string>>();

            //This is the list which holds the title row
            result.Add(new List<string>());
            result[0].Add("Ranks");
            //This for adds a new row to the table for every rank in the consult, and also adds the first column of the table
            for (int i = 0; i < memberRanks.Count(); i++)
            {
                result.Add(new List<string>());
                result[i+1].Add(memberRanks[i]);
            }

            //Now we start adding and classifign the results into the table
            foreach (DataRow item in dbInfo.Rows)
            {


                //This section adds 
                tempString = Convert.ToString(item["topicName"]);
                topicIndex = result[0].IndexOf(tempString);
                //If the topic havent been added, a column is created to hold it
                if (topicIndex < 0)
                {
                    //Title row updated
                    result[0].Add(tempString);
                    //Creation of the rest of the column 
                    for (int i = 1; i < result.Count(); i++)
                    {
                        result[i].Add("0");
                    }
                    topicIndex = result[0].IndexOf(tempString);
                }

                //This section classifies 
                tempString = Convert.ToString(item["memberRank"]);
                //We need to add one unit because of the title row
                rankIndex = memberRanks.IndexOf(tempString) + 1;
                //We add one unit to the string in the table
                tempInt = Convert.ToInt32(result[rankIndex][topicIndex]) + Convert.ToInt32(item["accessCount"]);
                result[rankIndex][topicIndex] = Convert.ToString(tempInt);


            }
            return result;
        }

        //This method creates the second part of the table, the categories by member rank.
        public List<List<string>> fillCategoryViews(DataTable dbInfo, List<string> memberRanks, ref List<List<string>> processedInfo)
        {
            //Temp string for operations
            string tempString = "";
            //Temp int for operations
            int tempInt = 0;
            //This int keeps the column of a specific topic
            int categoryIndex = 0;
            //This int keeps the row of a specific rank
            int rankIndex = 0;

            //Creation of a division
            divisorColumn(ref processedInfo);

            //Now we start adding and classifign the results into the table
            foreach (DataRow item in dbInfo.Rows)
            {


                //This section adds categories
                tempString = Convert.ToString(item["category"]);
                categoryIndex = processedInfo[0].IndexOf(tempString);
                //If the topic havent been added, a column is created to hold it
                if (categoryIndex < 0)
                {
                    //Title row updated
                    processedInfo[0].Add(tempString);
                    //Creation of the rest of the column 
                    for (int i = 1; i < processedInfo.Count(); i++)
                    {
                        processedInfo[i].Add("0");
                    }
                    categoryIndex = processedInfo[0].IndexOf(tempString);
                }

                //This section classifies by rank
                tempString = Convert.ToString(item["memberRank"]);
                //We need to add one unit because of the title row
                rankIndex = memberRanks.IndexOf(tempString) + 1;
                //We add one unit to the string in the table
                tempInt = Convert.ToInt32(processedInfo[rankIndex][categoryIndex]) + Convert.ToInt32(item["accessCount"]);
                processedInfo[rankIndex][categoryIndex] = Convert.ToString(tempInt);


            }
            return processedInfo;
        }
    }
}
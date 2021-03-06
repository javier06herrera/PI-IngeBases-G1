﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proyecto.Models;
using Newtonsoft.Json;
using System.Data;

namespace Proyecto.Controllers
{
    public class CommunityProgressReportController : Controller
    {
        public const int MAX_NUMBER_OF_MEMBERS = 6;

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(CommunityProgressReportModel model)
        {
            return View();
        }

        [HttpPost]
        public string GetFilteredValues(string filter)
        {
            CommunityProgressReportDBHandle dataBaseHandler = new CommunityProgressReportDBHandle();

            string query = this.GetQuery(filter);
            List<ReportModel> filteredValues = dataBaseHandler.GetFilteredValues(query);
            return JsonConvert.SerializeObject(filteredValues);
        }

        [HttpPost]
        public string GetFilteredTable(string filter)
        {
            CommunityProgressReportDBHandle dataBaseHandler = new CommunityProgressReportDBHandle();

            string query = this.GetQuery(filter);
            List<List<string>> listOfString;
            if (filter == "Number of articles peer category and topic")
            {
                listOfString = dataBaseHandler.getRankTopicCategory(query);
            }
            else
            {
                listOfString = dataBaseHandler.getViewsTopicCategory(query);
            }
            return JsonConvert.SerializeObject(listOfString);
        }

        public string GetQuery(string filter)
        {
            string query = null;

            switch (filter)
            {
                case "Number of articles":
                    query = "SELECT	CM.memberRank AS [Member rank], COUNT(W.articleId) AS [Count]\n" +
                            "FROM CommunityMember CM\n" +
                            "JOIN WRITES W\n" +
                            "ON CM.email = W.email\n" +
                            "GROUP BY CM.memberRank";
                    break;
                case "Article score":
                    query = "SELECT CM.memberRank AS [Member rank], SUM(A.baseGrade) AS [Count]\n" +
                            "FROM CommunityMember CM\n" +
                            "JOIN WRITES W\n" +
                            "ON CM.email = W.email\n" +
                            "JOIN Article A\n" +
                            "ON W.articleId = A.articleId\n" +
                            "WHERE A.checkedStatus = 'published' \n" +
                            "GROUP BY CM.memberRank";
                    break;
                case "Number of articles peer category and topic":
                    query = "SELECT DISTINCT A.articleId, I.topicName, I.category, C.memberRank " +
                            "FROM Article A " +
                            "JOIN INVOLVES I on I.articleId = A.articleId " +
                            "JOIN WRITES W on A.articleId = W.articleId " +
                            "JOIN CommunityMember C on C.email = W.email " +
                            "WHERE A.checkedStatus = 'published' " +
                            "ORDER BY C.memberRank, I.topicName";
                    break;

                case "Access Count peer category and topic":
                    query = "SELECT DISTINCT A.articleId, C.memberRank, I.topicName, I.category, A.accessCount " +
                            "FROM Article A " +
                            "JOIN INVOLVES I on I.articleId = A.articleId " +
                            "JOIN WRITES W on A.articleId = W.articleId " +
                            "JOIN CommunityMember C on C.email = W.email " +
                            "WHERE A.checkedStatus = 'published' " +
                            "ORDER BY C.memberRank, I.topicName";
                    break;
            }
            return query;
        }

       
    }

}

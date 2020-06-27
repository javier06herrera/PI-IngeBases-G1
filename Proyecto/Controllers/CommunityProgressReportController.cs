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
        public string GetFilteredValues(string[] selectedMemberRanks, string filter)
        {
            CommunityProgressReportDBHandle dataBaseHandler = new CommunityProgressReportDBHandle();

            string query = this.GetQuery(selectedMemberRanks,filter);
            List<ReportModel> filteredValues = dataBaseHandler.GetFilteredValues(query);
            return JsonConvert.SerializeObject(filteredValues);
        }

        public string GetQuery(string [] selectedMemberRanks, string filter)
        {
            string query = null;
            string memberRanks = GetMembersCondition(selectedMemberRanks);

            switch (filter)
            {
                case "Number of articles":
                    query = "SELECT	CM.memberRank AS [Member rank], COUNT(W.articleId) AS [Count]\n" +
                            "FROM CommunityMember CM\n" +
                            "JOIN WRITES W\n" +
                            "ON CM.email = W.email\n" +
                             memberRanks + "\n" +
                            "GROUP BY CM.memberRank";
                    break;
                case "Article score":
                    query = "SELECT CM.memberRank AS [Member rank], AVG(A.baseGrade + likeBalance + accessCount) AS [Count]\n" +
                            "FROM CommunityMember CM\n" +
                            "JOIN WRITES W\n" +
                            "ON CM.email = W.email\n" +
                            "JOIN Article A\n" +
                            "ON  W.articleId = A.articleId\n" +
                            memberRanks + "\n" +
                            "AND A.checkedStatus = 'published'\n" +
                            "GROUP BY CM.memberRank ";
                    break;
                case "Number of articles peer category and topic":
                    query = " ";
                    break;

                case "Access count peer category and topic":
                    query = "";
                    break;
            }
            return query;
        }

        public string GetMembersCondition(string [] selectedMemberRanks)
        {
            string membersCondition = null;
            if (selectedMemberRanks.Length < MAX_NUMBER_OF_MEMBERS)
            {
                membersCondition = "WHERE (";
                for (int memberRank = 0; memberRank < selectedMemberRanks.Length; ++memberRank)
                {
                    membersCondition += "CM.memberRank = " + "'" + selectedMemberRanks[memberRank].ToLower() + "'";
                    if (memberRank + 1 < selectedMemberRanks.Length)
                    {
                        membersCondition += " OR ";
                    }
                }
                membersCondition += ")";
            }
            else
            {
                membersCondition = string.Empty;
            }
            return membersCondition;
        }
    }

}

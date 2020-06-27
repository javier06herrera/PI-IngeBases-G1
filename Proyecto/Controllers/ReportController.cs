using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proyecto.Models;
using Newtonsoft.Json;

namespace Proyecto.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult ReportUsers()
        {
            return View();
        }


        [HttpPost]
        public string ReportUsers(string category)
        {
            ReportDBHandle sdb = new ReportDBHandle();
            List<ReportModel> dataPoints = new List<ReportModel>();
            string query;
            if (category == "Country")
            {
                query = "SELECT C.addressCountry As 'Value', (Count(*)) As 'Count' " +
                                " FROM CommunityMember C " +
                                " Group By C.addressCountry ";
                dataPoints = sdb.GetCountryStats(query);
            }

            else
            {
                if (category == "Skills")
                {
                    query = "SELECT H.skillName As 'Value', (Count(*)) As 'Count' " +
                                    " FROM CommunityMember C, HAS_SKILL H, Skill S " +
                                    " WHERE C.email = H.email  " +
                                    " AND H.category = S.subjectCategory " +
                                    " AND H.skillName = subjectSkillName " +
                                    " Group By H.skillName ";
                    dataPoints = sdb.GetCountryStats(query);
                }


                else
                {
                    if (category == "Languages")
                    {
                        query = "SELECT C.languages As 'Value', (Count(*)) As 'Count' " +
                                        " FROM CommunityMember C " +
                                        " Group By C.languages ";
                        dataPoints = sdb.GetCountryStats(query);
                    }

                    else
                    {
                        if (category == "Hobbies")
                        {
                            query = "SELECT C.hobbies As 'Value', (Count(*)) As 'Count' " +
                                            " FROM CommunityMember C " +
                                            " Group By C.hobbies ";
                            dataPoints = sdb.GetCountryStats(query);
                        }
                    }
                }
            }
            return JsonConvert.SerializeObject(dataPoints);
        }
    }
}
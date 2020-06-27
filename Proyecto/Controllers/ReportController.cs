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
            ReportModel model = new ReportModel();
            model.CategoryList = new List<SelectListItem>();
            model.CategoryList.Add(new SelectListItem() { Text = "Country", Value = "Country" });
            model.CategoryList.Add(new SelectListItem() { Text = "Skills", Value = "Skills" });
            model.CategoryList.Add(new SelectListItem() { Text = "Languages", Value = "Languages" });
            model.CategoryList.Add(new SelectListItem() { Text = "Hobbies", Value = "Hobbies" });
            return View(model);
        }

        //[HttpPost]
        //public ActionResult ReportUsers(ReportModel model)
        //{
        //    ReportDBHandle sdb = new ReportDBHandle();
        //    string query;
        //    if (model.selectedCategory.Contains("Country"))
        //    {
        //        query = "SELECT C.addressCountry As 'Value', (Count(*)) As 'Count' " +
        //                        " FROM CommunityMember C " +
        //                        " Group By C.addressCountry ";
        //        List<ReportModel> dataPoints = sdb.GetCountryStats(query);
        //        ViewBag.Country = JsonConvert.SerializeObject(dataPoints);
        //    }

        //    if (model.selectedCategory.Contains("Skills"))
        //    {
        //        query = "SELECT H.skillName As 'Value', (Count(*)) As 'Count' " +
        //                        " FROM CommunityMember C, HAS_SKILL H, Skill S " +
        //                        " WHERE C.email = H.email  " +
        //                        " AND H.category = S.subjectCategory " +
        //                        " AND H.skillName = subjectSkillName " +
        //                        " Group By H.skillName ";
        //        List<ReportModel> dataPoints = sdb.GetCountryStats(query);
        //        ViewBag.Skills = JsonConvert.SerializeObject(dataPoints);
        //    }


        //    if (model.selectedCategory.Contains("Languages"))
        //    {
        //        query = "SELECT C.languages As 'Value', (Count(*)) As 'Count' " +
        //                        " FROM CommunityMember C " +
        //                        " Group By C.languages ";
        //        List<ReportModel> dataPoints = sdb.GetCountryStats(query);
        //        ViewBag.Languages = JsonConvert.SerializeObject(dataPoints);
        //    }

        //    if (model.selectedCategory.Contains("Hobbies"))
        //    {
        //        query = "SELECT C.hobbies As 'Value', (Count(*)) As 'Count' " +
        //                        " FROM CommunityMember C " +
        //                        " Group By C.hobbies ";
        //        List<ReportModel> dataPoints = sdb.GetCountryStats(query);
        //        ViewBag.Hobbies = JsonConvert.SerializeObject(dataPoints);
        //    }

        //    model.CategoryList = new List<SelectListItem>();
        //    model.CategoryList.Add(new SelectListItem() { Text = "Country", Value = "Country" });
        //    model.CategoryList.Add(new SelectListItem() { Text = "Skills", Value = "Skills" });
        //    model.CategoryList.Add(new SelectListItem() { Text = "Languages", Value = "Languages" });
        //    model.CategoryList.Add(new SelectListItem() { Text = "Hobbies", Value = "Hobbies" });
        //    return View(model);
        //}

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

            string valueReturned = JsonConvert.SerializeObject(dataPoints);
            return valueReturned;
        }
    }
}
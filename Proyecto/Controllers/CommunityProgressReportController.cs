using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proyecto.Models;

namespace Proyecto.Controllers
{
    public class CommunityProgressReportController : Controller
    {
        public ActionResult Index()
        {
            var model = new CommunityProgressReportModel
            {
                SelectedFiltersIds = new[] { 2 },
                Filters = GetAllFilters()
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(CommunityProgressReportModel model)
        {
            model.Filters = GetAllFilters();
            if (model.SelectedFiltersIds != null)
            {
                List<SelectListItem> selectedItems = model.Filters.Where(p => model.SelectedFiltersIds.Contains(int.Parse(p.Value))).ToList();
                foreach (var filter in selectedItems)
                {
                    filter.Selected = true;
                    ViewBag.Message += filter.Text + " | ";
                }
            }
            return View(model);
        }

        public List<SelectListItem> GetAllFilters()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Article Count", Value = "1" });
            items.Add(new SelectListItem { Text = "Article Points", Value = "2" });
            items.Add(new SelectListItem { Text = "Article Count Peer Category And Topic", Value = "3" });
            items.Add(new SelectListItem { Text = "Access Count Peer Category And Topic", Value = "4" });
            return items;
        }

    }
}
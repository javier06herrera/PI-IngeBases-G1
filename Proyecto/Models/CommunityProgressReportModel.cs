using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto.Models
{
    public class CommunityProgressReportModel
    {
        public int[] SelectedFiltersIds { get; set; }
        public IEnumerable<SelectListItem> Filters { get; set; }
    }
}
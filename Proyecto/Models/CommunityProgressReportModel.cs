using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto.Models
{
    public class CommunityProgressReportModel
    {
        public string[] SelectedMemberRanks { get; set; }
        public string[] SelectedFilters { get; set; }
    }
}
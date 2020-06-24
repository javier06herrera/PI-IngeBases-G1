using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Runtime.Serialization;

namespace Proyecto.Models
{

    [DataContract]
    public class ReportModel
    {
        public string[] selectedCategory { get; set; }
        public List<SelectListItem> CategoryList { get; set; }
        public int count { get; set; }
        public string value { get; set; }

    }
}
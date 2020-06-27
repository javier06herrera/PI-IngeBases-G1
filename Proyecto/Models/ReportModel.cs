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

        public ReportModel()
        {
            this.Label = "";
            this.Y = null;
        }
        public ReportModel(string label, int y)
        {
            this.Label = label;
            this.Y = y;
        }
        public string[] selectedCategory { get; set; }
        public List<SelectListItem> CategoryList { get; set; }

        [DataMember(Name = "y")]
        public Nullable<int> Y = null;
        //public int count { get; set; }
        [DataMember(Name = "label")]
        public string Label = "";
        //public string value { get; set; }



    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto.Controllers;
using Proyecto.Models;

namespace Proyecto.Tests.Controllers
{
    interface IReport
    {
        //List<ReportModel> GetCountryStats(string query);
        List<ReportModel> GetCountryStats { get; set; }
    }
}

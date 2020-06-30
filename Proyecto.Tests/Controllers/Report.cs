using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto.Controllers;
using Proyecto.Models;

namespace Proyecto.Tests.Controllers
{
    class Report
    {
        private readonly IReport _report;
        //public List<ReportModel> GetCountryStats(string query)
        //{
        //    throw new NotImplementedException();
        //}
        public Report(IReport report)
        {
            if(report == null)
            {
                throw new ArgumentNullException(nameof(report));
            }
            _report = report;
        }

        public List<ReportModel> GetCountryStats() => _report.GetCountryStats;
    }
}

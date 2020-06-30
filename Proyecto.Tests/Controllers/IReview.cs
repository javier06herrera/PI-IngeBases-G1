using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Proyecto.Tests.Controllers
{
    public interface IReview
    {
        int articleId { get; set; }
        string email { get; set; }
        string comments { get; set; }
        int generalOpinion { get; set; }
        int communityContribution { get; set; }
        int articleStructure { get; set; }
        int totalGrade { get; set; }        
        string state { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto.Controllers
{
    public class CoordinatorController : Controller
    {
        // GET: Coordinator
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Coordinator()
        {
            return View();
        }

        public ActionResult PendingVeredicts()
        {
            ReviewController rController = new ReviewController();
            return rController.PendingVeredicts();
        }

    }
}
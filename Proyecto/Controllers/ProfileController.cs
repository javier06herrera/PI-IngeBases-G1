using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proyecto.Models;

namespace Proyecto.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Profile()
        {
            ProfileDBHandle sdb = new ProfileDBHandle();
            ProfileModel memberProfile = sdb.getMemberProfile(1);
            return View(memberProfile);
        }
    }
}
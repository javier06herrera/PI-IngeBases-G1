﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto.Controllers
{
    public class CoreMemberController : Controller
    {
        // GET: CoreMember
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CoreMember()
        {
            return View();
        }
    }
}
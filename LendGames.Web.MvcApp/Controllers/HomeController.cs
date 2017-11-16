using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LendGames.Web.MvcApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (ConnectedId != 0)
                return RedirectToAction("Dashboard", "Account");
            else
                return View();
        }
    }
}
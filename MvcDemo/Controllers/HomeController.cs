using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jaytwo.AspNet.MvcDemo.Controllers
{
    public class HomeController : Controller
    {
        // GET: /Public/

        public ActionResult Index()
        {
            return View();
        }
    }
}

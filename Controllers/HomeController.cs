using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc4.OpenId.Sample.Security;

namespace Mvc4.OpenId.Sample.Controllers
{
    public class HomeController : Controller
    {
        [OpenIdAuthorize]
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to kick-start your ASP.NET MVC application.";

            return View();
        }

        [OpenIdAuthorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}

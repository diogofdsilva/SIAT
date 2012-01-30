using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facebook;
using Facebook.Web;
using SIAT.Operations;
using SIAT.WebApplication.Models;
using Facebook.Web.Mvc;

namespace SIAT.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        
        //
        // GET: /Home/ProfileInfo/

        [Authorize]
        public ActionResult ProfileInfo()
        {
            var client = new FacebookWebClient();

            dynamic me = client.Get("me");
            ViewBag.Id = me.id;
            ViewBag.Name = me.name;
            ViewBag.ImageURL = string.Format(@"http://graph.facebook.com/{0}/picture?type=large", @ViewBag.Id);
            return View();
        }
    }
}

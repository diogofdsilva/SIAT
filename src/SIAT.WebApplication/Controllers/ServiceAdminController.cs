using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIAT.Operations;

namespace SIAT.WebApplication.Controllers
{
    public class ServiceAdminController : Controller
    {
        //
        // GET: /ServiceAdmin/

        public ActionResult Index()
        {
            var siatOperations = (SIATOperations)this.HttpContext.Application["siat"];

            ViewBag.SIAT = siatOperations.ProxysManager.SIATService.IsAlive();
            ViewBag.OSM = siatOperations.ProxysManager.OSMService.IsAlive();
            ViewBag.User = siatOperations.ProxysManager.UserInfoService.IsAlive();

            return View("Index");
        }

        public ActionResult ServerRefresh(int id)
        {
            var siatOperations = (SIATOperations)this.HttpContext.Application["siat"];

            try
            {
                switch (id)
                {
                    case 0: // OSM Service
                        siatOperations.ProxysManager.OSMService.Discover();
                        break;
                    case 1: // SIAT Service
                        siatOperations.ProxysManager.SIATService.Discover();
                        break;
                    case 2: // USER Service
                        siatOperations.ProxysManager.UserInfoService.Discover();
                        break;
                    case 3: // TODOS
                        siatOperations.ProxysManager.UserInfoService.Discover();
                        siatOperations.ProxysManager.SIATService.Discover();
                        siatOperations.ProxysManager.OSMService.Discover();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError("", exception.Message);
            }

            return Index();
        }

        public ActionResult ServerWakeUp(int id)
        {
            var siatOperations = (SIATOperations)this.HttpContext.Application["siat"];

            try
            {
                switch (id)
                {
                    case 0: // OSM Service
                        siatOperations.ProxysManager.OSMService.UltimateWakeUp();
                        //siatOperations.ProxysManager.OSMService.Build();
                        break;
                    case 1: // SIAT Service
                        siatOperations.ProxysManager.SIATService.UltimateWakeUp();
                        //siatOperations.ProxysManager.SIATService.Build();
                        break;
                    case 2: // USER Service
                        siatOperations.ProxysManager.UserInfoService.UltimateWakeUp();
                        //siatOperations.ProxysManager.UserInfoService.Build();
                        break;
                    case 3: // TODOS
                        siatOperations.ProxysManager.OSMService.UltimateWakeUp();
                        //siatOperations.ProxysManager.OSMService.Build();

                        siatOperations.ProxysManager.SIATService.UltimateWakeUp();
                        //siatOperations.ProxysManager.SIATService.Build();

                        siatOperations.ProxysManager.UserInfoService.UltimateWakeUp();
                        //siatOperations.ProxysManager.UserInfoService.Build();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError("", exception.Message);
            }

            return Index();
        }

        public ActionResult InsertURI()
        {
            return View();
        }

        [HttpPost]
        public ActionResult InsertURI(FormCollection ob)
        {
            var siatOperations = (SIATOperations)this.HttpContext.Application["siat"];
            string uri = ob["uri"];
            string service = ob["AllServices"];

            switch (service)
            {
                case "SIAT":
                    siatOperations.ProxysManager.SIATService.Build(new Uri(uri));
                    break;
                case "User":
                    siatOperations.ProxysManager.UserInfoService.Build(new Uri(uri));
                    break;
                case "OSM":
                    siatOperations.ProxysManager.OSMService.Build(new Uri(uri));
                    break;
                default:
                    return View();
            }


            return RedirectToAction("ServicesStatus");
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIAT.Operations;
using SIAT.Service.Contract.DTO;
using Occurrence = SIAT.WebApplication.Models.Occurrence;

namespace SIAT.WebApplication.Controllers
{
    public class OccurrenceController : Controller
    {
        //
        // GET: /Occurrence/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Occurrence/

        public ActionResult List()
        {
            var siatOperations = (SIATOperations)this.HttpContext.Application["siat"];
            var list = siatOperations.ListAllOccurrences().Select(o => new Occurrence()
                                                                           {
                                                                               WayName = o.WayName,
                                                                               Latitude = Convert.ToDecimal(o.Latitude),
                                                                               Longitude = Convert.ToDecimal(o.Longitude),
                                                                               Intensity = o.Intensity.ToString(),
                                                                               Id = o.Id
                                                                           });
            
            return View(list);
        }

        public ActionResult Occurrences()
        {
            var siatOperations = (SIATOperations)this.HttpContext.Application["siat"];
            var list = siatOperations.ListAllOccurrences().Select(o => new Occurrence()
            {
                WayName = o.WayName,
                Latitude = Convert.ToDecimal(o.Latitude),
                Longitude = Convert.ToDecimal(o.Longitude),
                Intensity = o.Intensity.ToString(),
                Id = o.Id
            });
            
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Occurrence/Details/5

        public ActionResult Details(int id)
        {
            var siatOperations = (SIATOperations)this.HttpContext.Application["siat"];
            var findOccurrence = siatOperations.FindOccurrence(id);
            
            return View(new Occurrence()
            {
                WayName = findOccurrence.WayName,
                Latitude = Convert.ToDecimal(findOccurrence.Latitude),
                Longitude = Convert.ToDecimal(findOccurrence.Longitude),
                Intensity = findOccurrence.Intensity.ToString(),
                Id = findOccurrence.Id
            });
        }

        //
        // GET: /Occurrence/Create

        public ActionResult Create()
        {
            List<string> alertLevels = new List<string>{ "Road Block", "High Traffic", "Medium Traffic" };
            ViewBag.Levels = alertLevels;

            return View();
        } 

        //
        // POST: /Occurrence/Create

        [HttpPost]
        public ActionResult Create(Occurrence occurrence)
        {
            try
            {
                var siatOperations = (SIATOperations)this.HttpContext.Application["siat"];

                var o = new Service.Contract.DTO.Occurrence();

                o.Description = occurrence.Description;
                o.Latitude = (double) occurrence.Latitude;
                o.Longitude = (double) occurrence.Longitude;
                
                switch (occurrence.Intensity)
                {
                    case "Road Block":
                        o.Intensity = 0;
                        break;
                    case "Medium Traffic":
                        o.Intensity = 10; // +/- 40km/h
                        break;
                    case "High Traffic":
                        o.Intensity = 5; // +/- 20km/h
                        break;
                }

                siatOperations.SendAlert(o);
                
                return RedirectToAction("List");
            }
            catch
            {
                return Create();
            }
        }
        
        //
        // GET: /Occurrence/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Occurrence/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIAT.Operations;
using SIAT.UserInfo.Contract.DTO;

namespace SIAT.WebApplication.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/
        [Authorize]
        public ActionResult Index()
        {
            User user;

            var siatOperations = (SIATOperations)this.HttpContext.Application["siat"];

            user = siatOperations.GetUserByEmail(HttpContext.User.Identity.Name);
            
            return View(user);
        }

        //
        // GET: /User/Details/5
        [Authorize]
        public ActionResult Details(long id)
        {
            
            
            
            return View();
        }

        //
        // GET: /User/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /User/Create

        [HttpPost]
        public ActionResult Create(User model)
        {
            try
            {
                var siatOperations = (SIATOperations)this.HttpContext.Application["siat"];

                model = siatOperations.CreateNewUser(model);

                if (model == null)
                {
                    ModelState.AddModelError("", "Please review your data");    

                    return View();
                }

                

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /User/Edit/5
        [Authorize]
        public ActionResult Edit(long id)
        {
            return View();
        }

        //
        // POST: /User/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(long id, User collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /User/Delete/5
        [Authorize]
        public ActionResult Delete(long id)
        {
            return View();
        }

        //
        // POST: /User/Delete/5

        [HttpPost]
        [Authorize]
        public ActionResult Delete(long id, FormCollection collection)
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

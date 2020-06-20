using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Neshagostar.DAL.DataModel;
using Neshagostar.DAL.DataModel.ActivityLogging.PersonnelActivity;
using Neshagostar.WebUI.App_Start;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Neshagostar.WebUI.Areas.PersonnelManagement.Models.JustifyRelated;

namespace Neshagostar.WebUI.Areas.PersonnelManagement.Controllers
{
    public class JustifiesController : Controller
    {
        private NeshagostarContext context = new NeshagostarContext();


        private PersonnelSignInManager _signInManager;
        private PersonnelManager _userManager;
        private PersonnelRoleManager _roleManager;

        public PersonnelRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<PersonnelRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        public PersonnelSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<PersonnelSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        public PersonnelManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<PersonnelManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        // GET: PersonnelManagement/Justifies
        public ActionResult Index()
        {
            var justifies = context.Justifies.Include(j => j.Personnel).ToList();
            return View(justifies);
        }

        // GET: PersonnelManagement/Justifies/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Justify justify = context.Justifies.Find(id);
            if (justify == null)
            {
                return HttpNotFound();
            }
            return View(justify);
        }

        [HttpPost]
        // GET: PersonnelManagement/Justifies/Create
        public JsonResult Create(JustifyDTO justify)
        {
            var userId = this.AuthenticationManager.User.Identity.GetUserId();


            if (ModelState.IsValid)
            {
                justify.Id = Guid.NewGuid();
                justify.PersonnelId = userId;
                Justify newJustify = new Justify
                {
                    Id = justify.Id,
                    PersonnelId = userId,
                    Action = justify.Action,
                    Comments = justify.Comments,
                    Entity = justify.Entity,
                    Reason = justify.Reason
                };
                context.Justifies.Add(newJustify);
                context.SaveChanges();
                return Json(new {value = true},JsonRequestBehavior.AllowGet);
            }

            return Json(new { value = false }, JsonRequestBehavior.AllowGet);
        }

        // POST: PersonnelManagement/Justifies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,PersonnelId,Action,Entity,Reason,Comments")] Justify justify)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        justify.Id = Guid.NewGuid();
        //        context.Justifies.Add(justify);
        //        context.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.PersonnelId = new SelectList(context.Users, "Id", "Email", justify.PersonnelId);
        //    return View(justify);
        //}

        // GET: PersonnelManagement/Justifies/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Justify justify = context.Justifies.Find(id);
            if (justify == null)
            {
                return HttpNotFound();
            }
            ViewBag.PersonnelId = new SelectList(context.Users, "Id", "Email", justify.PersonnelId);
            return View(justify);
        }

        // POST: PersonnelManagement/Justifies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PersonnelId,Action,Entity,Reason,Comments")] Justify justify)
        {
            if (ModelState.IsValid)
            {
                context.Entry(justify).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PersonnelId = new SelectList(context.Users, "Id", "Email", justify.PersonnelId);
            return View(justify);
        }

        // GET: PersonnelManagement/Justifies/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Justify justify = context.Justifies.Find(id);
            if (justify == null)
            {
                return HttpNotFound();
            }
            return View(justify);
        }

        // POST: PersonnelManagement/Justifies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Justify justify = context.Justifies.Find(id);
            context.Justifies.Remove(justify);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

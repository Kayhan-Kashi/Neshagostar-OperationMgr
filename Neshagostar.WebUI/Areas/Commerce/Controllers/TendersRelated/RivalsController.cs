using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Neshagostar.DAL.DataModel;
using Neshagostar.DAL.DataModel.CommerceRelated.TenderRelated;

namespace Neshagostar.WebUI.Areas.Commerce.Controllers.TendersRelated
{
    [Authorize(Roles = "commerce-manager, CEO")]
    public class RivalsController : Controller
    {
        private NeshagostarContext db = new NeshagostarContext();

        // GET: Commerce/Rivals
        public ActionResult Index()
        {
            return View("~/Areas/Commerce/Views/TendersRelated/Rivals/Index.cshtml", db.Rivals.ToList());
        }

        // GET: Commerce/Rivals/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rival rival = db.Rivals.Find(id);
            if (rival == null)
            {
                return HttpNotFound();
            }
            return View("~/Areas/Commerce/Views/TendersRelated/Rivals/Details.cshtml", rival);
        }

        // GET: Commerce/Rivals/Create
        public ActionResult Create()
        {
            return View("~/Areas/Commerce/Views/TendersRelated/Rivals/Create.cshtml");
        }

        // POST: Commerce/Rivals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,City")] Rival rival)
        {
            if (ModelState.IsValid)
            {
                rival.Id = Guid.NewGuid();
                db.Rivals.Add(rival);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("~/Areas/Commerce/Views/TendersRelated/Rivals/Create.cshtml", rival);
        }

        // GET: Commerce/Rivals/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rival rival = db.Rivals.Find(id);
            if (rival == null)
            {
                return HttpNotFound();
            }
            return View("~/Areas/Commerce/Views/TendersRelated/Rivals/Edit.cshtml", rival);
        }

        // POST: Commerce/Rivals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,City")] Rival rival)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rival).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("~/Areas/Commerce/Views/TendersRelated/Rivals/Edit.cshtml", rival);
        }

        // GET: Commerce/Rivals/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rival rival = db.Rivals.Find(id);
            if (rival == null)
            {
                return HttpNotFound();
            }
            return View("~/Areas/Commerce/Views/TendersRelated/Rivals/Delete.cshtml", rival);
        }

        // POST: Commerce/Rivals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Rival rival = db.Rivals.Find(id);
            db.Rivals.Remove(rival);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

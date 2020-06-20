using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Neshagostar.DAL.DataModel;
using Neshagostar.DAL.DataModel.CommerceRelated.ProductsRelated;

namespace Neshagostar.WebUI.Areas.Commerce.Controllers.ProductsRelated
{
    public class CouplingsController : Controller
    {
        private NeshagostarContext db = new NeshagostarContext();

        // GET: Commerce/Couplings
        public ActionResult Index()
        {
            return View(db.Couplings.OrderBy(p => p.Code).ToList());
        }

        // GET: Commerce/Couplings/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coupling coupling = db.Couplings.Find(id);
            if (coupling == null)
            {
                return HttpNotFound();
            }
            return View(coupling);
        }

        // GET: Commerce/Couplings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Commerce/Couplings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Coupling coupling)
        {
            if (ModelState.IsValid)
            {
                coupling.Id = Guid.NewGuid();
                db.Couplings.Add(coupling);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(coupling);
        }

        // GET: Commerce/Couplings/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coupling coupling = db.Couplings.Find(id);
            if (coupling == null)
            {
                return HttpNotFound();
            }
            return View(coupling);
        }

        // POST: Commerce/Couplings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Coupling coupling)
        {
            if (ModelState.IsValid)
            {
                db.Entry(coupling).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(coupling);
        }

        // GET: Commerce/Couplings/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coupling coupling = db.Couplings.Find(id);
            if (coupling == null)
            {
                return HttpNotFound();
            }
            return View(coupling);
        }

        // POST: Commerce/Couplings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Coupling coupling = db.Couplings.Find(id);
            db.Couplings.Remove(coupling);
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

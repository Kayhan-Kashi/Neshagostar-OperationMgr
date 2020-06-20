using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Neshagostar.DAL.DataModel;
using Neshagostar.DAL.DataModel.ProductionRelated;

namespace Neshagostar.WebUI.Areas.Production.Controllers
{
    public class SaloonsController : Controller
    {
        private NeshagostarContext db = new NeshagostarContext();

        // GET: Production/Saloons
        public ActionResult Index()
        {
            return View(db.Saloons.ToList());
        }

        // GET: Production/Saloons/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Saloon saloon = db.Saloons.Find(id);
            if (saloon == null)
            {
                return HttpNotFound();
            }
            return View(saloon);
        }

        // GET: Production/Saloons/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Production/Saloons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ManagerId,Name,Comments,Code")] Saloon saloon)
        {
            if (ModelState.IsValid)
            {
                saloon.Id = Guid.NewGuid();
                db.Saloons.Add(saloon);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(saloon);
        }

        // GET: Production/Saloons/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Saloon saloon = db.Saloons.Find(id);
            if (saloon == null)
            {
                return HttpNotFound();
            }
            return View(saloon);
        }

        // POST: Production/Saloons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ManagerId,Name,Comments,Code")] Saloon saloon)
        {
            if (ModelState.IsValid)
            {
                db.Entry(saloon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(saloon);
        }

        // GET: Production/Saloons/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Saloon saloon = db.Saloons.Find(id);
            if (saloon == null)
            {
                return HttpNotFound();
            }
            return View(saloon);
        }

        // POST: Production/Saloons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Saloon saloon = db.Saloons.Find(id);
            db.Saloons.Remove(saloon);
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

        [HttpGet]
        public JsonResult GetSaloons()
        {
            return Json(db.Saloons.OrderBy(s => s.Code).ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}

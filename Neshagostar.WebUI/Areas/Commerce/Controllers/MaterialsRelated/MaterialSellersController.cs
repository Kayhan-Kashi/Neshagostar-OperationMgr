using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Neshagostar.DAL.DataModel;
using Neshagostar.DAL.DataModel.CommerceRelated.MaterialRelated;

namespace Neshagostar.WebUI.Areas.Commerce.Controllers.MaterialsRelated
{
    public class MaterialSellersController : Controller
    {
        private NeshagostarContext db = new NeshagostarContext();

        // GET: Commerce/MaterialSellers
        public ActionResult Index()
        {
            return View(db.MaterialSellers.ToList());
        }

        // GET: Commerce/MaterialSellers/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaterialSeller materialSeller = db.MaterialSellers.Find(id);
            if (materialSeller == null)
            {
                return HttpNotFound();
            }
            return View(materialSeller);
        }

        // GET: Commerce/MaterialSellers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Commerce/MaterialSellers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] MaterialSeller materialSeller)
        {
            if (ModelState.IsValid)
            {
                materialSeller.Id = Guid.NewGuid();
                db.MaterialSellers.Add(materialSeller);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(materialSeller);
        }

        // GET: Commerce/MaterialSellers/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaterialSeller materialSeller = db.MaterialSellers.Find(id);
            if (materialSeller == null)
            {
                return HttpNotFound();
            }
            return View(materialSeller);
        }

        // POST: Commerce/MaterialSellers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] MaterialSeller materialSeller)
        {
            if (ModelState.IsValid)
            {
                db.Entry(materialSeller).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(materialSeller);
        }

        // GET: Commerce/MaterialSellers/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaterialSeller materialSeller = db.MaterialSellers.Find(id);
            if (materialSeller == null)
            {
                return HttpNotFound();
            }
            return View(materialSeller);
        }

        // POST: Commerce/MaterialSellers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            MaterialSeller materialSeller = db.MaterialSellers.Find(id);
            db.MaterialSellers.Remove(materialSeller);
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

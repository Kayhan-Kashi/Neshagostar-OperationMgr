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
    public class MaterialSpecificationsController : Controller
    {
        private NeshagostarContext db = new NeshagostarContext();

        // GET: Commerce/MaterialSpecifications
        public ActionResult Index()
        {
            return View(db.MaterialSpecifications.ToList());
        }

        // GET: Commerce/MaterialSpecifications/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaterialSpecification materialSpecification = db.MaterialSpecifications.Find(id);
            if (materialSpecification == null)
            {
                return HttpNotFound();
            }
            return View(materialSpecification);
        }

        // GET: Commerce/MaterialSpecifications/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Commerce/MaterialSpecifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Specification")] MaterialSpecification materialSpecification)
        {
            if (ModelState.IsValid)
            {
                materialSpecification.Id = Guid.NewGuid();
                db.MaterialSpecifications.Add(materialSpecification);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(materialSpecification);
        }

        // GET: Commerce/MaterialSpecifications/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaterialSpecification materialSpecification = db.MaterialSpecifications.Find(id);
            if (materialSpecification == null)
            {
                return HttpNotFound();
            }
            return View(materialSpecification);
        }

        // POST: Commerce/MaterialSpecifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Specification")] MaterialSpecification materialSpecification)
        {
            if (ModelState.IsValid)
            {
                db.Entry(materialSpecification).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(materialSpecification);
        }

        // GET: Commerce/MaterialSpecifications/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaterialSpecification materialSpecification = db.MaterialSpecifications.Find(id);
            if (materialSpecification == null)
            {
                return HttpNotFound();
            }
            return View(materialSpecification);
        }

        // POST: Commerce/MaterialSpecifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            MaterialSpecification materialSpecification = db.MaterialSpecifications.Find(id);
            db.MaterialSpecifications.Remove(materialSpecification);
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

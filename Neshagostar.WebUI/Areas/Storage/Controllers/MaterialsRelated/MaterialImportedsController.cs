using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Neshagostar.DAL.DataModel;
using Neshagostar.DAL.DataModel.StorageRelated.MaterialRelated;

namespace Neshagostar.WebUI.Areas.Storage.Controllers.MaterialsRelated
{
    public class MaterialImportedsController : Controller
    {
        private NeshagostarContext db = new NeshagostarContext();

        // GET: Storage/MaterialImporteds
        public ActionResult Index()
        {
            var materialImporteds = db.MaterialImporteds.Include(m => m.MaterialPurchased.MaterialSeller).OrderByDescending(d => d.DateOfImport);
            return View(materialImporteds.ToList());
        }

        // GET: Storage/MaterialImporteds/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaterialImported materialImported = db.MaterialImporteds.Find(id);
            if (materialImported == null)
            {
                return HttpNotFound();
            }
            return View(materialImported);
        }

        // GET: Storage/MaterialImporteds/Create
        public ActionResult Create()
        {
            ViewBag.MaterialPurchasedId = new SelectList(db.MaterialPurchaseds.Include(mbox => mbox.MaterialSpecification).Include(mbox => mbox.MaterialImporteds).Include(mbox => mbox.MaterialDedicateds).Include(mbox => mbox.MaterialSeller).OrderByDescending(md => md.DateOfPurchase).ToList().Where(d => d.AmountImported < d.AmountPurchased).Where(m => !m.IsConsumedCompletely).ToList(), "Id", "TitleWithNotImportedAmount");
            return View();
        }

        // POST: Storage/MaterialImporteds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MaterialPurchasedId,AmountImported,DateOfImport,CarriageNumber,Location,DriverName,CarriagePrice,OtherCost,Comments,LostAmount")] MaterialImported materialImported)
        {
            if (ModelState.IsValid)
            {
                materialImported.Id = Guid.NewGuid();
                db.MaterialImporteds.Add(materialImported);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaterialPurchasedId = new SelectList(db.MaterialPurchaseds, "Id", "Code", materialImported.MaterialPurchasedId);
            return View(materialImported);
        }

        // GET: Storage/MaterialImporteds/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaterialImported materialImported = db.MaterialImporteds.Find(id);
            if (materialImported == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaterialPurchasedId = new SelectList(db.MaterialPurchaseds, "Id", "Code", materialImported.MaterialPurchasedId);
            return View(materialImported);
        }

        // POST: Storage/MaterialImporteds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MaterialPurchasedId,AmountImported,DateOfImport,CarriageNumber,Location,DriverName,CarriagePrice,OtherCost,Comments,LostAmount")] MaterialImported materialImported)
        {
            if (ModelState.IsValid)
            {
                db.Entry(materialImported).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaterialPurchasedId = new SelectList(db.MaterialPurchaseds, "Id", "Code", materialImported.MaterialPurchasedId);
            return View(materialImported);
        }

        // GET: Storage/MaterialImporteds/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaterialImported materialImported = db.MaterialImporteds.Find(id);
            if (materialImported == null)
            {
                return HttpNotFound();
            }
            return View(materialImported);
        }

        // POST: Storage/MaterialImporteds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            MaterialImported materialImported = db.MaterialImporteds.Find(id);
            db.MaterialImporteds.Remove(materialImported);
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

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
    public class MaterialDedicatedsController : Controller
    {
        private NeshagostarContext db = new NeshagostarContext();

        // GET: Storage/MaterialDedicateds
        public ActionResult Index()
        {
            var materialDedicateds = db.MaterialDedicateds.Include(m => m.Device).Include(m => m.MaterialPurchased.MaterialSeller).OrderByDescending(d => d.DateOfDedication);
            return View(materialDedicateds.ToList());
        }

        // GET: Storage/MaterialDedicateds/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaterialDedicated materialDedicated = db.MaterialDedicateds.Find(id);
            if (materialDedicated == null)
            {
                return HttpNotFound();
            }
            return View(materialDedicated);
        }

        // GET: Storage/MaterialDedicateds/Create
        public ActionResult Create()
        {
            var materialList = db.MaterialPurchaseds.Include(mbox => mbox.MaterialSpecification).Include(mbox => mbox.MaterialImporteds).Include(mbox => mbox.MaterialDedicateds).Include(mbox => mbox.MaterialSeller).OrderByDescending(md => md.DateOfPurchase).ToList().Where(d => d.AmountImported > 0).Where(m => !m.IsConsumedCompletely).ToList();
            ViewBag.DeviceId = new SelectList(db.Devices, "Id", "Name");
            ViewBag.MaterialPurchasedId = new SelectList(materialList, "Id", "TitleWithRemaining");
            return View();
        }

        // POST: Storage/MaterialDedicateds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MaterialPurchasedId,DeviceId,DateOfDedication,AmountDedicated")] MaterialDedicated materialDedicated)
        {
            if (ModelState.IsValid)
            {
                materialDedicated.Id = Guid.NewGuid();
                db.MaterialDedicateds.Add(materialDedicated);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DeviceId = new SelectList(db.Devices, "Id", "Name", materialDedicated.DeviceId);
            ViewBag.MaterialPurchasedId = new SelectList(db.MaterialPurchaseds, "Id", "Code", materialDedicated.MaterialPurchasedId);
            return View(materialDedicated);
        }

        // GET: Storage/MaterialDedicateds/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaterialDedicated materialDedicated = db.MaterialDedicateds.Find(id);
            if (materialDedicated == null)
            {
                return HttpNotFound();
            }
            ViewBag.DeviceId = new SelectList(db.Devices, "Id", "Name", materialDedicated.DeviceId);
            ViewBag.MaterialPurchasedId = new SelectList(db.MaterialPurchaseds, "Id", "Code", materialDedicated.MaterialPurchasedId);
            return View(materialDedicated);
        }

        // POST: Storage/MaterialDedicateds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MaterialPurchasedId,DeviceId,DateOfDedication,AmountDedicated")] MaterialDedicated materialDedicated)
        {
            if (ModelState.IsValid)
            {
                db.Entry(materialDedicated).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DeviceId = new SelectList(db.Devices, "Id", "Name", materialDedicated.DeviceId);
            ViewBag.MaterialPurchasedId = new SelectList(db.MaterialPurchaseds, "Id", "Code", materialDedicated.MaterialPurchasedId);
            return View(materialDedicated);
        }

        // GET: Storage/MaterialDedicateds/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaterialDedicated materialDedicated = db.MaterialDedicateds.Find(id);
            if (materialDedicated == null)
            {
                return HttpNotFound();
            }
            return View(materialDedicated);
        }

        // POST: Storage/MaterialDedicateds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            MaterialDedicated materialDedicated = db.MaterialDedicateds.Find(id);
            db.MaterialDedicateds.Remove(materialDedicated);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult SetWithdrawedAmount(Guid materialDedicatedId, double amountToSet, string dateToset, string withdrawalComments)
        {
            var dedicated = db.MaterialDedicateds.Where(m => m.Id == materialDedicatedId).FirstOrDefault();
            dedicated.AmountWithdrawed = amountToSet;
            dedicated.WithdrawalComments += "/n" + withdrawalComments;
            dedicated.DateOfWithdrawal = dateToset;
            db.Entry(dedicated).State = EntityState.Modified;
            db.SaveChanges();
            return Json("succeed", JsonRequestBehavior.AllowGet);
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

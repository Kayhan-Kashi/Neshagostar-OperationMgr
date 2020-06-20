using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Neshagostar.DAL.DataModel;
using Neshagostar.DAL.DataModel.StorageRelated.OrdersRelated;

namespace Neshagostar.WebUI.Areas.Storage.Controllers.OrdersRelated
{
    public class SendingAllocatedsController : Controller
    {
        private NeshagostarContext db = new NeshagostarContext();

        // GET: Storage/SendingAllocateds
        public ActionResult Index()
        {
            var sendingAllocateds = db.SendingAllocateds.Include(s => s.AllocatedProduct);
            return View(sendingAllocateds.ToList());
        }

        // GET: Storage/SendingAllocateds/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SendingAllocated sendingAllocated = db.SendingAllocateds.Find(id);
            if (sendingAllocated == null)
            {
                return HttpNotFound();
            }
            return View(sendingAllocated);
        }

        // GET: Storage/SendingAllocateds/Create
        public ActionResult Create()
        {
            ViewBag.AllocatedProductId = new SelectList(db.AllocatedProducts, "Id", "Date");
            return View();
        }

        // POST: Storage/SendingAllocateds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AllocatedProductId,AmountSent")] SendingAllocated sendingAllocated)
        {
            if (ModelState.IsValid)
            {
                sendingAllocated.Id = Guid.NewGuid();
                db.SendingAllocateds.Add(sendingAllocated);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AllocatedProductId = new SelectList(db.AllocatedProducts, "Id", "Date", sendingAllocated.AllocatedProductId);
            return View(sendingAllocated);
        }

        // GET: Storage/SendingAllocateds/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SendingAllocated sendingAllocated = db.SendingAllocateds.Find(id);
            if (sendingAllocated == null)
            {
                return HttpNotFound();
            }
            ViewBag.AllocatedProductId = new SelectList(db.AllocatedProducts, "Id", "Date", sendingAllocated.AllocatedProductId);
            return View(sendingAllocated);
        }

        // POST: Storage/SendingAllocateds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AllocatedProductId,AmountSent")] SendingAllocated sendingAllocated)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sendingAllocated).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AllocatedProductId = new SelectList(db.AllocatedProducts, "Id", "Date", sendingAllocated.AllocatedProductId);
            return View(sendingAllocated);
        }

        // GET: Storage/SendingAllocateds/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SendingAllocated sendingAllocated = db.SendingAllocateds.Find(id);
            if (sendingAllocated == null)
            {
                return HttpNotFound();
            }
            return View(sendingAllocated);
        }

        // POST: Storage/SendingAllocateds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            SendingAllocated sendingAllocated = db.SendingAllocateds.Find(id);
            db.SendingAllocateds.Remove(sendingAllocated);
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

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

namespace Neshagostar.WebUI.Areas.Commerce.Controllers.MaterialsRelated
{
    public class MaterialPurchasedsController : Controller
    {
        private NeshagostarContext db = new NeshagostarContext();

        // GET: Commerce/MaterialPurchaseds
        public ActionResult Index()
        {
            var materialPurchaseds = db.MaterialPurchaseds.Include(m => m.MaterialSeller).Include(m => m.MaterialSpecification);
            return View(materialPurchaseds.ToList());
        }

        // GET: Commerce/MaterialPurchaseds/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaterialPurchased materialPurchased = db.MaterialPurchaseds.Find(id);
            if (materialPurchased == null)
            {
                return HttpNotFound();
            }
            return View(materialPurchased);
        }

        // GET: Commerce/MaterialPurchaseds/Create
        public ActionResult Create()
        {
            ViewBag.MaterialSellerId = new SelectList(db.MaterialSellers, "Id", "Name");
            ViewBag.MaterialSpecificationId = new SelectList(db.MaterialSpecifications, "Id", "Specification");
            return View();
        }

        // POST: Commerce/MaterialPurchaseds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MaterialSellerId,MaterialSpecificationId,Code,DateOfPurchase,Price,AddedCostPrice,PurchaseMethod,Escont,OtherCosts,AmountPurchased")] MaterialPurchased materialPurchased)
        {
            if (ModelState.IsValid)
            {
                materialPurchased.Id = Guid.NewGuid();
                db.MaterialPurchaseds.Add(materialPurchased);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaterialSellerId = new SelectList(db.MaterialSellers, "Id", "Name", materialPurchased.MaterialSellerId);
            ViewBag.MaterialSpecificationId = new SelectList(db.MaterialSpecifications, "Id", "Specification", materialPurchased.MaterialSpecificationId);
            return View(materialPurchased);
        }

        // GET: Commerce/MaterialPurchaseds/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaterialPurchased materialPurchased = db.MaterialPurchaseds.Find(id);
            if (materialPurchased == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaterialSellerId = new SelectList(db.MaterialSellers, "Id", "Name", materialPurchased.MaterialSellerId);
            ViewBag.MaterialSpecificationId = new SelectList(db.MaterialSpecifications, "Id", "Specification", materialPurchased.MaterialSpecificationId);
            return View(materialPurchased);
        }

        // POST: Commerce/MaterialPurchaseds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MaterialSellerId,MaterialSpecificationId,Code,DateOfPurchase,Price,AddedCostPrice,PurchaseMethod,Escont,OtherCosts,AmountPurchased")] MaterialPurchased materialPurchased)
        {
            if (ModelState.IsValid)
            {
                db.Entry(materialPurchased).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaterialSellerId = new SelectList(db.MaterialSellers, "Id", "Name", materialPurchased.MaterialSellerId);
            ViewBag.MaterialSpecificationId = new SelectList(db.MaterialSpecifications, "Id", "Specification", materialPurchased.MaterialSpecificationId);
            return View(materialPurchased);
        }

        // GET: Commerce/MaterialPurchaseds/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaterialPurchased materialPurchased = db.MaterialPurchaseds.Find(id);
            if (materialPurchased == null)
            {
                return HttpNotFound();
            }
            return View(materialPurchased);
        }

        // POST: Commerce/MaterialPurchaseds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            MaterialPurchased materialPurchased = db.MaterialPurchaseds.Find(id);
            db.MaterialPurchaseds.Remove(materialPurchased);
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

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Neshagostar.DAL.DataModel;
using Neshagostar.DAL.DataModel.StorageRelated.WastageRelated;

namespace Neshagostar.WebUI.Areas.Storage.Controllers.WastagesRelated
{
    public class ImportedUnStandardProductsController : Controller
    {
        private NeshagostarContext db = new NeshagostarContext();

        // GET: Storage/ImportedUnStandardProducts
        public ActionResult Index()
        {
            var importedUnStandardProducts = db.ImportedUnStandardProducts.Include(i => i.OrderItem).Include(i => i.Product).Include(i => i.ProductIssue).OrderByDescending(p => p.Date);
            return View(importedUnStandardProducts.ToList());
        }

        // GET: Storage/ImportedUnStandardProducts/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImportedUnStandardProduct importedUnStandardProduct = db.ImportedUnStandardProducts.Find(id);
            if (importedUnStandardProduct == null)
            {
                return HttpNotFound();
            }
            return View(importedUnStandardProduct);
        }

        // GET: Storage/ImportedUnStandardProducts/Create
        public ActionResult Create()
        {
            ViewBag.ProductIssues = db.ProductIssues.Select(d => new SelectListItem
            {
                Text = d.Title,
                Value = d.Id.ToString()
            }).ToList();


            ViewBag.OrderItemId = new SelectList(db.OrderItems, "Id", "Comments");
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Code");
            return View(new ImportedUnStandardProduct { Date = PersianDateTime.Now.ToString().Substring(0, 10) });
        }

        // POST: Storage/ImportedUnStandardProducts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProductId,OrderItemId,ProductIssueId,Date,AmountImportedInMeter,AmountImportedInKilo,Issue,Comments")] ImportedUnStandardProduct importedUnStandardProduct)
        {
            if (ModelState.IsValid)
            {
                importedUnStandardProduct.Id = Guid.NewGuid();
                db.ImportedUnStandardProducts.Add(importedUnStandardProduct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrderItemId = new SelectList(db.OrderItems, "Id", "Comments", importedUnStandardProduct.OrderItemId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Code", importedUnStandardProduct.ProductId);
            ViewBag.ProductIssueId = new SelectList(db.ProductIssues, "Id", "Title", importedUnStandardProduct.ProductIssueId);
            return View(importedUnStandardProduct);
        }

        // GET: Storage/ImportedUnStandardProducts/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImportedUnStandardProduct importedUnStandardProduct = db.ImportedUnStandardProducts.Find(id);
            if (importedUnStandardProduct == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderItemId = new SelectList(db.OrderItems, "Id", "Comments", importedUnStandardProduct.OrderItemId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Code", importedUnStandardProduct.ProductId);
            ViewBag.ProductIssueId = new SelectList(db.ProductIssues, "Id", "Title", importedUnStandardProduct.ProductIssueId);
            return View(importedUnStandardProduct);
        }

        // POST: Storage/ImportedUnStandardProducts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProductId,OrderItemId,ProductIssueId,Date,AmountImportedInMeter,AmountImportedInKilo,Issue,Comments")] ImportedUnStandardProduct importedUnStandardProduct)
        {
            if (ModelState.IsValid)
            {
                db.Entry(importedUnStandardProduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrderItemId = new SelectList(db.OrderItems, "Id", "Comments", importedUnStandardProduct.OrderItemId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Code", importedUnStandardProduct.ProductId);
            ViewBag.ProductIssueId = new SelectList(db.ProductIssues, "Id", "Title", importedUnStandardProduct.ProductIssueId);
            return View(importedUnStandardProduct);
        }

        // GET: Storage/ImportedUnStandardProducts/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImportedUnStandardProduct importedUnStandardProduct = db.ImportedUnStandardProducts.Find(id);
            if (importedUnStandardProduct == null)
            {
                return HttpNotFound();
            }
            return View(importedUnStandardProduct);
        }

        // POST: Storage/ImportedUnStandardProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ImportedUnStandardProduct importedUnStandardProduct = db.ImportedUnStandardProducts.Find(id);
            db.ImportedUnStandardProducts.Remove(importedUnStandardProduct);
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

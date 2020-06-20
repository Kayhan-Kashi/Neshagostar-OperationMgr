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
    public class ProductIssuesController : Controller
    {
        private NeshagostarContext db = new NeshagostarContext();

        // GET: Storage/ProductIssues
        public ActionResult Index()
        {
            return View(db.ProductIssues.ToList());
        }

        // GET: Storage/ProductIssues/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductIssue productIssue = db.ProductIssues.Find(id);
            if (productIssue == null)
            {
                return HttpNotFound();
            }
            return View(productIssue);
        }

        // GET: Storage/ProductIssues/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Storage/ProductIssues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title")] ProductIssue productIssue)
        {
            if (ModelState.IsValid)
            {
                productIssue.Id = Guid.NewGuid();
                db.ProductIssues.Add(productIssue);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productIssue);
        }

        // GET: Storage/ProductIssues/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductIssue productIssue = db.ProductIssues.Find(id);
            if (productIssue == null)
            {
                return HttpNotFound();
            }
            return View(productIssue);
        }

        // POST: Storage/ProductIssues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title")] ProductIssue productIssue)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productIssue).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productIssue);
        }

        // GET: Storage/ProductIssues/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductIssue productIssue = db.ProductIssues.Find(id);
            if (productIssue == null)
            {
                return HttpNotFound();
            }
            return View(productIssue);
        }

        // POST: Storage/ProductIssues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ProductIssue productIssue = db.ProductIssues.Find(id);
            db.ProductIssues.Remove(productIssue);
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

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

    public class ProductCategoriesController : Controller
    {
        private NeshagostarContext db = new NeshagostarContext();

        // GET: Commerce/ProductCategories
        public ActionResult Index()
        {
            return View("~/Areas/Commerce/Views/ProductsRelated/ProductCategories/Index.cshtml", db.ProductCategories.OrderBy(p => p.Code).ToList());
        }

        // GET: Commerce/ProductCategories/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategory productCategory = db.ProductCategories.Find(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            return View("~/Areas/Commerce/Views/ProductsRelated/ProductCategories/Details.cshtml", productCategory);
        }

        // GET: Commerce/ProductCategories/Create
        public ActionResult Create()
        {
            return View("~/Areas/Commerce/Views/ProductsRelated/ProductCategories/Create.cshtml");
        }

        // POST: Commerce/ProductCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                productCategory.Id = Guid.NewGuid();
                db.ProductCategories.Add(productCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("~/Areas/Commerce/Views/ProductsRelated/ProductCategories/Create.cshtml", productCategory);
        }

        // GET: Commerce/ProductCategories/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategory productCategory = db.ProductCategories.Find(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            return View("~/Areas/Commerce/Views/ProductsRelated/ProductCategories/Edit.cshtml", productCategory);
        }

        // POST: Commerce/ProductCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("~/Areas/Commerce/Views/ProductsRelated/ProductCategories/Edit.cshtml", productCategory);
        }

        // GET: Commerce/ProductCategories/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategory productCategory = db.ProductCategories.Find(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            return View("~/Areas/Commerce/Views/ProductsRelated/ProductCategories/Delete.cshtml", productCategory);
        }

        // POST: Commerce/ProductCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ProductCategory productCategory = db.ProductCategories.Find(id);
            db.ProductCategories.Remove(productCategory);
            db.SaveChanges();
            return RedirectToAction("Index", new { controller = "ProductCategories", area="Commerce" });
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

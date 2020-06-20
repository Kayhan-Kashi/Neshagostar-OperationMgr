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
    public class ProductGenresController : Controller
    {
        private NeshagostarContext db = new NeshagostarContext();

        // GET: Commerce/ProductGenres
        public ActionResult Index()
        {
            var productGenres = db.ProductGenres.Include(p => p.PipeDiameter).Include(p => p.PipeProfile).Include(p => p.ProductCategory).OrderBy(p => p.Code);
            return View(productGenres.ToList());
        }

        // GET: Commerce/ProductGenres/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductGenre productGenre = db.ProductGenres.Find(id);
            if (productGenre == null)
            {
                return HttpNotFound();
            }
            return View(productGenre);
        }

        // GET: Commerce/ProductGenres/Create
        public ActionResult Create()
        {
            ViewBag.PipeDiameterId = new SelectList(db.PipeDiameters, "Id", "Size");
            ViewBag.PipeProfileId = new SelectList(db.PipeProfiles, "Id", "Name");
            ViewBag.ProductCategoryId = new SelectList(db.ProductCategories, "Id", "Name");
            return View();
        }

        // POST: Commerce/ProductGenres/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductGenre productGenre)
        {
            if (ModelState.IsValid)
            {
                productGenre.Id = Guid.NewGuid();
                productGenre.Code = GenerateCode(productGenre);
                db.ProductGenres.Add(productGenre);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PipeDiameterId = new SelectList(db.PipeDiameters, "Id", "Size", productGenre.PipeDiameterId);
            ViewBag.PipeProfileId = new SelectList(db.PipeProfiles, "Id", "Name", productGenre.PipeProfileId);
            ViewBag.ProductCategoryId = new SelectList(db.ProductCategories, "Id", "Name", productGenre.ProductCategoryId);
            return View(productGenre);
        }

        // GET: Commerce/ProductGenres/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductGenre productGenre = db.ProductGenres.Find(id);
            if (productGenre == null)
            {
                return HttpNotFound();
            }
            ViewBag.PipeDiameterId = new SelectList(db.PipeDiameters, "Id", "Size", productGenre.PipeDiameterId);
            ViewBag.PipeProfileId = new SelectList(db.PipeProfiles, "Id", "Name", productGenre.PipeProfileId);
            ViewBag.ProductCategoryId = new SelectList(db.ProductCategories, "Id", "Name", productGenre.ProductCategoryId);
            return View(productGenre);
        }

        // POST: Commerce/ProductGenres/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( ProductGenre productGenre)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productGenre).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PipeDiameterId = new SelectList(db.PipeDiameters, "Id", "Size", productGenre.PipeDiameterId);
            ViewBag.PipeProfileId = new SelectList(db.PipeProfiles, "Id", "Name", productGenre.PipeProfileId);
            ViewBag.ProductCategoryId = new SelectList(db.ProductCategories, "Id", "Name", productGenre.ProductCategoryId);
            return View(productGenre);
        }

        // GET: Commerce/ProductGenres/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductGenre productGenre = db.ProductGenres.Find(id);
            if (productGenre == null)
            {
                return HttpNotFound();
            }
            return View(productGenre);
        }

        // POST: Commerce/ProductGenres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ProductGenre productGenre = db.ProductGenres.Find(id);
            db.ProductGenres.Remove(productGenre);
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

        private string GenerateCode(ProductGenre product)
        {
            string productCategoryCode = db.ProductCategories.Find(product.ProductCategoryId).Code;
            string profileCode = db.PipeProfiles.Find(product.PipeProfileId).Code;
            string diameterCode = db.PipeDiameters.Find(product.PipeDiameterId).Code;
            return productCategoryCode + profileCode + diameterCode;
        }
    }
}

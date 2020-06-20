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
    
    public class ProductsController : Controller
    {
        private NeshagostarContext db = new NeshagostarContext();

        // GET: Commerce/Products
        public ActionResult Index()
        {
            var products = db.Products.OrderBy(p => p.Code).Include(p => p.ProductGenre.PipeDiameter).Include(p => p.ProductGenre.PipeProfile).Include(p => p.ProductGenre.ProductCategory).Include(p => p.RingStiffness);
            return View("~/Areas/Commerce/Views/ProductsRelated/Products/Index.cshtml", products.ToList());
        }

        // GET: Commerce/Products/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View("~/Areas/Commerce/Views/ProductsRelated/Products/Details.cshtml",product);
        }

        // GET: Commerce/Products/Create
        public ActionResult Create()
        {
            ViewBag.ProductGenreId = new SelectList(db.ProductGenres.OrderBy(p => p.Code), "Id", "Name");
            ViewBag.RingStiffnessId = new SelectList(db.RingStiffnesses, "Id", "Description");
            ViewBag.CouplingId = new SelectList(db.Couplings, "Id", "HasCoupling");
            return View("~/Areas/Commerce/Views/ProductsRelated/Products/Create.cshtml");
        }

        // POST: Commerce/Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                product.Id = Guid.NewGuid();
                product.Code = GenerateCode(product);
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductGenres = new SelectList(db.ProductGenres.OrderBy(p => p.Code), "Id", "Name");
            ViewBag.RingStiffnessId = new SelectList(db.RingStiffnesses, "Id", "Description");
            ViewBag.CouplingId = new SelectList(db.Couplings, "Id", "HasCoupling");

            return View("~/Areas/Commerce/Views/ProductsRelated/Products/Create.cshtml");
        }

        // GET: Commerce/Products/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductGenreId = new SelectList(db.ProductGenres.OrderBy(p => p.Code), "Id", "Name");
            ViewBag.RingStiffnessId = new SelectList(db.RingStiffnesses, "Id", "Description");
            ViewBag.CouplingId = new SelectList(db.Couplings, "Id", "HasCoupling");

            return View("~/Areas/Commerce/Views/ProductsRelated/Products/Edit.cshtml",product);
        }

        // POST: Commerce/Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Product product)
        {
            if (ModelState.IsValid)
            {
                var fetchedproduct = db.Products.Find(product.Id);
                fetchedproduct.ProductGenreId = product.ProductGenreId;
                fetchedproduct.RingStiffnessId = product.RingStiffnessId;
                fetchedproduct.CouplingId = product.CouplingId;
                fetchedproduct.Title = product.Title;
                fetchedproduct.Code = product.Code;
                db.Entry(fetchedproduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductGenreId = new SelectList(db.ProductGenres.OrderBy(p => p.Code), "Id", "Name");
            ViewBag.RingStiffnessId = new SelectList(db.RingStiffnesses, "Id", "Description");
            ViewBag.CouplingId = new SelectList(db.Couplings, "Id", "HasCoupling");
            return View("~/Areas/Commerce/Views/ProductsRelated/Products/Edit.cshtml",product);
        }

        // GET: Commerce/Products/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View("~/Areas/Commerce/Views/ProductsRelated/Products/Delete.cshtml",product);
        }

        // POST: Commerce/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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

        [HttpGet]
        public JsonResult GetProducts()
        {
            var products = db.Products.OrderBy(p => p.Code).Select(p => new { id = p.Id, name = p.Title, category = p.ProductGenre.ProductCategory.Name });
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        private string GenerateCode(Product product)
        {
            string productGenreCode = db.ProductGenres.Find(product.ProductGenreId).Code;
            string ringStiffnessCode = db.RingStiffnesses.Find(product.RingStiffnessId).Code;
            string couplingCode = db.Couplings.Find(product.CouplingId).Code;
            return productGenreCode + ringStiffnessCode + couplingCode;
        }

    }
}

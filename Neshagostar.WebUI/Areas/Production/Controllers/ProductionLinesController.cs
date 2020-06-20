using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Neshagostar.DAL.DataModel;
using Neshagostar.DAL.DataModel.ProductionRelated;

namespace Neshagostar.WebUI.Areas.Production.Controllers
{
    public class ProductionLinesController : Controller
    {
        private NeshagostarContext db = new NeshagostarContext();

        // GET: Production/ProductionLines
        public ActionResult Index()
        {
            var productionLines = db.ProductionLines.Include(p => p.Device).Include(p => p.ProductGenre).OrderBy(p => p.Code);
            return View(productionLines.ToList());
        }

        // GET: Production/ProductionLines/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductionLine productionLine = db.ProductionLines.Find(id);
            if (productionLine == null)
            {
                return HttpNotFound();
            }
            return View(productionLine);
        }

        // GET: Production/ProductionLines/Create
        public ActionResult Create()
        {
            ViewBag.DeviceId = new SelectList(db.Devices, "Id", "Name");
            ViewBag.ProductGenreId = new SelectList(db.ProductGenres, "Id", "Name");
            return View();
        }

        // POST: Production/ProductionLines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DeviceId,ProductGenreId,Description,Comments,Code")] ProductionLine productionLine)
        {
            if (ModelState.IsValid)
            {
                productionLine.Id = Guid.NewGuid();
                db.ProductionLines.Add(productionLine);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DeviceId = new SelectList(db.Devices, "Id", "Name", productionLine.DeviceId);
            ViewBag.ProductGenreId = new SelectList(db.ProductGenres, "Id", "Name", productionLine.ProductGenreId);
            return View(productionLine);
        }

        // GET: Production/ProductionLines/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductionLine productionLine = db.ProductionLines.Find(id);
            if (productionLine == null)
            {
                return HttpNotFound();
            }
            ViewBag.DeviceId = new SelectList(db.Devices, "Id", "Name", productionLine.DeviceId);
            ViewBag.ProductGenreId = new SelectList(db.ProductGenres, "Id", "Name", productionLine.ProductGenreId);
            return View(productionLine);
        }

        // POST: Production/ProductionLines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DeviceId,ProductGenreId,Description,Comments,Code")] ProductionLine productionLine)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productionLine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DeviceId = new SelectList(db.Devices, "Id", "Name", productionLine.DeviceId);
            ViewBag.ProductGenreId = new SelectList(db.ProductGenres, "Id", "Name", productionLine.ProductGenreId);
            return View(productionLine);
        }

        // GET: Production/ProductionLines/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductionLine productionLine = db.ProductionLines.Find(id);
            if (productionLine == null)
            {
                return HttpNotFound();
            }
            return View(productionLine);
        }

        // POST: Production/ProductionLines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ProductionLine productionLine = db.ProductionLines.Find(id);
            db.ProductionLines.Remove(productionLine);
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

        public JsonResult GetRelatedProducts(Guid productionLineId)
        {
            var products = db.ProductionLines.Include("ProductGenre.Products").FirstOrDefault(p => p.Id.Equals(productionLineId)).ProductGenre.Products;
            return Json(products.OrderBy(p => p.Code).Select(p => new { Id = p.Id, Title = p.Title }), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetProductionLines(Guid deviceId)
        {
            var list = db.ProductionLines.Where(d => d.DeviceId == deviceId).OrderBy(s => s.Code).ToList();
            return Json(list , JsonRequestBehavior.AllowGet);
        }
    }
}

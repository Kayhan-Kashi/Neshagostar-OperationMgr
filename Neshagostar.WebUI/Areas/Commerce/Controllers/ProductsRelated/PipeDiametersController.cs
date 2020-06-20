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

    public class PipeDiametersController : Controller
    {
        private NeshagostarContext db = new NeshagostarContext();

        // GET: Commerce/PipeDiameters
        public ActionResult Index()
        {
            return View("~/Areas/Commerce/Views/ProductsRelated/PipeDiameters/Index.cshtml", db.PipeDiameters.OrderBy(p => p.Code).ToList());
        }

        // GET: Commerce/PipeDiameters/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PipeDiameter pipeDiameter = db.PipeDiameters.Find(id);
            if (pipeDiameter == null)
            {
                return HttpNotFound();
            }
            return View("~/Areas/Commerce/Views/ProductsRelated/PipeDiameters/Details.cshtml",pipeDiameter);
        }

        // GET: Commerce/PipeDiameters/Create
        public ActionResult Create()
        {
            return View("~/Areas/Commerce/Views/ProductsRelated/PipeDiameters/Create.cshtml");
        }

        // POST: Commerce/PipeDiameters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PipeDiameter pipeDiameter)
        {
            if (ModelState.IsValid)
            {
                pipeDiameter.Id = Guid.NewGuid();
                db.PipeDiameters.Add(pipeDiameter);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("~/Areas/Commerce/Views/ProductsRelated/PipeDiameters/Create.cshtml", pipeDiameter);
        }

        // GET: Commerce/PipeDiameters/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PipeDiameter pipeDiameter = db.PipeDiameters.Find(id);
            if (pipeDiameter == null)
            {
                return HttpNotFound();
            }
            return View("~/Areas/Commerce/Views/ProductsRelated/PipeDiameters/Edit.cshtml", pipeDiameter);
        }

        // POST: Commerce/PipeDiameters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( PipeDiameter pipeDiameter)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pipeDiameter).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("~/Areas/Commerce/Views/ProductsRelated/PipeDiameters/Edit.cshtml", pipeDiameter);
        }

        // GET: Commerce/PipeDiameters/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PipeDiameter pipeDiameter = db.PipeDiameters.Find(id);
            if (pipeDiameter == null)
            {
                return HttpNotFound();
            }
            return View("~/Areas/Commerce/Views/ProductsRelated/PipeDiameters/Delete.cshtml", pipeDiameter);
        }

        // POST: Commerce/PipeDiameters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            PipeDiameter pipeDiameter = db.PipeDiameters.Find(id);
            db.PipeDiameters.Remove(pipeDiameter);
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

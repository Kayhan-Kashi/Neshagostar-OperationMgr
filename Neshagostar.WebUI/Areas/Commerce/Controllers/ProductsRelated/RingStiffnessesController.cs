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
    [Authorize(Roles = "CEO, commerce-manager")]
    public class RingStiffnessesController : Controller
    {
        private NeshagostarContext db = new NeshagostarContext();

        // GET: Commerce/RingStiffnesses
        public ActionResult Index()
        {
            return View("~/Areas/Commerce/Views/ProductsRelated/RingStiffnesses/Index.cshtml", db.RingStiffnesses.OrderBy(p => p.Code).ToList());
        }

        // GET: Commerce/RingStiffnesses/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RingStiffness ringStiffness = db.RingStiffnesses.Find(id);
            if (ringStiffness == null)
            {
                return HttpNotFound();
            }
            return View("~/Areas/Commerce/Views/ProductsRelated/RingStiffnesses/Detail.cshtml", ringStiffness);
        }

        // GET: Commerce/RingStiffnesses/Create
        public ActionResult Create()
        {
            return View("~/Areas/Commerce/Views/ProductsRelated/RingStiffnesses/Create.cshtml");
        }

        // POST: Commerce/RingStiffnesses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( RingStiffness ringStiffness)
        {
            if (ModelState.IsValid)
            {
                ringStiffness.Id = Guid.NewGuid();
                db.RingStiffnesses.Add(ringStiffness);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("~/Areas/Commerce/Views/ProductsRelated/RingStiffnesses/Create.cshtml", ringStiffness);
        }

        // GET: Commerce/RingStiffnesses/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RingStiffness ringStiffness = db.RingStiffnesses.Find(id);
            if (ringStiffness == null)
            {
                return HttpNotFound();
            }
            return View("~/Areas/Commerce/Views/ProductsRelated/RingStiffnesses/Edit.cshtml", ringStiffness);
        }

        // POST: Commerce/RingStiffnesses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RingStiffness ringStiffness)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ringStiffness).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("~/Areas/Commerce/Views/ProductsRelated/RingStiffnesses/Edit.cshtml", ringStiffness);
        }

        // GET: Commerce/RingStiffnesses/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RingStiffness ringStiffness = db.RingStiffnesses.Find(id);
            if (ringStiffness == null)
            {
                return HttpNotFound();
            }
            return View("~/Areas/Commerce/Views/ProductsRelated/RingStiffnesses/Delete.cshtml", ringStiffness);
        }

        // POST: Commerce/RingStiffnesses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            RingStiffness ringStiffness = db.RingStiffnesses.Find(id);
            db.RingStiffnesses.Remove(ringStiffness);
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

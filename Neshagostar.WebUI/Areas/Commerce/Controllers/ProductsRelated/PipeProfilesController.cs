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

    public class PipeProfilesController : Controller
    {
        private NeshagostarContext db = new NeshagostarContext();

        // GET: Commerce/PipeProfiles
        public ActionResult Index()
        {
            return View("~/Areas/Commerce/Views/ProductsRelated/PipeProfiles/Index.cshtml", db.PipeProfiles.OrderBy(p => p.Code).ToList());
        }

        // GET: Commerce/PipeProfiles/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PipeProfile pipeProfile = db.PipeProfiles.Find(id);
            if (pipeProfile == null)
            {
                return HttpNotFound();
            }
            return View("~/Areas/Commerce/Views/ProductsRelated/PipeProfiles/Details.cshtml", pipeProfile);
        }

        // GET: Commerce/PipeProfiles/Create
        public ActionResult Create()
        {
            return View("~/Areas/Commerce/Views/ProductsRelated/PipeProfiles/Create.cshtml");
        }

        // POST: Commerce/PipeProfiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( PipeProfile pipeProfile)
        {
            if (ModelState.IsValid)
            {
                pipeProfile.Id = Guid.NewGuid();
                db.PipeProfiles.Add(pipeProfile);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("~/Areas/Commerce/Views/ProductsRelated/PipeProfiles/Create.cshtml", pipeProfile);
        }

        // GET: Commerce/PipeProfiles/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PipeProfile pipeProfile = db.PipeProfiles.Find(id);
            if (pipeProfile == null)
            {
                return HttpNotFound();
            }
            return View("~/Areas/Commerce/Views/ProductsRelated/PipeProfiles/Edit.cshtml", pipeProfile);
        }

        // POST: Commerce/PipeProfiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( PipeProfile pipeProfile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pipeProfile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("~/Areas/Commerce/Views/ProductsRelated/PipeProfiles/Edit.cshtml", pipeProfile);
        }

        // GET: Commerce/PipeProfiles/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PipeProfile pipeProfile = db.PipeProfiles.Find(id);
            if (pipeProfile == null)
            {
                return HttpNotFound();
            }
            return View("~/Areas/Commerce/Views/ProductsRelated/PipeProfiles/Delete.cshtml", pipeProfile);
        }

        // POST: Commerce/PipeProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            PipeProfile pipeProfile = db.PipeProfiles.Find(id);
            db.PipeProfiles.Remove(pipeProfile);
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

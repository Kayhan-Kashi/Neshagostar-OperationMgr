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
    public class DevicesController : Controller
    {
        private NeshagostarContext db = new NeshagostarContext();

        // GET: Production/Devices
        public ActionResult Index()
        {
            var devices = db.Devices.Include(d => d.Saloon);
            return View(devices.ToList());
        }

        // GET: Production/Devices/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device device = db.Devices.Find(id);
            if (device == null)
            {
                return HttpNotFound();
            }
            return View(device);
        }

        // GET: Production/Devices/Create
        public ActionResult Create()
        {
            ViewBag.SaloonId = new SelectList(db.Saloons, "Id", "Name");
            return View();
        }

        // POST: Production/Devices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SaloonId,Name,Comments,Code")] Device device)
        {
            if (ModelState.IsValid)
            {
                device.Id = Guid.NewGuid();
                db.Devices.Add(device);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SaloonId = new SelectList(db.Saloons, "Id", "Name", device.SaloonId);
            return View(device);
        }

        // GET: Production/Devices/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device device = db.Devices.Find(id);
            if (device == null)
            {
                return HttpNotFound();
            }
            ViewBag.SaloonId = new SelectList(db.Saloons, "Id", "Name", device.SaloonId);
            return View(device);
        }

        // POST: Production/Devices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SaloonId,Name,Comments,Code")] Device device)
        {
            if (ModelState.IsValid)
            {
                db.Entry(device).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SaloonId = new SelectList(db.Saloons, "Id", "Name", device.SaloonId);
            return View(device);
        }

        // GET: Production/Devices/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device device = db.Devices.Find(id);
            if (device == null)
            {
                return HttpNotFound();
            }
            return View(device);
        }

        // POST: Production/Devices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Device device = db.Devices.Find(id);
            db.Devices.Remove(device);
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
        public JsonResult GetDevices(Guid saloonId)
        {
            return Json(db.Devices.Where(s => s.SaloonId == saloonId).OrderBy(d => d.Code).ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}

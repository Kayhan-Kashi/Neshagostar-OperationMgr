using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Neshagostar.DAL.DataModel;
using Neshagostar.DAL.DataModel.PersonnelRelated;

namespace Neshagostar.WebUI.Areas.PersonnelManagement.Controllers
{
    public class DepartmentsController : Controller
    {
        private NeshagostarContext db = new NeshagostarContext();

        // GET: PersonnelManagement/Departments
        public ActionResult Index()
        {
            return View("~/Areas/PersonnelManagement/Views/Departments/Index.cshtml", db.Departments.ToList());
        }

        // GET: PersonnelManagement/Departments/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View("~/Areas/PersonnelManagement/Views/Departments/Details.cshtml", department);
        }

        // GET: PersonnelManagement/Departments/Create
        public ActionResult Create()
        {
            return View("~/Areas/PersonnelManagement/Views/Departments/Create.cshtml");
        }

        // POST: PersonnelManagement/Departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Department department)
        {
            if (ModelState.IsValid)
            {
                department.Id = Guid.NewGuid();
                db.Departments.Add(department);
                db.SaveChanges();
                return RedirectToAction("Index", new { controller="Departments", area="PersonnelManagement" });
            }

            return View("~/Areas/PersonnelManagement/Views/Departments/Create.cshtml", department);
        }

        // GET: PersonnelManagement/Departments/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(Url.Action("~/Areas/PersonnelManagement/Views/Departments/Edit.cshtml", department));
        }

        // POST: PersonnelManagement/Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Department department)
        {
            if (ModelState.IsValid)
            {
                db.Entry(department).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { controller = "Departments", area = "PersonnelManagement" });
            }
            return View("~/Areas/PersonnelManagement/Views/Departments/Edit.cshtml", department);
        }

        // GET: PersonnelManagement/Departments/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(Url.Action("Delete", new { controller = "Departments", area = "PersonnelManagement" }), department);
        }

        // POST: PersonnelManagement/Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Department department = db.Departments.Find(id);
            db.Departments.Remove(department);
            db.SaveChanges();
            return RedirectToAction("Index", new { controller = "Departments", area = "PersonnelManagement" });
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

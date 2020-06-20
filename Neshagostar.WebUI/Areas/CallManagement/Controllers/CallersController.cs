using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Neshagostar.DAL.DataModel;
using Neshagostar.DAL.DataModel.CallManagement;
using Neshagostar.WebUI.Infrastructure.Filters;

namespace Neshagostar.WebUI.Areas.CallManagement.Controllers
{
    [Authorize(Roles = "secretary, CEO")]
    public class CallersController : Controller
    {
        private NeshagostarContext db = new NeshagostarContext();
        const int PAGESIZE = 40;
        // GET: CallManagement/Callers
        public ActionResult Index(int page = 1)
        {
            var result = db.Callers.OrderBy(c => c.Name).Take(PAGESIZE).ToList();
            ViewBag.PageNumber = Math.Ceiling(decimal.Parse(db.Callers.ToList().Count.ToString()) / (decimal)PAGESIZE);
            ViewBag.Page = 1;
            return View(result);
        }

        // GET: CallManagement/Callers/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Caller caller = db.Callers.Find(id);
            if (caller == null)
            {
                return HttpNotFound();
            }
            return View(caller);
        }

        // GET: CallManagement/Callers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CallManagement/Callers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLogger(ActivityName = "ثبت", ModelNameBeingOperated = "تماس گیرنده")]
        public ActionResult Create([Bind(Include = "Id,Name,Tel,Comments")] Caller caller)
        {
            if (ModelState.IsValid)
            {
                caller.Id = Guid.NewGuid();
                db.Callers.Add(caller);
                db.SaveChanges();
                ViewBag.ModelOperatedId = caller.Id.ToString();
                return RedirectToAction("Index");
            }

            return View(caller);
        }

        // GET: CallManagement/Callers/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Caller caller = db.Callers.Find(id);
            if (caller == null)
            {
                return HttpNotFound();
            }
            return View(caller);
        }

        // POST: CallManagement/Callers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ActivityLogger(ActivityName = "ثبت", ModelNameBeingOperated = "تماس گیرنده")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Tel,Comments")] Caller caller)
        {
            if (ModelState.IsValid)
            {
                db.Entry(caller).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.ModelOperatedId = caller.Id.ToString();
                return RedirectToAction("Index");
            }
            return View(caller);
        }

        // GET: CallManagement/Callers/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Caller caller = db.Callers.Find(id);
            if (caller == null)
            {
                return HttpNotFound();
            }
            return View(caller);
        }

        // POST: CallManagement/Callers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ActivityLogger(ActivityName = "ثبت", ModelNameBeingOperated = "تماس گیرنده")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Caller caller = db.Callers.Find(id);
            db.Callers.Remove(caller);
            db.SaveChanges();
            ViewBag.ModelOperatedId = caller.Id.ToString();
            return RedirectToAction("Index");
        }

        public JsonResult Search(string term)
        {
            var result = Json(db.Callers.Where(c => c.Name.Contains(term)).Select(ca => ca.Name).ToList(), JsonRequestBehavior.AllowGet);
            return result;
        }

        public JsonResult GetTel(string name)
        {
            var caller = db.Callers.Where(c => c.Name == name).FirstOrDefault();
            return Json(new { tel= caller.Tel , comments= caller.Comments}, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FilterCallers(string callerName, string tel, int page = 1)
        {      
            IQueryable<Caller> callers = db.Callers.OrderBy(c => c.Name);
            if (callerName != "")
            {
                callers = callers.Where(r => r.Name.Contains(callerName));
            }

            if (tel != "")
            {
                callers = callers.Where(r => r.Tel.Contains(tel));
            }

            var callersList = callers.ToList();
            var pageNumbers = Math.Ceiling(callersList.Count / (decimal)PAGESIZE);
            callersList = callersList.Skip((page - 1) * PAGESIZE).Take(PAGESIZE).ToList();

            if (page > pageNumbers)
            {
                page = Int32.Parse((pageNumbers).ToString());
            }

            var results = callersList.Select(r => new
            {
                callerName = r.Name,    
                id = r.Id,
                tel= r.Tel,
                page = page,
                pageNumbers = pageNumbers,
                pageSize = PAGESIZE
            }).ToList();

            ViewBag.PageNumbers = pageNumbers;

            ViewBag.Page = page;
            ViewBag.PageSize = PAGESIZE;
            //results = results.Skip((page - 1) * PAGESIZE).Take(PAGESIZE).ToList();



            return Json(results, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult FilterTel(string tel, int page = 1)
        //{
        //    var allCallers = db.Callers.Where(c => c.Tel == tel).ToList();
        //    var pageNumbers = Math.Ceiling(allCallers.Count / (decimal)PAGESIZE);
        //    List<Caller> callers = db.Callers.Where(p => p.Tel == tel).ToList();
        //    var results = allCallers.Select(r => new
        //    {
        //        callerName = r.Name,
        //        id = r.Id,
        //        tel = r.Tel,
        //        page = page,
        //        pageNumbers = pageNumbers,
        //        pageSize = PAGESIZE
        //    }).ToList();
        //    return Json(callers, JsonRequestBehavior.AllowGet);
        //}
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

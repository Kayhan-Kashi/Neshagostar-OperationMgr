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
    public class DailyProductionReportsController : Controller
    {
        private NeshagostarContext db = new NeshagostarContext();

        // GET: Production/DailyProductionReports
        public ActionResult Index()
        {
            var dailyProductionReports = db.DailyProductionReports.Include(d => d.Product).Include(d => d.ProductionLine).OrderByDescending(p => p.DateOfProduction).ToList();
            return View(dailyProductionReports.ToList());
        }

        // GET: Production/DailyProductionReports/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyProductionReport dailyProductionReport = db.DailyProductionReports.Find(id);
            if (dailyProductionReport == null)
            {
                return HttpNotFound();
            }
            return View(dailyProductionReport);
        }

        // GET: Production/DailyProductionReports/Create
        public ActionResult Create()
        {
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Title");
            ViewBag.ProductionLineId = new SelectList(db.ProductionLines.OrderBy(p => p.Code), "Id", "Description");
            return View();
        }

        // POST: Production/DailyProductionReports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProductionLineId,ProductId,DateOfProduction,DateOfReport,AmountOfAcceptedProduct,AmountOfUnacceptedProduct,WeightInUnit,OrdinaryMaterialConsumedAmount,GranuleMaterialConsumedAmount,BlackMasterbatchConsumedAmount,YellowMasterbatchConsumedAmount,RecycledConsumedAmount,AllMaterialsConsumedAmountInKilo,WastageProducedInKilo")] DailyProductionReport dailyProductionReport)
        {
            if (ModelState.IsValid)
            {
                dailyProductionReport.Id = Guid.NewGuid();
                dailyProductionReport.DateOfProduction = FormatDate(dailyProductionReport.DateOfProduction);
                dailyProductionReport.DateOfReport = FormatDate(dailyProductionReport.DateOfReport);
                db.DailyProductionReports.Add(dailyProductionReport);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductId = new SelectList(db.Products, "Id", "Title", dailyProductionReport.ProductId);
            ViewBag.ProductionLineId = new SelectList(db.ProductionLines, "Id", "Description", dailyProductionReport.ProductionLineId);
            return View(dailyProductionReport);
        }

        // GET: Production/DailyProductionReports/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyProductionReport dailyProductionReport = db.DailyProductionReports.Find(id);
            if (dailyProductionReport == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Title", dailyProductionReport.ProductId);
            ViewBag.ProductionLineId = new SelectList(db.ProductionLines, "Id", "Description", dailyProductionReport.ProductionLineId);
            return View(dailyProductionReport);
        }

        // POST: Production/DailyProductionReports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProductionLineId,ProductId,DateOfProduction,DateOfReport,AmountOfAcceptedProduct,AmountOfUnacceptedProduct,WeightInUnit,OrdinaryMaterialConsumedAmount,GranuleMaterialConsumedAmount,BlackMasterbatchConsumedAmount,YellowMasterbatchConsumedAmount,RecycledConsumedAmount,AllMaterialsConsumedAmountInKilo,WastageProducedInKilo")] DailyProductionReport dailyProductionReport)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dailyProductionReport).State = EntityState.Modified;
                dailyProductionReport.DateOfProduction = FormatDate(dailyProductionReport.DateOfProduction);
                dailyProductionReport.DateOfReport = FormatDate(dailyProductionReport.DateOfReport);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Title", dailyProductionReport.ProductId);
            ViewBag.ProductionLineId = new SelectList(db.ProductionLines, "Id", "Description", dailyProductionReport.ProductionLineId);
            return View(dailyProductionReport);
        }

        // GET: Production/DailyProductionReports/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyProductionReport dailyProductionReport = db.DailyProductionReports.Find(id);
            if (dailyProductionReport == null)
            {
                return HttpNotFound();
            }
            return View(dailyProductionReport);
        }

        // POST: Production/DailyProductionReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            DailyProductionReport dailyProductionReport = db.DailyProductionReports.Find(id);
            db.DailyProductionReports.Remove(dailyProductionReport);
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

        public string FormatDate(string date)
        {
            string[] strArray = date.Split('/');
            var dateFormatted = new PersianDateTime(Int32.Parse(strArray[0]), Int32.Parse(strArray[1]), Int32.Parse(strArray[2])).ToString().Substring(0, 10);
            return dateFormatted;

        }
    }
}

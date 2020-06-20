using Neshagostar.DAL.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Neshagostar.WebUI.Areas.Managament.Controllers
{
    public class ChartsController : Controller
    {
        NeshagostarContext context = new NeshagostarContext();

        public ActionResult WastageChart()
        {
            return View();
        }

        [HttpPost]
        // GET: Production/Charts
        public JsonResult Wastage(string duration)
        {
            var dailyProductionReports = context.DailyProductionReports.Include("Product").Include("ProductionLine").OrderBy(p => p.DateOfProduction).Select(d => new { Date = d.DateOfProduction, Amount = d.WastageProducedInKilo }).ToList();
            return Json(dailyProductionReports, JsonRequestBehavior.AllowGet);

        }
    }
}
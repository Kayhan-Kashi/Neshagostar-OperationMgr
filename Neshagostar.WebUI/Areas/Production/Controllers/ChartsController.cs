using Neshagostar.DAL.DataModel;
using Neshagostar.DAL.DataModel.ProductionRelated;
using Neshagostar.WebUI.Areas.Production.Models.ChartsRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Neshagostar.WebUI.Areas.Production.Controllers
{
    public class ChartsController : Controller
    {

        NeshagostarContext context = new NeshagostarContext();
        string[] monthNameArray = { "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند" };
        public ActionResult WastageChart()
        {
            ViewBag.Years = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "همه سالها",
                    Value = ""
                }
            };

            ViewBag.Months = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "همه ماهها",
                    Value = ""
                }
            };

            ViewBag.ChartTypes = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "نمودار میله ای",
                    Value = "columnChart"
                },
                 new SelectListItem
                {
                    Text = "نمودار خطی",
                    Value = "lineChart"
                }
            };

            
            return View();
        }

        [HttpPost]
        // GET: Production/Charts
        public JsonResult Wastage(string duration)
        {
            var dailyProductionReports = context.DailyProductionReports.Include("Product").Include("ProductionLine").OrderBy(p => p.DateOfProduction).Select(d => new { Date=d.DateOfProduction, WastageAmount = d.WastageProducedInKilo , ConsumedAmount=d.AllMaterialsConsumedAmountInKilo}).ToList();
            return Json(dailyProductionReports, JsonRequestBehavior.AllowGet);

        }

        public JsonResult DrawWastage(string year, string month)
        {
            month = FormatNumber(month);
            //  It means show wastage amounts grouped by years for all of the years of production
            if (year == "" || year == null)
            {
                return GetWastageYearChart("");
            }
            //  It means we do have a year to show
            else
            {

                // It means show wastage amounts grouped by each month for the selected year
                if (month == "" || month == null)
                {
                   return GetWastageYearChart(year);
                }
                //  It means we do have a year and a month to show so we show the wastage amounts grouped by each day in a selected month of the year
                else
                {
                    return GetWastageMonthChartGroupdByDay(year, month);
                }
            }           
        }

        private JsonResult GetWastageYearChart(string year)
        {
            var dailyProductionReports = context.DailyProductionReports.Include("Product").Include("ProductionLine");

            if (year == "" || year == null)
            {
              
                var dailyProductionReportsGroupedByYear = dailyProductionReports.GroupBy(d => d.DateOfProduction.Substring(0, 4)).ToList();
                var wastageProducedGroupedByYear = dailyProductionReportsGroupedByYear.Select(d => 
                new {
                    Date = d.Key,
                    WastageAmount = d.Sum(dg => dg.WastageProducedInKilo),
                    ConsumedAmount = d.Sum(dg => dg.AllMaterialsConsumedAmountInKilo),
                    MaxConsumed = dailyProductionReportsGroupedByYear.Max(r => r.Sum(dr => dr.AllMaterialsConsumedAmountInKilo)),
                    MaxWastageAmount = dailyProductionReportsGroupedByYear.Max(r => r.Sum(dr => dr.WastageProducedInKilo)),
                    MinConsumed = dailyProductionReportsGroupedByYear.Min(r => r.Sum(dr => dr.AllMaterialsConsumedAmountInKilo)),
                    MinWastage = dailyProductionReportsGroupedByYear.Min(r => r.Sum(dr => dr.WastageProducedInKilo)),
                    AverageConsumed = dailyProductionReportsGroupedByYear.Average(r => r.Sum(dr => dr.AllMaterialsConsumedAmountInKilo)),
                    AverageWastage = dailyProductionReportsGroupedByYear.Average(r => r.Sum(dr => dr.WastageProducedInKilo)),
                    TotalMaterialConsumed = dailyProductionReportsGroupedByYear.Sum(r => r.Sum(dr => dr.AllMaterialsConsumedAmountInKilo)),
                    TotalWastage = dailyProductionReportsGroupedByYear.Sum(r => r.Sum(dr => dr.WastageProducedInKilo))
                }).ToList();
                return Json(wastageProducedGroupedByYear, JsonRequestBehavior.AllowGet);
            }
           else
            {
                var dailyProductionReportsInSelecedYear = dailyProductionReports.Where(d => d.DateOfProduction.Substring(0, 4) == year);
                var dailyProductionReportsGroupedByMonth = dailyProductionReportsInSelecedYear.GroupBy(d => d.DateOfProduction.Substring(5, 2)).OrderBy(d => d.Key).ToList();
                
                var wastageProducedGroupedByMonth = dailyProductionReportsGroupedByMonth.Select(d => 
                new {
                    Date = monthNameArray[int.Parse(d.Key)],
                    WastageAmount = d.Sum(dg => dg.WastageProducedInKilo),
                    ConsumedAmount = d.Sum(dg => dg.AllMaterialsConsumedAmountInKilo),
                    MaxConsumed = dailyProductionReportsGroupedByMonth.Max(r => r.Sum(dr => dr.AllMaterialsConsumedAmountInKilo)),
                    MaxWastageAmount = dailyProductionReportsGroupedByMonth.Max(r => r.Sum(dr => dr.WastageProducedInKilo)),
                    MinConsumed = dailyProductionReportsGroupedByMonth.Min(r => r.Sum(dr => dr.AllMaterialsConsumedAmountInKilo)),
                    MinWastage = dailyProductionReportsGroupedByMonth.Min(r => r.Sum(dr => dr.WastageProducedInKilo)),
                    AverageConsumed = dailyProductionReportsGroupedByMonth.Average(r => r.Sum(dr => dr.AllMaterialsConsumedAmountInKilo)),
                    AverageWastage = dailyProductionReportsGroupedByMonth.Average(r => r.Sum(dr => dr.WastageProducedInKilo)),
                    TotalMaterialConsumed = dailyProductionReportsGroupedByMonth.Sum(r => r.Sum(dr => dr.AllMaterialsConsumedAmountInKilo)),
                    TotalWastage = dailyProductionReportsGroupedByMonth.Sum(r => r.Sum(dr => dr.WastageProducedInKilo))
                }).ToList();
                var vl = dailyProductionReportsGroupedByMonth.Max(d => d.Sum(dr => dr.AllMaterialsConsumedAmountInKilo));
                //wastageProducedGroupedByMonth.ElementAt(0).AverageConsumed = wastageProducedGroupedByMonth.Average(d => d.AverageConsumed);
                //wastageProducedGroupedByMonth.ElementAt(0).AverageConsumed = wastageProducedGroupedByMonth.Average(d => d.AverageConsumed);
                //wastageProducedGroupedByMonth.ElementAt(0).AverageConsumed = wastageProducedGroupedByMonth.Average(d => d.AverageConsumed);

                return Json(wastageProducedGroupedByMonth, JsonRequestBehavior.AllowGet);
            }          
        }

        private JsonResult GetWastageMonthChartGroupdByDay(string year, string month)
        {
            var dailyProductionReports = context.DailyProductionReports.Include("Product").Include("ProductionLine");
            var dailyProductionReportsInSelectedMonth = dailyProductionReports.Where(d => d.DateOfProduction.Substring(0, 4) == year).Where(d => d.DateOfProduction.Substring(5, 2) == month).ToList();
            var dailyProductionReportsGroupedByDays = dailyProductionReportsInSelectedMonth.GroupBy(d => d.DateOfProduction.Substring(8, 2)).ToList();
            var wastageProducedGroupedByMonth = dailyProductionReportsGroupedByDays
                .Select(d => 
                    new {
                        Date = d.Key,
                        WastageAmount = Math.Ceiling(d.Sum(dg => dg.WastageProducedInKilo)),
                        ConsumedAmount = Math.Ceiling(d.Sum(dg => dg.AllMaterialsConsumedAmountInKilo)),
                        MaxConsumed = Math.Ceiling(dailyProductionReportsGroupedByDays.Max(r => r.Sum(dr => dr.AllMaterialsConsumedAmountInKilo))),
                        MaxWastageAmount = Math.Ceiling(dailyProductionReportsGroupedByDays.Max(r => r.Sum(dr => dr.WastageProducedInKilo))),
                        MinConsumed = Math.Ceiling(dailyProductionReportsGroupedByDays.Min(r => r.Sum(dr => dr.AllMaterialsConsumedAmountInKilo))),
                        MinWastage = Math.Ceiling(dailyProductionReportsGroupedByDays.Min(r => r.Sum(dr => dr.WastageProducedInKilo))),
                        AverageConsumed = Math.Ceiling(dailyProductionReportsGroupedByDays.Average(r => r.Sum(dr => dr.AllMaterialsConsumedAmountInKilo))),
                        AverageWastage = Math.Ceiling(dailyProductionReportsGroupedByDays.Average(r => r.Sum(dr => dr.WastageProducedInKilo))),
                        TotalMaterialConsumed = Math.Ceiling(dailyProductionReportsGroupedByDays.Sum(r => r.Sum(dr => dr.AllMaterialsConsumedAmountInKilo))),
                        TotalWastage = Math.Ceiling(dailyProductionReportsGroupedByDays.Sum(r => r.Sum(dr => dr.WastageProducedInKilo)))
                    }).ToList();
            return Json(wastageProducedGroupedByMonth, JsonRequestBehavior.AllowGet);      
        }



        public JsonResult GetYearsOfProduction()
        {
            List<string> years = context.DailyProductionReports.Select(d => d.DateOfProduction.Substring(0,4)).ToList();
            List<string> uniqueYears = years.Distinct().ToList();
            return Json(uniqueYears, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMonthOfProduction(string year)
        {

            if(year != "" || year != null)
            {
                var months = context.DailyProductionReports.Where(d => d.DateOfProduction.Substring(0, 4) == year).Select(d => d.DateOfProduction.Substring(5, 2)).ToList();
                var uniqueMonths = months.Distinct().OrderBy(d => d).ToList();
               
                var uniqueMonthsNameOfProduction = uniqueMonths.Select(d => new { MonthNumber = int.Parse(d), MonthName = monthNameArray[int.Parse(d) - 1] }).ToList();
                
             
                return Json(uniqueMonthsNameOfProduction, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }



        }

        private string FormatNumber(string num)
        {
            string numFormatted = num.Length == 1 ? "0" + num : num;
            return numFormatted;
        }

        public JsonResult DrawWastages(string year, string month, string saloonId, string deviceId, string productionLineId)
        {
            month = FormatNumber(month);
            List<ChartsDTO> chartsData;
            //  It means show wastage amounts grouped by years for all of the years of production
            if (year == "" || year == null)
            {
                chartsData =  GetWastageYearList("", saloonId, deviceId, productionLineId);
            }
            //  It means we do have a year to show
            else
            {

                // It means show wastage amounts grouped by each month for the selected year
                if (month == "" || month == null)
                {
                    chartsData =  GetWastageYearList(year, saloonId, deviceId, productionLineId);
                }
                //  It means we do have a year and a month to show so we show the wastage amounts grouped by each day in a selected month of the year
                else
                {
                    chartsData =  GetWastageMonthListGroupdByDay(year, month, saloonId, deviceId, productionLineId);
                }
            }

            return Json(chartsData, JsonRequestBehavior.AllowGet);



        }

        private List<ChartsDTO> GetWastageYearList(string year, string saloonId, string deviceId, string productionLineId)
        {
            IQueryable<DailyProductionReport> dailyProductionReports = context.DailyProductionReports.Include("Product").Include("ProductionLine.Device.Saloon");

            if (saloonId != "" && saloonId != null)
            {
                var saloonIdConverted = Guid.Parse(saloonId);
                dailyProductionReports = dailyProductionReports.Where(d => d.ProductionLine.Device.SaloonId == saloonIdConverted);
               
            }

            if (deviceId != "" && deviceId != null)
            {
                var deviceIdConverted = Guid.Parse(deviceId);
                dailyProductionReports = dailyProductionReports.Where(d => d.ProductionLine.DeviceId == deviceIdConverted);
            }

            if (productionLineId != "" && productionLineId != null)
            {
                var productionLineIdConverted = Guid.Parse(productionLineId);
                dailyProductionReports = dailyProductionReports.Where(d => d.ProductionLineId == productionLineIdConverted);
            }
        

            if (year == "" || year == null)
            {

                var dailyProductionReportsGroupedByYear = dailyProductionReports.GroupBy(d => d.DateOfProduction.Substring(0, 4)).ToList();
                var wastageProducedGroupedByYear = dailyProductionReportsGroupedByYear.Select(d =>
                new ChartsDTO
                {
                    Date = d.Key,
                    WastageAmount = Math.Ceiling(d.Sum(dg => dg.WastageProducedInKilo)),
                    ConsumedAmount = Math.Ceiling(d.Sum(dg => dg.AllMaterialsConsumedAmountInKilo)),
                    MaxConsumed = Math.Ceiling(dailyProductionReportsGroupedByYear.Max(r => r.Sum(dr => dr.AllMaterialsConsumedAmountInKilo))),
                    MaxWastageAmount = Math.Ceiling(dailyProductionReportsGroupedByYear.Max(r => r.Sum(dr => dr.WastageProducedInKilo))),
                    MinConsumed = Math.Ceiling(dailyProductionReportsGroupedByYear.Min(r => r.Sum(dr => dr.AllMaterialsConsumedAmountInKilo))),
                    MinWastage = Math.Ceiling(dailyProductionReportsGroupedByYear.Min(r => r.Sum(dr => dr.WastageProducedInKilo))),
                    AverageConsumed = Math.Ceiling(dailyProductionReportsGroupedByYear.Average(r => r.Sum(dr => dr.AllMaterialsConsumedAmountInKilo))),
                    AverageWastage = Math.Ceiling(dailyProductionReportsGroupedByYear.Average(r => r.Sum(dr => dr.WastageProducedInKilo))),
                    TotalMaterialConsumed = Math.Ceiling(dailyProductionReportsGroupedByYear.Sum(r => r.Sum(dr => dr.AllMaterialsConsumedAmountInKilo))),
                    TotalWastage = Math.Ceiling(dailyProductionReportsGroupedByYear.Sum(r => r.Sum(dr => dr.WastageProducedInKilo)))
                }).ToList();
                return wastageProducedGroupedByYear.OrderBy(d => d.Date).ToList();
            }
            else
            {
                var dailyProductionReportsInSelecedYear = dailyProductionReports.Where(d => d.DateOfProduction.Substring(0, 4) == year);
                var dailyProductionReportsGroupedByMonth = dailyProductionReportsInSelecedYear.GroupBy(d => d.DateOfProduction.Substring(5, 2)).OrderBy(d => d.Key).ToList();

                var wastageProducedGroupedByMonth = dailyProductionReportsGroupedByMonth.Select(d =>
                new ChartsDTO
                {
                    Date = d.Key,
                    WastageAmount = Math.Ceiling(d.Sum(dg => dg.WastageProducedInKilo)),
                    ConsumedAmount = Math.Ceiling(d.Sum(dg => dg.AllMaterialsConsumedAmountInKilo)),
                    MaxConsumed = Math.Ceiling(dailyProductionReportsGroupedByMonth.Max(r => r.Sum(dr => dr.AllMaterialsConsumedAmountInKilo))),
                    MaxWastageAmount = Math.Ceiling(dailyProductionReportsGroupedByMonth.Max(r => r.Sum(dr => dr.WastageProducedInKilo))),
                    MinConsumed = Math.Ceiling(dailyProductionReportsGroupedByMonth.Min(r => r.Sum(dr => dr.AllMaterialsConsumedAmountInKilo))),
                    MinWastage = Math.Ceiling(dailyProductionReportsGroupedByMonth.Min(r => r.Sum(dr => dr.WastageProducedInKilo))),
                    AverageConsumed = Math.Ceiling(dailyProductionReportsGroupedByMonth.Average(r => r.Sum(dr => dr.AllMaterialsConsumedAmountInKilo))),
                    AverageWastage = Math.Ceiling(dailyProductionReportsGroupedByMonth.Average(r => r.Sum(dr => dr.WastageProducedInKilo))),
                    TotalMaterialConsumed = Math.Ceiling(dailyProductionReportsGroupedByMonth.Sum(r => r.Sum(dr => dr.AllMaterialsConsumedAmountInKilo))),
                    TotalWastage = Math.Ceiling(dailyProductionReportsGroupedByMonth.Sum(r => r.Sum(dr => dr.WastageProducedInKilo)))
                }).ToList();
                var vl = dailyProductionReportsGroupedByMonth.Max(d => d.Sum(dr => dr.AllMaterialsConsumedAmountInKilo));
                //wastageProducedGroupedByMonth.ElementAt(0).AverageConsumed = wastageProducedGroupedByMonth.Average(d => d.AverageConsumed);
                //wastageProducedGroupedByMonth.ElementAt(0).AverageConsumed = wastageProducedGroupedByMonth.Average(d => d.AverageConsumed);
                //wastageProducedGroupedByMonth.ElementAt(0).AverageConsumed = wastageProducedGroupedByMonth.Average(d => d.AverageConsumed);

                return wastageProducedGroupedByMonth.OrderBy(d => d.Date).ToList();
            }
        }

        private List<ChartsDTO> GetWastageMonthListGroupdByDay(string year, string month, string saloonId, string deviceId, string productionLineId)
        {

            IQueryable<DailyProductionReport> dailyProductionReports = context.DailyProductionReports.Include("Product").Include("ProductionLine.Device.Saloon");



            if (saloonId != "" && saloonId != null)
            {
                var saloonIdConverted = Guid.Parse(saloonId);
                dailyProductionReports = dailyProductionReports.Where(d => d.ProductionLine.Device.SaloonId == saloonIdConverted);

            }

            if (deviceId != "" && deviceId != null)
            {
                var deviceIdConverted = Guid.Parse(deviceId);
                dailyProductionReports = dailyProductionReports.Where(d => d.ProductionLine.DeviceId == deviceIdConverted);
            }

            if (productionLineId != "" && productionLineId != null)
            {
                var productionLineIdConverted = Guid.Parse(productionLineId);
                dailyProductionReports = dailyProductionReports.Where(d => d.ProductionLineId == productionLineIdConverted);
            }

            var dailyProductionReportsInSelectedMonth = dailyProductionReports.Where(d => d.DateOfProduction.Substring(0, 4) == year).Where(d => d.DateOfProduction.Substring(5, 2) == month).ToList();
            var dailyProductionReportsGroupedByDays = dailyProductionReportsInSelectedMonth.GroupBy(d => d.DateOfProduction.Substring(8, 2)).ToList();
            var wastageProducedGroupedByMonth = dailyProductionReportsGroupedByDays
                .Select(d =>
                    new ChartsDTO
                    {
                        Date = d.Key,
                        WastageAmount = Math.Ceiling(d.Sum(dg => dg.WastageProducedInKilo)),
                        ConsumedAmount = Math.Ceiling(d.Sum(dg => dg.AllMaterialsConsumedAmountInKilo)),
                        MaxConsumed = Math.Ceiling(dailyProductionReportsGroupedByDays.Max(r => r.Sum(dr => dr.AllMaterialsConsumedAmountInKilo))),
                        MaxWastageAmount = Math.Ceiling(dailyProductionReportsGroupedByDays.Max(r => r.Sum(dr => dr.WastageProducedInKilo))),
                        MinConsumed = Math.Ceiling(dailyProductionReportsGroupedByDays.Min(r => r.Sum(dr => dr.AllMaterialsConsumedAmountInKilo))),
                        MinWastage = Math.Ceiling(dailyProductionReportsGroupedByDays.Min(r => r.Sum(dr => dr.WastageProducedInKilo))),
                        AverageConsumed = Math.Ceiling(dailyProductionReportsGroupedByDays.Average(r => r.Sum(dr => dr.AllMaterialsConsumedAmountInKilo))),
                        AverageWastage = Math.Ceiling(dailyProductionReportsGroupedByDays.Average(r => r.Sum(dr => dr.WastageProducedInKilo))),
                        TotalMaterialConsumed = Math.Ceiling(dailyProductionReportsGroupedByDays.Sum(r => r.Sum(dr => dr.AllMaterialsConsumedAmountInKilo))),
                        TotalWastage = Math.Ceiling(dailyProductionReportsGroupedByDays.Sum(r => r.Sum(dr => dr.WastageProducedInKilo)))
                    }).ToList();
            return wastageProducedGroupedByMonth.OrderBy(d => d.Date).ToList();
        }
    }
}
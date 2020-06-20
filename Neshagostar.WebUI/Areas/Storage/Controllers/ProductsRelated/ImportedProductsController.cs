using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Neshagostar.DAL.DataModel;
using Neshagostar.DAL.DataModel.StorageRelated.ProductsRelated;
using Neshagostar.WebUI.Areas.Storage.Models;

namespace Neshagostar.WebUI.Areas.Storage.Controllers.ProductsRelated
{
    public class ImportedProductsController : Controller
    {
        private NeshagostarContext context = new NeshagostarContext();
        const int PAGESIZE = 4;

        // GET: Storage/ImportedProducts
        public ActionResult Index()
        {
            // this method gets previous days date starting from today
            List<PersianDateTime> days = GetPreviousDays(5);

            // here we trim the obtained days hour and minutes
            List<string> daysTrimmed = new List<string>();
            foreach (PersianDateTime day in days)
            {
                daysTrimmed.Add(day.ToString().Substring(0, 10));
            }

            // here we put همه تواریخ as the first row in drop down list
            var dates = new List<SelectListItem>();
            dates.Add(new SelectListItem
            {
                Text = "همه تواریخ",
                Value = ""
            });



            var importedProducts = context.ImportedProducts.Include(i => i.Product).Include("AllocatedProducts.SendingAllocateds").Include(p => p.AllocatedProducts).OrderByDescending(p => p.Date).ToList();
            // now we put the days in List in order to show in dates drop down list
            ViewBag.Dates = daysTrimmed.OrderByDescending(d => d).Select(d => new SelectListItem
            {
                Text = d,
                Value = d
            });
            ViewBag.PageNumber = Math.Ceiling(decimal.Parse(importedProducts.Count.ToString()) / (decimal)PAGESIZE);
            ViewBag.Page = 1;
            ViewBag.PageSize = PAGESIZE;
            importedProducts = importedProducts.Take(PAGESIZE).ToList();
            return View(importedProducts);
        }

        [HttpPost]
        public JsonResult Filter(string importDate, string productId, string isAllocatedCompletely = "", string isSentCompletely = "", int page = 1)
        {
            IQueryable<ImportedProduct> orders = context.ImportedProducts.Include("AllocatedProducts").Include("Product");

            if (importDate != "" && importDate != null && importDate != "previousDates")
            {
                string[] modelarray = importDate.Split('/');
                PersianDateTime dateModel = new PersianDateTime(int.Parse(modelarray[0]), int.Parse(modelarray[1]), int.Parse(modelarray[2]));
                string dateInModel = dateModel.Date.ToString().Substring(0, 10);

                orders = orders.Where(i => i.Date.Substring(0, 10).Equals(dateInModel));
            }

        

            if (productId != "")
            {
                var convertedProductId = Guid.Parse(productId);
                orders = orders.Where(i => i.ProductId.Equals(convertedProductId));
                //var orderItemsWithSameProductIdGroupedBy = orders.OrderByDescending(o => o.Order.Date).GroupBy(ord => ord.OrderId).ToList();
                //List<List<OrderItemStorageInfo>> resultWithSameProductId = ConvertOrdersToStorageInfoList(orderItemsWithSameProductIdGroupedBy);

            }

            var allOrders = orders.ToList();

            if (isAllocatedCompletely != "" && isAllocatedCompletely != null)
            {
                if (isAllocatedCompletely == "true")
                {
                    allOrders = allOrders.Where(i => i.IsAllocatedCompletely == true).ToList();
                }
                else
                {
                    allOrders = allOrders.Where(i => i.IsAllocatedCompletely == false).ToList();
                }
            }

            if (isSentCompletely != "" && isSentCompletely != null)
            {
                if (isSentCompletely == "true")
                {
                    allOrders = allOrders.Where(i => i.IsSentCompletely == true).ToList();
                }
                else
                {
                    allOrders = allOrders.Where(i => i.IsSentCompletely == false).ToList();
                }
            }


            var ordersList = allOrders.OrderByDescending(i => i.Date).ToList();


            var pageNumbers = Math.Ceiling(ordersList.Count / (decimal)PAGESIZE);
            ordersList = ordersList.Skip((page - 1) * PAGESIZE).Take(PAGESIZE).ToList();

            if (page > pageNumbers)
            {
                page = Int32.Parse((pageNumbers).ToString());
            }

            List<ImportedProductDTO> result = new List<ImportedProductDTO>();
            foreach (var item in ordersList)
            {
                result.Add(ConvertToDTO(item));
            }


            if (result.Count > 0)
            {
                result.ElementAt(0).Page = page;
                result.ElementAt(0).PageNumbers = pageNumbers;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // GET: Storage/ImportedProducts/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImportedProduct importedProduct = context.ImportedProducts.Find(id);
            if (importedProduct == null)
            {
                return HttpNotFound();
            }
            return View(importedProduct);
        }

        // GET: Storage/ImportedProducts/Create
        public ActionResult Create()
        {
            //ViewBag.ProductId = new SelectList(db.Products.OrderBy(p => p.Code), "Id", "Title");
            return View();
        }

        // POST: Storage/ImportedProducts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProductId,Date,AmountImported,Comments")] ImportedProduct importedProduct)
        {
            if (ModelState.IsValid)
            {
                importedProduct.Id = Guid.NewGuid();
                context.ImportedProducts.Add(importedProduct);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductId = new SelectList(context.Products.OrderBy(p => p.Code), "Id", "Title", importedProduct.ProductId);
            return View(importedProduct);
        }

        // GET: Storage/ImportedProducts/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImportedProduct importedProduct = context.ImportedProducts.Find(id);
            if (importedProduct == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = new SelectList(context.Products.OrderBy(p => p.Code), "Id", "Title", importedProduct.ProductId);
            return View(importedProduct);
        }

        // POST: Storage/ImportedProducts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProductId,Date,AmountImported,Comments")] ImportedProduct importedProduct)
        {
            var oldImported = context.ImportedProducts.Include(p => p.AllocatedProducts).Where(p => p.Id == importedProduct.Id).FirstOrDefault();

            var sumOfAmounts = oldImported.AllocatedProducts.Sum(al => al.Amount);

            if(importedProduct.AmountImported < sumOfAmounts)
            {
                ModelState.AddModelError("AmountImported", "مقدار وارده کمتر از مقدار تخصیص داده شده قبلی است");
            }

            if (ModelState.IsValid)
            {
                oldImported.Date = importedProduct.Date;
                oldImported.AmountImported = importedProduct.AmountImported;
                oldImported.Comments = importedProduct.Comments;
                context.Entry(oldImported).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductId = new SelectList(context.Products.OrderBy(p => p.Code), "Id", "Title", importedProduct.ProductId);
            return View(importedProduct);
        }

        // GET: Storage/ImportedProducts/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImportedProduct importedProduct = context.ImportedProducts.Find(id);
            if (importedProduct == null)
            {
                return HttpNotFound();
            }
            return View(importedProduct);
        }

        // POST: Storage/ImportedProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ImportedProduct importedProduct = context.ImportedProducts.Include(i => i.AllocatedProducts).Where(p => p.Id == id).FirstOrDefault();
            if(importedProduct.AllocatedProducts.Count > 0)
            {
                ModelState.AddModelError("AmountImported", "مقدار تخصیص داده شده دارد");
                return RedirectToAction("Delete");
            }

            context.ImportedProducts.Remove(importedProduct);
            context.SaveChanges();
            return RedirectToAction("Index");
        }


        private static List<PersianDateTime> GetPreviousDays(double previousdays)
        {
            double numberOfDayspreviousdays = (previousdays + 1) * -1;
            var today = PersianDateTime.Now.Date;
            List<PersianDateTime> days = new List<PersianDateTime>();
            days.Add(today);
            for (int i = -1; i > numberOfDayspreviousdays; i--)
            {
                days.Add(today.AddDays(i));
            }

            return days;
        }

        public string FormatDate(string date)
        {
            string[] strArray = date.Split('/');
            var dateFormatted = new PersianDateTime(Int32.Parse(strArray[0]), Int32.Parse(strArray[1]), Int32.Parse(strArray[2])).ToString().Substring(0, 10);
            return dateFormatted;

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }

        public ImportedProductDTO ConvertToDTO(ImportedProduct model)
        {
            return new ImportedProductDTO
            {
                Id = model.Id,
                Date = model.Date,
                Comments = model.Comments,
                ProductId = model.ProductId,
                AmountImported = model.AmountImported,
                AmountAllocated = model.AmountAllocated,
                AmountSent = model.AmountSent,
                AmountUnAllocated = model.AmountUnAllocated,
                AmountUnSent = model.AmountUnSent,
                IsAllocatedCompletely = model.IsAllocatedCompletely,
                IsSentCompletely = model.IsSentCompletely,
                ProductName = model.Product.Title                      
            };
        }


    }
}

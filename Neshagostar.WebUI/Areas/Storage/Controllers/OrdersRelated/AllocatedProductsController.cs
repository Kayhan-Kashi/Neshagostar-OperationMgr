using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Neshagostar.DAL.DataModel;
using Neshagostar.DAL.DataModel.OrdersRelated.ProductsRelated;
using Neshagostar.DAL.DataModel.StorageRelated.ProductsRelated;
using Neshagostar.WebUI.Areas.Storage.Models;

namespace Neshagostar.WebUI.Areas.Storage.Controllers.OrdersRelated
{
    public class AllocatedProductsController : Controller
    {
        private NeshagostarContext db = new NeshagostarContext();


        public ActionResult Allocate(Guid importedProductId)
        {
          
            var importedProduct = db.ImportedProducts.Include(p => p.Product).Include("AllocatedProducts.OrderItem.Order.Customer").Where(ip => ip.Id.Equals(importedProductId)).FirstOrDefault();
            var relatedUnfinishedOrderItems = db.OrderItems.Include(o => o.Order.Customer).Include(or => or.Sendings).Include(or => or.AllocatedProducts).Where(o => o.ProductId.Equals(importedProduct.ProductId)).ToList();
            relatedUnfinishedOrderItems = relatedUnfinishedOrderItems.Where(p => p.IsProductionCompleted == false).ToList();
            ViewBag.RelatedAllocations = importedProduct.AllocatedProducts
                .Select(p => new AllocationDTO { Id=p.Id, AllocatingDate = p.Date, Amount=p.Amount , CustomerName=p.OrderItem.Order.Customer.Name, ProductionDate = p.ImportedProduct.Date })
                .ToList();

            
            ViewBag.RelatedOrderItems = relatedUnfinishedOrderItems.Select(r => new SelectListItem
            {
                Text = r.Order.Customer.Name + "    " + "،مقدار تخصیص یافته " + r.AllocatedAmount + "    " +  "، مقدار تخصیص نیافته " + r.UnAllocatedAmount,
                Value = r.Id.ToString()
            });

            ViewBag.ImportedAmount = importedProduct.AmountImported;
            ViewBag.AmountAllocated = importedProduct.AmountAllocated;
            ViewBag.AmountUnAllocated = importedProduct.AmountUnAllocated;
            return View();
        }

        [HttpPost]
        public ActionResult Allocate(AllocatedProduct allocatedProduct)
        {
            var unAllocatedAmount = db.OrderItems.Include(o => o.AllocatedProducts).Where(p => p.Id == allocatedProduct.OrderItemId).FirstOrDefault().UnAllocatedAmount;
            if(unAllocatedAmount < allocatedProduct.Amount)
            {
                ModelState.AddModelError("Amount", "مقدار تخصیص بیشتر از مقدار سفارش است");
            }
            var availableAmount = db.ImportedProducts.Include(i => i.AllocatedProducts).Where(p => p.Id == allocatedProduct.ImportedProductId).FirstOrDefault().AmountUnAllocated;
            if (availableAmount < allocatedProduct.Amount)
            {
                ModelState.AddModelError("Amount", "مقدار تخصیص بیشتر از مقدار موجود است");
            }
            if (ModelState.IsValid)
            {
                allocatedProduct.Id = Guid.NewGuid();
                db.AllocatedProducts.Add(allocatedProduct);
                db.SaveChanges();
                return RedirectToAction("Index", new { controller="ImportedProducts" });
            }

            var importedProduct = db.ImportedProducts.Include(p => p.Product).Include("AllocatedProducts.OrderItem.Order.Customer").Where(ip => ip.Id.Equals(allocatedProduct.ImportedProductId)).FirstOrDefault();
            var relatedUnfinishedOrderItems = db.OrderItems.Include(o => o.Order.Customer).Include(or => or.Sendings).Include(or => or.AllocatedProducts).Where(o => o.ProductId.Equals(importedProduct.ProductId)).ToList();
            relatedUnfinishedOrderItems = relatedUnfinishedOrderItems.Where(p => p.IsProductionCompleted == false).ToList();
            ViewBag.RelatedAllocations = importedProduct.AllocatedProducts
              .Select(p => new AllocationDTO { Id = p.Id, AllocatingDate = p.Date, Amount = p.Amount, CustomerName = p.OrderItem.Order.Customer.Name, ProductionDate = p.ImportedProduct.Date })
              .ToList();


            ViewBag.RelatedOrderItems = relatedUnfinishedOrderItems.Select(r => new SelectListItem
            {
                Text = r.Order.Customer.Name + "    " + "،مقدار تخصیص یافته " + r.AllocatedAmount + "    " + "، مقدار تخصیص نیافته " + r.UnAllocatedAmount,
                Value = r.Id.ToString()
            });

            ViewBag.ImportedAmount = importedProduct.AmountImported;
            ViewBag.AmountAllocated = importedProduct.AmountAllocated;
            ViewBag.AmountUnAllocated = importedProduct.AmountUnAllocated;
            return View();
        }

        public ActionResult OrderItemAllocationList(Guid orderItemId)
        {
            var orderitem = db.OrderItems.Include(or => or.Product).Include("AllocatedProducts.ImportedProduct").Include(or => or.Order.Customer).Include(or => or.AllocatedProducts).Where(o => o.Id.Equals(orderItemId)).FirstOrDefault();
            var allocations = orderitem.AllocatedProducts.Select(a => new AllocationDTO
            {
                Id = a.Id,
                Amount = a.Amount,
                CustomerName = a.OrderItem.Order.Customer.Name,
               AllocatingDate = a.Date,
               ProductionDate = a.ImportedProduct.Date
            }).OrderByDescending(o => o.ProductionDate).ToList();
            ViewBag.CustomerName = orderitem.Order.Customer.Name;
            ViewBag.ProductName = orderitem.Product.Title;
            ViewBag.AllocatedAmount = allocations.Sum(p => p.Amount);
            ViewBag.UnAllocatedAmount = orderitem.UnAllocatedAmount;
            ViewBag.AvailableAmountToAllocate = db.ImportedProducts.Include(path => path.AllocatedProducts).Where(p => p.ProductId == orderitem.ProductId).ToList().Sum(i => i.AmountUnAllocated);
            ViewBag.AllocationMethods = new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Text="تخصیص قدیمی ترین ورودی انبار",
                    Value="oldest",
                    Selected=true
                    
                },
                new SelectListItem
                {
                     Text="تخصیص جدید ترین ورودی انبار",
                    Value="newest"
                },
                new SelectListItem
                {
                    Text="تخصیص ورودی انبار در روز مشخص ",
                    Value="customized"
                }
            };
            ViewBag.OrderItemId = orderItemId;
            return View(allocations);
        }

        // GET: Storage/AllocatedProducts
        public ActionResult Index()
        {
            var allocatedProducts = db.AllocatedProducts.Include(a => a.ImportedProduct.Product).Include(a => a.OrderItem);
            return View(allocatedProducts.ToList());
        }

        // GET: Storage/AllocatedProducts/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AllocatedProduct allocatedProduct = db.AllocatedProducts.Find(id);
            if (allocatedProduct == null)
            {
                return HttpNotFound();
            }
            return View(allocatedProduct);
        }

        // GET: Storage/AllocatedProducts/Create
        public ActionResult Create()
        {
            ViewBag.ImportedProductId = new SelectList(db.ImportedProducts, "Id", "Date");
            ViewBag.OrderItemId = new SelectList(db.OrderItems, "Id", "Comments");
            return View();
        }

        // POST: Storage/AllocatedProducts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ImportedProductId,OrderItemId,Amount,Date")] AllocatedProduct allocatedProduct)
        {
            if (ModelState.IsValid)
            {
                allocatedProduct.Id = Guid.NewGuid();
                db.AllocatedProducts.Add(allocatedProduct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ImportedProductId = new SelectList(db.ImportedProducts, "Id", "Date", allocatedProduct.ImportedProductId);
            ViewBag.OrderItemId = new SelectList(db.OrderItems, "Id", "Comments", allocatedProduct.OrderItemId);
            return View(allocatedProduct);
        }

        // GET: Storage/AllocatedProducts/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AllocatedProduct allocatedProduct = db.AllocatedProducts.Find(id);
            if (allocatedProduct == null)
            {
                return HttpNotFound();
            }

            var alProduct = db.AllocatedProducts.Include(p => p.ImportedProduct).Include(o => o.OrderItem.Order.Customer).Where(p => p.Id == id).FirstOrDefault();
            var importedProduct = db.ImportedProducts.Include(p => p.Product).Include("AllocatedProducts.OrderItem.Order.Customer").Where(ip => ip.Id.Equals(allocatedProduct.ImportedProductId)).FirstOrDefault();
            var dto = new AllocationDTO
            {
                Id = alProduct.Id,
                Amount = alProduct.Amount,
                AllocatingDate = alProduct.Date,
                ProductionDate = alProduct.ImportedProduct.Date,
                CustomerName = alProduct.OrderItem.Order.Customer.Name,
                ImportedProductId = allocatedProduct.ImportedProductId,
                OrderItemId = allocatedProduct.OrderItemId
            };
            ViewBag.OrderItemId = alProduct.OrderItemId;
            //ViewBag.ImportedProductId = new SelectList(db.ImportedProducts, "Id", "Date", allocatedProduct.ImportedProductId);
            //ViewBag.OrderItemId = new SelectList(db.OrderItems, "Id", "Comments", allocatedProduct.OrderItemId);
            return View(dto);
        }

        // POST: Storage/AllocatedProducts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AllocatedProduct allocatedProduct)
        {
            var oldEntity = db.AllocatedProducts.Where(p => p.Id == allocatedProduct.Id).FirstOrDefault();
            var difference = oldEntity.Amount - allocatedProduct.Amount;
            double oldUnallocatedOrderAmount = db.OrderItems.Include(o => o.AllocatedProducts).Where(p => p.Id == allocatedProduct.OrderItemId).FirstOrDefault().UnAllocatedAmount;
            double unAllocatedOrderAmount;

            if (difference >= 0)
            {
                unAllocatedOrderAmount = oldUnallocatedOrderAmount + oldEntity.Amount;
            }
            else
            {
                unAllocatedOrderAmount = oldUnallocatedOrderAmount + difference;
            }
               
            if (unAllocatedOrderAmount < 0 || unAllocatedOrderAmount < allocatedProduct.Amount)
            {
                ModelState.AddModelError("Amount", "مقدار تخصیص بیشتر از مقدار سفارش است");
            }


            double availableAmount;
            //db.SaveChanges();
            if (difference >= 0)
            {
                availableAmount = db.ImportedProducts.Include(i => i.AllocatedProducts).Where(p => p.Id == allocatedProduct.ImportedProductId).FirstOrDefault().AmountUnAllocated + oldEntity.Amount;
            }
            else
            {
                availableAmount = db.ImportedProducts.Include(i => i.AllocatedProducts).Where(p => p.Id == allocatedProduct.ImportedProductId).FirstOrDefault().AmountUnAllocated + difference;
            }
             

            if (availableAmount < 0 || availableAmount < allocatedProduct.Amount)
            {
                ModelState.AddModelError("Amount", "مقدار تخصیص بیشتر از مقدار موجود است");
            }

            if (ModelState.IsValid)
            {
                oldEntity.Amount = allocatedProduct.Amount;
                oldEntity.Date = allocatedProduct.Date;
                db.Entry(oldEntity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Allocate", new { controller="AllocatedProducts", importedProductId=allocatedProduct.ImportedProductId});
            }
            var customer = db.OrderItems.Include(o => o.Order.Customer).Where(p => p.Id == allocatedProduct.OrderItemId);
            ViewBag.ImportedProductId = new SelectList(db.ImportedProducts, "Id", "Date", allocatedProduct.ImportedProductId);
            //ViewBag.OrderItemId = new SelectList(db.OrderItems, "Id", "Comments", allocatedProduct.OrderItemId);
            var alProduct = db.AllocatedProducts.Include(p => p.ImportedProduct).Include(o => o.OrderItem.Order.Customer).Where(p => p.Id == allocatedProduct.Id).FirstOrDefault();
            var importedProduct = db.ImportedProducts.Include(p => p.Product).Include("AllocatedProducts.OrderItem.Order.Customer").Where(ip => ip.Id.Equals(allocatedProduct.ImportedProductId)).FirstOrDefault();

            var dto = new AllocationDTO
            {
                Id = allocatedProduct.Id,
                Amount = allocatedProduct.Amount,
                AllocatingDate = allocatedProduct.Date,
                ProductionDate = importedProduct.Date,
                CustomerName = alProduct.OrderItem.Order.Customer.Name,
                ImportedProductId = allocatedProduct.ImportedProductId,
                OrderItemId = allocatedProduct.OrderItemId
            };
            return View(dto);
        }

        // GET: Storage/AllocatedProducts/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AllocatedProduct allocatedProduct = db.AllocatedProducts.Find(id);
            if (allocatedProduct == null)
            {
                return HttpNotFound();
            }
            return View(allocatedProduct);
        }

        // POST: Storage/AllocatedProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            AllocatedProduct allocatedProduct = db.AllocatedProducts.Find(id);
            db.AllocatedProducts.Remove(allocatedProduct);
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

        public ActionResult Change(Guid id)
        {
            var allocated = db.AllocatedProducts.Include(p => p.ImportedProduct.Product).Include(p => p.OrderItem.Order).Where(o => o.Id.Equals(id)).FirstOrDefault();
            return View();
        }

        public JsonResult AllocateProductionToOrderItemDirectly(Guid orderItemId, string allocationMethod, double amountWishToAlloc)
        {
            var productId = db.OrderItems.Include("Product").Where(ord => ord.Id == orderItemId).FirstOrDefault().ProductId;
            var importedProducts = db.ImportedProducts.Include("AllocatedProducts").Where(i => i.ProductId == productId).ToList().Where(o => !(o.IsAllocatedCompletely)).ToList();


            if (allocationMethod == "oldest")
            {
                double remainedAmount = amountWishToAlloc;
                importedProducts = importedProducts.OrderBy(p => p.Date).ToList();
             
                foreach(var importedProduct in importedProducts)
                {
                    if(remainedAmount > 0)
                    {
                        remainedAmount = AllocateToImportedProduct(amountWishToAlloc, importedProduct, orderItemId);
                    }               
                }

                return Json(Url.Action("OrderItemAllocationList", new { orderItemId = orderItemId, controller = "AllocatedProducts", area = "storage" }), JsonRequestBehavior.AllowGet);
            }
            if (allocationMethod == "newest")
            {
                double remainedAmount = amountWishToAlloc;
                importedProducts = importedProducts.OrderByDescending(p => p.Date).ToList();

                foreach (var importedProduct in importedProducts)
                {
                    if (remainedAmount > 0)
                    {
                        remainedAmount = AllocateToImportedProduct(amountWishToAlloc, importedProduct, orderItemId);
                    }
                }
                return Json(Url.Action("OrderItemAllocationList", new { orderItemId = orderItemId, controller = "AllocatedProducts", area = "storage" }), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(Url.Action("AllocateCustomizedProduct", new { controller = "AllocatedProducts", area = "storage", orderItemId = orderItemId }), JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult AllocateCustomizedProduct(Guid orderItemId)
        {
            var productId = db.OrderItems.Where(d => d.Id == orderItemId).FirstOrDefault().ProductId;
            var importedProducts = db.ImportedProducts.Include(i => i.Product).Include(p => p.AllocatedProducts).Where(o => o.ProductId == productId).ToList().Where(p => !(p.IsAllocatedCompletely)).OrderByDescending(p => p.Date).ToList();
            return View(importedProducts);
        }

        public double AllocateToImportedProduct(double amountWishToAlloc, ImportedProduct importedProduct, Guid orderItemId)
        {
            double amountReadyToAlloc = CalculateAmountToAlloc(amountWishToAlloc, importedProduct);
            db.AllocatedProducts.Add(new AllocatedProduct
            {

                Amount = amountReadyToAlloc,
                Date = PersianDateTime.Now.ToString().Substring(0, 10),
                Id = Guid.NewGuid(),
                ImportedProductId = importedProduct.Id,
                OrderItemId = orderItemId
            });
            db.SaveChanges();
            // amount that is remained and has to be allocated in next imported products
            return amountWishToAlloc - amountReadyToAlloc;
        }

        public double CalculateAmountToAlloc(double amountWishToAlloc, ImportedProduct importedProduct)
        {
            if(amountWishToAlloc > importedProduct.AmountUnAllocated)
            {
                return importedProduct.AmountUnAllocated;
            }
            else if(amountWishToAlloc < importedProduct.AmountUnAllocated)
            {
                return amountWishToAlloc;
            }
            else
            {
                return amountWishToAlloc;
            }
        }
    }
}

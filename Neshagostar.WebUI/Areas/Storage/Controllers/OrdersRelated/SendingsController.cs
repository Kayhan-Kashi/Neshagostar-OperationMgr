using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Neshagostar.DAL.DataModel;
using Neshagostar.DAL.DataModel.StorageRelated.OrdersRelated;
using Neshagostar.WebUI.Areas.Storage.Models;
using Newtonsoft.Json;

namespace Neshagostar.WebUI.Areas.Storage.Controllers.OrdersRelated
{
    public class SendingsController : Controller
    {
        const int PAGESIZE = 30;
        private NeshagostarContext context = new NeshagostarContext();

        // GET: Storage/Sendings
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

            // now we put the days in List in order to show in dates drop down list
            ViewBag.Dates = daysTrimmed.OrderByDescending(d => d).Select(d => new SelectListItem
            {
                Text = d,
                Value = d
            });

    

            var sendings = context.Sendings.Include(s => s.OrderItem.Order.Customer).Include(s => s.OrderItem.Product.ProductGenre.ProductCategory).Include(s => s.OrderItem.Product).Include("SendingAllocateds.AllocatedProduct").OrderByDescending(s => s.DateSent).ToList();
            var orderItemDtos = sendings.Select(s => new OrderItemSendingDetailDTO
            {
                DateSent = s.DateSent,
                CustomerName = s.OrderItem.Order.Customer.Name,
                ProductName = s.OrderItem.Product.Title,
                SendingAmount = s.AmountSent,
                Unit = s.OrderItem.Product.ProductGenre.ProductCategory.SendingUnit,
                Destination = s.Destination,
                Id = s.Id
            });
            ViewBag.PageNumber = Math.Ceiling(decimal.Parse(sendings.Count.ToString()) / (decimal)PAGESIZE);
            ViewBag.Page = 1;
            ViewBag.PageSize = PAGESIZE;

            var result = sendings.Take(PAGESIZE).ToList();
            return View( orderItemDtos.ToList());
        }

        // GET: Storage/Sendings/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sending sending = context.Sendings.Where(s => s.Id == id).Include("SendingAllocateds.AllocatedProduct.ImportedProduct.AllocatedProducts.OrderItem.Order.Customer").FirstOrDefault();
            var productName = context.OrderItems.Where(p => p.Id == sending.OrderItemId).Include(p => p.Product.ProductGenre).FirstOrDefault().Product.Title;
            OrderItemSendingDetailDTO sendingDTO = new OrderItemSendingDetailDTO
            {
                Id = sending.Id,
                BasculeWeight = sending.BasculeWeight,
                CarrierNumberCode = sending.CarrierNumberCode,
                Comments = sending.Comments,
                DateArrived = sending.DateArrived,
                DateSent = sending.DateSent,
                DriverTel = sending.DriverTel,
                Destination = sending.Destination,
                OrderItemId = sending.OrderItemId,
                Price = sending.Price,
                SendingAmount = sending.AmountSent,
                CustomerName = sending.OrderItem.Order.Customer.Name,
                DriverName = sending.DriverName,
                ProductName = productName,
                SendingAllocateds = sending.SendingAllocateds.Select(s => new SendingAllocateDTO
                {
                    Id = s.Id,
                    AllocatedProductId = s.AllocatedProductId,
                    SendingAmount = s.AmountSent,
                    DateProducted = s.AllocatedProduct.ImportedProduct.Date,
                    AvailableAmountToSend = s.AllocatedProduct.AmountUnSent + s.AmountSent
                }).ToList()
            };
            if (sending == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderItemId = new SelectList(context.OrderItems, "Id", "Comments", sending.OrderItemId);
            return View(sendingDTO);
        }

        [HttpPost]
        public JsonResult Filter(string sentDate, string customerName, string productId, int page = 1)
        {
            IQueryable<Sending> orders = context.Sendings.Include("SendingAllocateds").Include("OrderItem.Order.Customer").Include("OrderItem.Product.ProductGenre.ProductCategory");

            if (sentDate != "" && sentDate != null && sentDate != "previousDates")
            {
                string[] modelarray = sentDate.Split('/');
                PersianDateTime dateModel = new PersianDateTime(int.Parse(modelarray[0]), int.Parse(modelarray[1]), int.Parse(modelarray[2]));
                string dateInModel = dateModel.Date.ToString().Substring(0, 10);

                orders = orders.Where(i => i.DateSent.Substring(0, 10).Equals(dateInModel));
            }

            if (customerName != "" && customerName != null)
            {
                orders = orders.Where(i => i.OrderItem.Order.Customer.Name.Contains(customerName));
            }

            var allOrders = orders.ToList();

            if (productId != "")
            {
                var convertedProductId = Guid.Parse(productId);
                orders = orders.Where(i => i.OrderItem.ProductId.Equals(convertedProductId));
                //var orderItemsWithSameProductIdGroupedBy = orders.OrderByDescending(o => o.Order.Date).GroupBy(ord => ord.OrderId).ToList();
                //List<List<OrderItemStorageInfo>> resultWithSameProductId = ConvertOrdersToStorageInfoList(orderItemsWithSameProductIdGroupedBy);

            }


            var ordersList = orders.OrderByDescending(i => i.DateSent).ToList();


            var pageNumbers = Math.Ceiling(ordersList.Count / (decimal)PAGESIZE);
            ordersList = ordersList.Skip((page - 1) * PAGESIZE).Take(PAGESIZE).ToList();

            if (page > pageNumbers)
            {
                page = Int32.Parse((pageNumbers).ToString());
            }

            List<OrderItemSendingDetailDTO> result = new List<OrderItemSendingDetailDTO>();
            foreach (var item in ordersList)
            {
                result.Add(ConvertToSendingDetailDTO(item));
            }


            if (result.Count > 0)
            {
                result.ElementAt(0).Page = page;
                result.ElementAt(0).PageNumbers = pageNumbers;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
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


        // POST: Storage/Sendings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(List<SendingDTO> sendings, Guid orderItemId)
        {
            if (ModelState.IsValid)
            {
                 var orderItem = context.OrderItems
                .Include("Product.ProductGenre.ProductCategory")
                .Include("Order.Customer")
                .Where(o => o.Id == orderItemId)
                .FirstOrDefault();

                sendings = sendings.Where(p => p.SendingAmount > 0).ToList();
              
                ViewBag.CustomerName = orderItem.Order.Customer.Name;
                ViewBag.ProductName = orderItem.Product.Title;
                ViewBag.DispatchDate = orderItem.DateOfDispatch;
                ViewBag.OrderItemId = orderItem.Id.ToString();
                ViewBag.Sendings = JsonConvert.SerializeObject(sendings);
                ViewBag.OrderItemId = orderItemId;
                ViewBag.SendingAmount = sendings.Sum(p => p.SendingAmount);
                ViewBag.SendingAllocateds = sendings;
                return View("~/Areas/Storage/Views/OrdersRelated/Sendings/Send.cshtml", new OrderItemSendingDetailDTO());
            }

           
            return View();
        }

        [HttpPost]
        public ActionResult Send(string sendings, Guid orderItemId , OrderItemSendingDetailDTO orderItemSendingDetail)
        {
            var sendingsDtos = JsonConvert.DeserializeObject<List<SendingDTO>>(sendings);
            var newSending = new Sending
            {
                Id = Guid.NewGuid(),
                BasculeWeight = orderItemSendingDetail.BasculeWeight,
                CarrierNumberCode = orderItemSendingDetail.CarrierNumberCode,
                Comments = orderItemSendingDetail.Comments,
                DateArrived = FormatDate(orderItemSendingDetail.DateArrived),
                DateSent = FormatDate(orderItemSendingDetail.DateSent),
                Destination = orderItemSendingDetail.Destination,
                DriverName = orderItemSendingDetail.DriverName,
                DriverTel = orderItemSendingDetail.DriverTel,
                OrderItemId = orderItemId,
                Price = orderItemSendingDetail.Price,
                SendingAllocateds = sendingsDtos.Select(p => new SendingAllocated
                {
                    Id = Guid.NewGuid(),
                    AllocatedProductId = p.AllocatedProductId,
                    AmountSent = p.SendingAmount
                }).ToList()
            };

            context.Sendings.Add(newSending);
            context.SaveChanges();
            return RedirectToAction("Edit", new { area = "Storage", controller = "Orders", id = orderItemId });
        }


        // GET: Storage/Sendings/Edit/5
        public ActionResult Edit(Guid? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sending sending = context.Sendings.Where(s => s.Id == id).Include("SendingAllocateds.AllocatedProduct.ImportedProduct.AllocatedProducts.OrderItem.Order.Customer").FirstOrDefault();
            var productName = context.OrderItems.Where(p => p.Id == sending.OrderItemId).Include(p => p.Product.ProductGenre).FirstOrDefault().Product.Title;
            OrderItemSendingDetailDTO sendingDTO = new OrderItemSendingDetailDTO
            {
                Id = sending.Id,
                BasculeWeight = sending.BasculeWeight,
                CarrierNumberCode = sending.CarrierNumberCode,
                Comments = sending.Comments,
                DateArrived = sending.DateArrived,
                DateSent = sending.DateSent,
                DriverTel = sending.DriverTel,
                Destination = sending.Destination,
                OrderItemId = sending.OrderItemId,
                Price = sending.Price,
                SendingAmount = sending.AmountSent,
                CustomerName = sending.OrderItem.Order.Customer.Name,
                DriverName = sending.DriverName,
                ProductName =productName,
                SendingAllocateds = sending.SendingAllocateds.Select(s => new SendingAllocateDTO
                {
                    Id = s.Id,
                    AllocatedProductId = s.AllocatedProductId,
                    SendingAmount = s.AmountSent,
                    DateProducted = s.AllocatedProduct.ImportedProduct.Date,
                    AvailableAmountToSend = s.AllocatedProduct.AmountUnSent + s.AmountSent
                }).ToList()
            };
            if (sending == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderItemId = new SelectList(context.OrderItems, "Id", "Comments", sending.OrderItemId);
            return View(sendingDTO);
        }

        // POST: Storage/Sendings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Edit(OrderItemSendingDetailDTO sending)
        {
            if (ModelState.IsValid)
            {
                foreach(var sent in sending.SendingAllocateds)
                {
                    var item = context.SendingAllocateds.Find(sent.Id);
                    item.AmountSent = sent.SendingAmount;
                    context.Entry(item).State = EntityState.Modified;
                    context.SaveChanges();
                }
                var sendingToEdit = context.Sendings.Find(sending.Id);
                sendingToEdit.BasculeWeight = sending.BasculeWeight;
                sendingToEdit.CarrierNumberCode = sending.CarrierNumberCode;
                sendingToEdit.Comments = sending.Comments;
                sendingToEdit.DateArrived = FormatDate(sending.DateArrived);
                sendingToEdit.DateSent = FormatDate(sending.DateSent);
                sendingToEdit.Destination = sending.Destination;
                sendingToEdit.DriverName = sending.DriverName;
                sendingToEdit.DriverTel = sending.DriverTel;
                sendingToEdit.Price = sending.Price;
                context.Entry(sendingToEdit).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrderItemId = new SelectList(context.OrderItems, "Id", "Comments", sending.OrderItemId);
            return View(sending);
        }

        public string FormatDate(string date)
        {
            string[] strArray = date.Split('/');
            var dateFormatted = new PersianDateTime(Int32.Parse(strArray[0]), Int32.Parse(strArray[1]), Int32.Parse(strArray[2])).ToString().Substring(0, 10);
            return dateFormatted;

        }

        private static OrderItemSendingDetailDTO ConvertToSendingDetailDTO(Sending sendingDetail)
        {
            return new OrderItemSendingDetailDTO
            {
                BasculeWeight = sendingDetail.BasculeWeight,
                CarrierNumberCode = sendingDetail.CarrierNumberCode,
                Comments = sendingDetail.Comments,
                DateArrived = sendingDetail.DateArrived,
                DateSent = sendingDetail.DateSent,
                Destination = sendingDetail.Destination,
                DriverName = sendingDetail.DriverName,
                DriverTel = sendingDetail.DriverTel,
                Id = sendingDetail.Id,
                OrderItemId = sendingDetail.OrderItemId,
                Price = sendingDetail.Price,
                SendingAmount = sendingDetail.AmountSent,
                CustomerName = sendingDetail.OrderItem.Order.Customer.Name,
                ProductName = sendingDetail.OrderItem.Product.Title,
                Unit = sendingDetail.OrderItem.Product.ProductGenre.ProductCategory.SendingUnit,

            };
        }

        // GET: Storage/Sendings/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sending sending = context.Sendings.Find(id);
            if (sending == null)
            {
                return HttpNotFound();
            }
            return View(sending);
        }

        // POST: Storage/Sendings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Sending sending = context.Sendings.Find(id);
       
            context.Sendings.Remove(sending);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

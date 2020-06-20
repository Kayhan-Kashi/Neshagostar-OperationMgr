using Neshagostar.DAL.DataModel;
using Neshagostar.DAL.DataModel.CommerceRelated.OrdersRelated;
using Neshagostar.WebUI.Areas.Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Neshagostar.WebUI.Areas.Storage.Controllers.OrdersRelated
{
  
    [Authorize(Roles ="CEO, storage-manager")]
    public class OrdersController : Controller
    {
        const int PAGESIZE = 30;
        NeshagostarContext context = new NeshagostarContext();


        // GET: Commerce/Orders
        public ActionResult Index()
        {
            ViewBag.Products = context.Products.Select(d => new SelectListItem
            {
                Text = d.Title,
                Value = d.Id.ToString()
            });

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


            // here we get all of the orders so far
            //var orders = context.Orders.Include("Customer")
            //    .Include("OrderItems.Product.ProductCategory")
            //    .Include("OrderItems.Order")
            //    .Include("OrderItems.OrderItemSendingDetails")
            //    .OrderByDescending(i => i.Date)
            //    .Where(o => o.OrderItems.Any(ord => ord.IsDispatched))
            //    .ToList();

            var ordersDispatched = context.OrderItems
                .Include("Order.Customer")
                .Include("Product.ProductGenre.ProductCategory")
                .Include("Sendings.SendingAllocateds")
                .Include("AllocatedProducts")
                .OrderByDescending(ord => ord.Order.Date).OrderBy(o => o.Product.Code)
                .Where(ord => ord.IsDispatched).ToList().GroupBy(ord => ord.OrderId).ToList();

            
            List<List<OrderItemStorageInfo>> ordersConvertedToStorageInfoList =  ConvertOrdersToStorageInfoList(ordersDispatched);

            //var ords = ordersDispatched.ElementAt(0).ElementAt(0);

            //List<List<OrderItemStorageInfo>> ordersConvertedToStorageInfoList = new List<List<OrderItemStorageInfo>>();
       

      

            // we use this technique to convert it from reference to value type in order to obtain number of page number calculating from all the numbers of inquiries
            ViewBag.PageNumber = Math.Ceiling(decimal.Parse(ordersDispatched.Count.ToString()) / (decimal)PAGESIZE);
            ViewBag.Page = 1;
            ViewBag.PageSize = PAGESIZE;

            var result = ordersDispatched.Take(PAGESIZE).ToList();


            // we convert order items into inquiry bags
            List<List<OrderItemStorageInfo>> ordersStorageInfos = ConvertOrdersToStorageInfoList(result);
            ordersStorageInfos = ordersStorageInfos.OrderByDescending(or => or.FirstOrDefault().OrderDate).ToList();

            ViewBag.TotalMaterialNeeded = GetMaterialsNeeded();
            ViewBag.TotalConsumedMaterialInCurrentYear = GetConsumedMaterialInCurrentYear();

            return View("~/Areas/Storage/Views/OrdersRelated/Orders/index.cshtml", ordersStorageInfos);
        }

        [HttpPost]
        public JsonResult Filter(string orderdate, string dispatchdate, string customerName, string productId, int page = 1)
        {
            IQueryable<OrderItem> orders = context.OrderItems.Include("Order.Customer").Include("Product.ProductGenre.ProductCategory").Include("Sendings").Include("AllocatedProducts").Where(o => o.IsDispatched);

            if (orderdate  != "" && orderdate != null && orderdate != "previousDates")
            {
                string[] modelarray = orderdate.Split('/');
                PersianDateTime dateModel = new PersianDateTime(int.Parse(modelarray[0]), int.Parse(modelarray[1]), int.Parse(modelarray[2]));
                string dateInModel = dateModel.Date.ToString().Substring(0, 10);

                orders = orders.Where(i => i.Order.Date.Substring(0, 10).Equals(dateInModel));
            }

            if (dispatchdate != "" && dispatchdate != null && orderdate != "previousDates")
            {
                string[] modelarray = dispatchdate.Split('/');
                PersianDateTime dateModel = new PersianDateTime(int.Parse(modelarray[0]), int.Parse(modelarray[1]), int.Parse(modelarray[2]));
                string dateInModel = dateModel.Date.ToString().Substring(0, 10);

                orders = orders.Where(i => i.DateOfDispatch.Substring(0, 10).Equals(dateInModel));
            }

            if (customerName != "" && customerName != null)
            {
                orders = orders.Where(i => i.Order.Customer.Name.Contains(customerName));
            }

            var allOrders = orders.ToList();

            if (productId != "")
            {
                var convertedProductId = Guid.Parse(productId);
                orders = orders.Where(i => i.ProductId.Equals(convertedProductId));
                //var orderItemsWithSameProductIdGroupedBy = orders.OrderByDescending(o => o.Order.Date).GroupBy(ord => ord.OrderId).ToList();
                //List<List<OrderItemStorageInfo>> resultWithSameProductId = ConvertOrdersToStorageInfoList(orderItemsWithSameProductIdGroupedBy);

            }


            var ordersList = orders.OrderByDescending(i => i.Order.Date).ToList();

            var ordersGroupedBy = ordersList.OrderByDescending(or => or.Order.Date).OrderBy(o => o.Product.Code).ToList().GroupBy(or => or.OrderId).ToList();
            var pageNumbers = Math.Ceiling(ordersGroupedBy.Count / (decimal)PAGESIZE);
            ordersGroupedBy = ordersGroupedBy.Skip((page - 1) * PAGESIZE).Take(PAGESIZE).ToList();

            if (page > pageNumbers)
            {
                page = Int32.Parse((pageNumbers).ToString());
            }

            List<List<OrderItemStorageInfo>> result = ConvertOrdersToStorageInfoList(ordersGroupedBy);

            if (result.Count > 0)
            {
                result.ElementAt(0).ElementAt(0).Page = page;
                result.ElementAt(0).ElementAt(0).PageNumbers = pageNumbers;
            }

            result = result.OrderByDescending(or => or.FirstOrDefault().OrderDate).ToList();

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


        private List<List<OrderItemStorageInfo>> ConvertOrdersToStorageInfoList(List<IGrouping<Guid, OrderItem>> orders)
        {
            List<List<OrderItemStorageInfo>> ordersConvertedToStorageInfoList = new List<List<OrderItemStorageInfo>>();
            
            foreach(var order in orders)
            {
                ordersConvertedToStorageInfoList.Add(ConvertOrderToStorageInfos(order));
            }
            return ordersConvertedToStorageInfoList;
        }

        private List<OrderItemStorageInfo> ConvertOrderToStorageInfos(IGrouping<Guid, OrderItem> order)
        {
            List<OrderItemStorageInfo> storageInfos = new List<OrderItemStorageInfo>();
            
            foreach(var orderItem in order)
            {
                storageInfos.Add(ConvertOrderItemToStorageInfo(orderItem));
            }
            return storageInfos;
        }

        private OrderItemStorageInfo ConvertOrderItemToStorageInfo(OrderItem orderItem)
        {
            return new OrderItemStorageInfo
            {
                OrderItemId = orderItem.Id,
                OrderId = orderItem.OrderId,
                CustomerId = orderItem.Order.CustomerId,
                CustomerName = orderItem.Order.Customer.Name,
                DispatchDate = orderItem.DateOfDispatch,
                OrderDate = orderItem.Order.Date,
                LastSendingDate = orderItem.LastSendingDate,
                AmountDispatched = orderItem.AmountDispatched,
                AmountSent = orderItem.AmountSent,
                AmountNotSent = orderItem.AmountNotSent,
                ProductName = orderItem.Product.Title,
                ProductId = orderItem.ProductId,
                OrderItemSendingDetails = orderItem.Sendings.Select(o => new OrderItemSendingDetailDTO
                {
                    BasculeWeight = o.BasculeWeight,
                    CarrierNumberCode = o.CarrierNumberCode,
                    Comments = o.Comments,
                    DateArrived = o.DateArrived,
                    DateSent = o.DateSent,
                    Destination = o.Destination,
                    DriverName = o.DriverName,
                    DriverTel = o.DriverTel,
                    Id = o.Id,
                    OrderItemId = o.OrderItemId,
                    Price = o.Price,
                    SendingAmount = o.AmountSent
                }).ToList(),
                Unit = orderItem.Product.ProductGenre.ProductCategory.SendingUnit,
                IsSendingCompleted = orderItem.IsSendingCompleted,
                AmountAllocated = orderItem.AllocatedAmount,
                AmountUnAllocated = orderItem.UnAllocatedAmount,
                IsProductionCompleted = orderItem.IsProductionCompleted
         
                
                
            };
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var orderItem = context.OrderItems.Include("Product.ProductGenre.ProductCategory").Include("Order.Customer").Include("Sendings.SendingAllocateds").Include("AllocatedProducts.SendingAllocateds").Include("AllocatedProducts.ImportedProduct").Where(ord => ord.Id.Equals(id)).FirstOrDefault();

            var sendingAllocateds = new List<SendingAllocateDTO>();
            if (orderItem.AllocatedProducts != null && orderItem.AllocatedProducts.Count > 0)
            {
                sendingAllocateds = orderItem.AllocatedProducts.Where(al => !(al.IsSentCompletely)).Select(al => new SendingAllocateDTO
                 {
                     AllocatedProductId = al.Id,
                     DateProducted = al.ImportedProduct.Date,
                     AvailableAmountToSend = al.AmountUnSent
                 }).ToList();
                ViewBag.SendingAllocateds = sendingAllocateds;

            }


            if (orderItem != null)
            {
                var allocs = context.AllocatedProducts.Where(p => p.OrderItemId == orderItem.Id).ToList();
                if(allocs != null && allocs.Count > 0)
                {
                    var sumOfAllocated = allocs.Sum(p => p.Amount);
                    ViewBag.AvailableAmountToSend = sumOfAllocated - orderItem.AmountSent > 0 ? sumOfAllocated - orderItem.AmountSent : 0;
                }
                else
                {
                    ViewBag.AvailableAmountToSend = 0;

                }
             
                orderItem.Sendings = orderItem.Sendings.OrderByDescending(o => o.DateSent).ToList();
                OrderItemStorageInfo storageInfo =   ConvertOrderItemToStorageInfo(orderItem);
                return View("~/Areas/Storage/Views/OrdersRelated/Orders/Edit.cshtml", storageInfo);
            }

            return HttpNotFound();


        }

        public double GetMaterialsNeeded()
        {

            var ordersDispatched = context.OrderItems
                .Include("Order.Customer")
                .Include("Product.ProductGenre.ProductCategory")
                .Include("Sendings.SendingAllocateds")
                .Include("AllocatedProducts")
                .ToList();

            return ordersDispatched.Sum(o => o.UnAllocatedAmount * o.NominalWeightPerMeter);
        }

        public double GetConsumedMaterialInCurrentYear()
        {
            var ordersDispatched = context.OrderItems
                .Include("Order.Customer")
                .Include("Product.ProductGenre.ProductCategory")
                .Include("Sendings.SendingAllocateds")
                .Include("AllocatedProducts")
                .ToList();

           

            return ordersDispatched.Where(p => p.DateToRecieve.Substring(0,4) == PersianDateTime.Now.Year.ToString()).Sum(o => o.AllocatedAmount * o.NominalWeightPerMeter);
        }
    }
}
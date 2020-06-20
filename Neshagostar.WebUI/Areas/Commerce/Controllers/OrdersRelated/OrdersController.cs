using Neshagostar.DAL.DataModel;
using Neshagostar.DAL.DataModel.CommerceRelated.OrdersRelated;
using Neshagostar.WebUI.Areas.Commerce.Models.OrdersRelated;
using Neshagostar.WebUI.Infrastructure.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Neshagostar.WebUI.Areas.Commerce.Controllers.OrdersRelated
{
    [Authorize(Roles = "CEO, commerce-manager")]
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
            List<Order> orders = context.Orders.Include("Customer").Include("OrderItems.Product.ProductGenre.ProductCategory").Include("OrderItems.Order").Include("OrderItems.Sendings.SendingAllocateds").OrderByDescending(i => i.Date).ToList();

            // we use this technique to convert it from reference to value type in order to obtain number of page number calculating from all the numbers of inquiries
            ViewBag.PageNumber = Math.Ceiling(decimal.Parse(orders.Count.ToString()) / (decimal)PAGESIZE);
            ViewBag.Page = 1;
            ViewBag.PageSize = PAGESIZE;

            var result = orders.Take(PAGESIZE).ToList();


            // we convert order items into inquiry bags
            List<List<OrderBag>> orderBags = ConvertOrdersToOrderBag(result);

            return View("~/Areas/Commerce/Views/OrdersRelated/Orders/index.cshtml", orderBags);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Products = new SelectList(context.Products, "Id", "Name");
            return View("~/Areas/Commerce/Views/OrdersRelated/Orders/Create.cshtml");
        }

        public ActionResult Edit(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            OrderItem orderItem = context.OrderItems.Include("Product.ProductGenre.ProductCategory").Include("Order.Customer").Where(i => i.Id.Equals(id)).FirstOrDefault();
            OrderBag orderBag = null;
            if (orderItem != null)
            {
                orderBag = ConvertIOrderItemToOrderBag(orderItem);
            }
            return View("~/Areas/Commerce/Views/OrdersRelated/Orders/Edit.cshtml", orderBag);
        }
        //[ActivityLogger(ActivityName = "ویرایش", ModelNameBeingOperated = "استعلام")]
        [HttpPost]

        public ActionResult Edit(OrderBag bag)
        {
            OrderItem orderItem = context.OrderItems.Include("Order").Where(o => o.Id.Equals(bag.Id)).FirstOrDefault();
            orderItem.IsDispatched = bag.IsDispatched;
            orderItem.ProductId = bag.ProductId;
            orderItem.NominalWeightPerMeter = bag.NominalWeightPerMeter;
            orderItem.TotalWeight = bag.TotalWeight;
            orderItem.Amount = bag.Amount;
            orderItem.Order.Comments = bag.Comments;
            orderItem.HDPEPrice = bag.HDPEPrice;
            orderItem.NominalWeightPerMeter = bag.NominalWeightPerMeter;
            orderItem.PricePerUnit = bag.PricePerUnit;
            orderItem.TotalPrice = bag.Amount * bag.PricePerUnit;
            orderItem.WasherPrice = bag.WasherPrice;
            orderItem.AmountDispatched = bag.AmountDispatched;
            orderItem.DateToRecieve = FormatDate(bag.DateToRecieve);
            orderItem.DateOfDispatch = FormatDate(bag.DateOfDispatch);
            orderItem.DispatchComments = bag.DispatchComments;
            orderItem.Comments = bag.Comments;
            orderItem.Order.Date = FormatDate(bag.DateTime);
           
            

            context.Entry(orderItem).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
            ViewBag.ModelOperatedId = orderItem.Id.ToString();
            return RedirectToAction("Index", new { area = "commerce", controller = "Orders" });

        }

        public ActionResult Details(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            OrderItem orderItem = context.OrderItems.Include("Product.ProductGenre.ProductCategory").Include("Order.Customer").Where(i => i.Id.Equals(id)).FirstOrDefault();
            OrderBag orderBag = null;
            if (orderItem != null)
            {
                orderBag = ConvertIOrderItemToOrderBag(orderItem);
            }
            return View("~/Areas/Commerce/Views/OrdersRelated/Orders/Details.cshtml", orderBag);
        }



        public ActionResult Add(Guid orderId)
        {
            Order order = context.Orders.Include("Customer").Where(i => i.Id.Equals(orderId)).FirstOrDefault();
            OrderBag bag = new OrderBag
            {
                OrderId = order.Id,
                CustomerId = order.CustomerId,
                CustomerName = order.Customer.Name,
                CustomerTel = order.Customer.TelephoneNumber,
                CustomerCity = order.Customer.City
            };
            return View("~/Areas/Commerce/Views/OrdersRelated/Orders/Add.cshtml", bag);
        }

        [HttpPost]
     //   [ActivityLogger(ActivityName = "اضافه", ModelNameBeingOperated = "استعلام")]
        public ActionResult Add(OrderBag bag)
        {
            OrderItem item = new OrderItem()
            {
                Id = Guid.NewGuid(),
                Amount = bag.Amount,
                Comments = bag.Comments,
                HDPEPrice = bag.HDPEPrice,
                OrderId = bag.OrderId,
                IsDispatched = bag.IsDispatched,
                NominalWeightPerMeter = bag.NominalWeightPerMeter,
                PricePerUnit = bag.PricePerUnit,
                TotalWeight = bag.TotalWeight,
                ProductId = bag.ProductId,
                WasherPrice = bag.WasherPrice,
                TotalPrice = (bag.Amount * bag.PricePerUnit),
                DateToRecieve = FormatDate(bag.DateToRecieve),               
            };
            context.OrderItems.Add(item);
            context.SaveChanges();
            ViewBag.ModelOperatedId = item.Id;
            return RedirectToAction("Index", new { area = "commerce", controller = "Orders" });
        }


        [HttpPost]
        public JsonResult Create(Order order)
        {

            order.Id = Guid.NewGuid();
            order.Date = FormatDate(order.Date);

            foreach (OrderItem item in order.OrderItems)
            {
                item.Id = Guid.NewGuid();
                item.OrderId = order.Id;
            }

            context.Orders.Add(order);
            context.SaveChanges();
            ViewBag.ModelOperatedId = order.Id.ToString();


            // we return return url in json because in ajax success callback function it's been used in windows.location method
            return Json(Url.Action("Index", "Orders", new { area = "Commerce" }));
        }

        [HttpPost]
        public JsonResult Filter(string date, string customerName, string productId, int page = 1)
        {
            IQueryable<Order> orders = context.Orders.Include("Customer").Include("OrderItems.Product.ProductGenre.ProductCategory").Include("OrderItems.Sendings.SendingAllocateds");

            if (date != "" && date != null)
            {
                string[] modelarray = date.Split('/');
                PersianDateTime dateModel = new PersianDateTime(int.Parse(modelarray[0]), int.Parse(modelarray[1]), int.Parse(modelarray[2]));
                string dateInModel = dateModel.Date.ToString().Substring(0, 10);

                orders = orders.Where(i => i.Date.Substring(0, 10).Equals(dateInModel));
            }

            if (customerName != "" && customerName != null)
            {
                orders = orders.Where(i => i.Customer.Name.Contains(customerName));
            }

            var allOrders = orders.ToList();

            if (productId != "")
            {
                // first we get the list of inquiry Items that has the defined product id
                var convertedProductId = Guid.Parse(productId);
                var orderItems = context.OrderItems.Where(i => i.ProductId.Equals(convertedProductId)).ToList();
                // and then we filter inquiries based on the id of inquiryItems that we have found.
                if (orderItems != null)
                {
                    List<Order> inqToList = new List<Order>();
                    List<Order> ordersWithSameProductId = new List<Order>();
                    foreach (var item in orderItems)
                    {
                        ordersWithSameProductId.AddRange(allOrders.Where(i => i.Id == item.OrderId));

                    }
                    allOrders = allOrders.Intersect(ordersWithSameProductId).ToList();
                }
            }

            var ordersList = allOrders.OrderByDescending(i => i.Date).ToList();
            var pageNumbers = Math.Ceiling(ordersList.Count / (decimal)PAGESIZE);
            ordersList = ordersList.Skip((page - 1) * PAGESIZE).Take(PAGESIZE).ToList();

            if (page > pageNumbers)
            {
                page = Int32.Parse((pageNumbers).ToString());
            }

            List<List<OrderBag>> result = ConvertOrdersToOrderBag(ordersList);

            if(result.Count > 0)
            {
                result.ElementAt(0).ElementAt(0).Page = page;
                result.ElementAt(0).ElementAt(0).PageNumbers = pageNumbers;
            }
      
            //foreach (var inq in inquiriesList)
            //{
            //    result.Add(ConvertInquiryItemsToInquiryBag(inq.InquiryItems));

              
            //}

            //if (result.Count == 0)
            //{
            //    var bag = new InquiryBag();
            //    var inquiryList = new List<InquiryBag>();
            //    inquiryList.Add(bag);
            //    result.Add(inquiryList);
            //}

            return Json(result, JsonRequestBehavior.AllowGet);



        }

        private OrderBag ConvertIOrderItemToOrderBag(OrderItem item)
        {
            return new OrderBag
            {
                CustomerId = item.Order.CustomerId,
                OrderId = item.OrderId,
                CustomerName = item.Order.Customer.Name,
                CustomerCity = item.Order.Customer.City,
                CustomerTel = item.Order.Customer.TelephoneNumber,
                Id = item.Id,
                //InquiryId = item.Order.InquiryId,
                ProductId = item.ProductId,
                DateTime = FormatDate(item.Order.Date),
                ProductName = item.Product.Title,
                Amount = item.Amount,
                PricePerUnit = item.PricePerUnit,
                PricePerKilo = item.PricePerKilo,
                TotalPrice = item.TotalPrice,
                TotalWeight = item.TotalWeight,
                AmountDispatched = item.AmountDispatched,
                HDPEPrice = item.HDPEPrice,
                NominalWeightPerMeter = item.NominalWeightPerMeter,
                Comments = item.Order.Comments,
                WasherPrice = item.WasherPrice,
                InquiryTotalPrice = item.Order.OrderItemsPriceSummation,
                IsDispatched = item.IsDispatched,
                DateOfDispatch = item.DateOfDispatch,
                DispatchComments = item.DispatchComments,
                DateToRecieve = item.DateToRecieve,
                OrderItemComments = item.Comments,
                PriceSummation = item.Order.OrderItemsPriceSummation,
                Unit = item.Product.ProductGenre.ProductCategory.SendingUnit,
                SentAmount = item.AmountSent,
                UnSentAmount = item.AmountNotSent
                
            };
        }

        public List<List<OrderBag>> ConvertOrdersToOrderBag(List<Order> orders)
        {
            List<List<OrderBag>> orderbags = new List<List<OrderBag>>();

            foreach (Order order in orders)
            {
                List<OrderBag> bags = new List<OrderBag>();
                order.OrderItems = order.OrderItems.OrderBy(o => o.Product.Code).ToList();
                foreach (var orderItem in order.OrderItems)
                {
                    bags.Add(ConvertIOrderItemToOrderBag(orderItem));
                }
                orderbags.Add(bags);
            }

            return orderbags;
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

        public JsonResult GetProductsChosen(Guid orderId)
        {
            var orderItems = context.Orders.Include("OrderItems.Product").Where(i => i.Id.Equals(orderId)).FirstOrDefault().OrderItems.ToList();


            return Json(orderItems.Select(i => new { id = i.ProductId }), JsonRequestBehavior.AllowGet);
        }

        private string FormatDate(string date)
        {
            if (date == null)
                return "1/1/1";


            string[] strArray = date.Split('/');
            string dateTime = new PersianDateTime(Int32.Parse(strArray[0]), Int32.Parse(strArray[1]), Int32.Parse(strArray[2])).Date.ToString().Substring(0, 10);
            return dateTime;
        }
        public ActionResult Ok()
        {
            return View();
        }



    }
}
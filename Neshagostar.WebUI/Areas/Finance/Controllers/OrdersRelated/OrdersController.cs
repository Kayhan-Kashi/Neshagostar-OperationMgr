using Neshagostar.DAL.DataModel;
using Neshagostar.DAL.DataModel.CommerceRelated.OrdersRelated;
using Neshagostar.DAL.DataModel.FinanceRelated;
using Neshagostar.WebUI.Areas.Finance.Models.OrdersRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Neshagostar.WebUI.Areas.Finance.Controllers
{
    [Authorize(Roles = "CEO, financial-manager")]
    public class OrdersController : Controller
    {
        const int PAGESIZE = 30;
        NeshagostarContext context = new NeshagostarContext();


        // GET: Finance/Orders
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
            ViewBag.PageNumber = context.Orders.Count();
            ViewBag.Page = 1;



            var orders = context.Orders.Include("OrderItems.Product.ProductGenre.ProductCategory").Include("Customer").Include("Payments.PaymentMethod").Include("OrderItems.Sendings").OrderByDescending(o => o.Date).Take(PAGESIZE).ToList();
            List<List<OrderItemFinanceDTO>> financeDTOs = new List<List<OrderItemFinanceDTO>>();
            foreach(var order in orders)
            {
                financeDTOs.Add(ConvertOrderToFinanceDTOs(order).ToList());
            }
            return View("~/Areas/Finance/Views/OrdersRelated/Orders/Index.cshtml", financeDTOs);
        }

        [HttpPost]
        public JsonResult Filter(string date, string customerName, string productId, string isPayingCompleted = "",  int page = 1)
        {
            IQueryable<Order> orders = context.Orders.Include("OrderItems.Product.ProductGenre.ProductCategory").Include("Customer").Include("Payments.PaymentMethod").Include("OrderItems.Sendings").OrderByDescending(o => o.Date);

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

            if (isPayingCompleted != "" && isPayingCompleted != null)
            {
                if(isPayingCompleted == "true")
                {
                    allOrders = allOrders.Where(i => i.IsPayingCompleted == true).ToList();
                }
                else
                {
                    allOrders = allOrders.Where(i => i.IsPayingCompleted == false).ToList();
                }               
            }

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


            List<List<OrderItemFinanceDTO>> result = new List<List<OrderItemFinanceDTO>>();
            foreach (var order in ordersList)
            {
                result.Add(ConvertOrderToFinanceDTOs(order).ToList());
            }




            if (result.Count > 0)
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

        [HttpGet]
        public ActionResult Pay(Guid orderId)
        {
            ViewBag.PaymentMethods = context.PaymentMethods.Select(p => new SelectListItem
            {
                Text = p.Title,
                Value = p.Id.ToString()             
            });



            var order = context.Orders.Include("OrderItems.Product.ProductGenre.ProductCategory").Include("Customer").Include("Payments.PaymentMethod").Include("OrderItems.Sendings").Where(o => o.Id == orderId).FirstOrDefault();
            var payments = order.Payments.OrderByDescending(p => p.Date).ToList();
            order.Payments = payments;
            List<OrderItemFinanceDTO> financeDTOs =  ConvertOrderToFinanceDTOs(order).ToList();
            ViewBag.TotalSentItemsValues = order.TotalSentItemsValues;
            ViewBag.TotalUnsentItemsValues = order.OrderItemsPriceSummation - order.TotalSentItemsValues;
            return View("~/Areas/Finance/Views/OrdersRelated/PaymentsRelated/Pay.cshtml", financeDTOs);
        }

        [HttpPost]
        public ActionResult Pay(PaymentDTO payment)
        {
            if(payment.CheckDateToCash == null)
            {
                payment.CheckDateToCash = "";
            }

            if (payment.Comments == null)
            {
                payment.Comments = "";
            }


            var newPayment = MapToPayment(payment);
            context.Payments.Add(newPayment);
            context.SaveChanges();

            return RedirectToAction("Pay", new { controller = "Orders", orderId = payment.OrderId, area = "Finance" });
        }


        private IEnumerable<OrderItemFinanceDTO> ConvertOrderToFinanceDTOs(Order order)
        {
            order.OrderItems = order.OrderItems.OrderBy(o => o.Product.Code).ToList();
            foreach(var orderItem in order.OrderItems)
            {
                yield return ConvertToFinanceDTO(orderItem);
            }
        }

        private OrderItemFinanceDTO ConvertToFinanceDTO(OrderItem orderItem)
        {
            return new OrderItemFinanceDTO
            {
                CustomerId = orderItem.Order.CustomerId,
                OrderId = orderItem.OrderId,
                ProductId = orderItem.ProductId,
                OrderItemId = orderItem.Id,

                CustomerName = orderItem.Order.Customer.Name,
                ProductName = orderItem.Product.Title,

                LastPayingDate = orderItem.Order.LastPayingDate,

                Unit = orderItem.Product.ProductGenre.ProductCategory.SendingUnit,

                OrderItemAmount = orderItem.Amount,
                OrderItemSentAmount = orderItem.AmountSent,
                OrderItemUnsentAmount = orderItem.AmountNotSent,

                OrderItemTotalPrice = orderItem.TotalPrice,
                OrderPayedAmount = orderItem.Order.TotalPayedAmounts,
                OrderRemainingToPayAmount = orderItem.Order.RemainingAmount,
                OrderTotalPrice = orderItem.Order.TotalPriceWithAddedCost,
                PricePerUnit = orderItem.PricePerUnit,

                IsPayingCompleted = orderItem.Order.IsPayingCompleted,

                SentAmountTotalPrice = orderItem.AmountSent * orderItem.PricePerUnit,
                UnsentAmountTotalPrice = orderItem.AmountNotSent * orderItem.PricePerUnit,
                OrderRemainingAmountToPayFromSentItems = orderItem.Order.RemainingAmountToPayFromSentItems,

                HasAddedCost = orderItem.Order.HasAddedCost,

                OrderDate = FormatDate(orderItem.Order.Date),
                CustomerCity = orderItem.Order.Customer.City,
                CustomerTel = orderItem.Order.Customer.TelephoneNumber,

                Payments = orderItem.Order.Payments.OrderByDescending(o => o.Date).Select(p => new PaymentDTO
                {
                    Amount = p.Amount,
                    CheckDateToCash = FormatDate(p.CheckDateToCash),
                    Comments = p.Comments,
                    CustomerId = orderItem.Order.CustomerId,
                    CustomerName = orderItem.Order.Customer.Name,
                    Date = (p.Date),
                    Id = p.Id,
                    IsCheckCashed = p.IsCheckCashed,
                    OrderId = p.OrderId,
                    PaymentMethodId = p.PaymentMethodId,
                    PaymentMethodTitle = p.PaymentMethod.Title

                }).ToList()
            };
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

        private static Payment MapToPayment(PaymentDTO payment) {
            return new Payment
            {
                Amount = payment.Amount,
                CheckDateToCash = FormatDate(payment.CheckDateToCash),
                Comments = payment.Comments,
                Date = FormatDate(payment.Date),
                Id = Guid.NewGuid(),
                IsCheckCashed = payment.IsCheckCashed,
                OrderId = payment.OrderId,
                PaymentMethodId = payment.PaymentMethodId
            };
        }


        private static string FormatDate(string date)
        {
            if (date == null || date== "" || date == " ")
                return "1/1/1";


            string[] strArray = date.Split('/');
            string dateTime = new PersianDateTime(Int32.Parse(strArray[0]), Int32.Parse(strArray[1]), Int32.Parse(strArray[2])).Date.ToString().Substring(0, 10);
            return dateTime;
        }
    }
}
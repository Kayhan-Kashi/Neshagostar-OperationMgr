using Neshagostar.DAL.DataModel;
using Neshagostar.DAL.DataModel.CommerceRelated.OrdersRelated;
using Neshagostar.DAL.DataModel.FinanceRelated;
using Neshagostar.WebUI.Areas.Finance.Models.OrdersRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Neshagostar.WebUI.Areas.Finance.Controllers.PaymentsRelated
{
    [Authorize(Roles = "CEO, financial-manager")]
    public class PaymentsController : Controller
    {
        NeshagostarContext context = new NeshagostarContext();
        // GET: Finance/Payments
        const int PAGESIZE = 40;

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
            List<SelectListItem> paymentMethods = new List<SelectListItem>();
            paymentMethods.Add(
            new SelectListItem
            {
                Text = "همه",
                Value = "",
                Selected = true
            });

            paymentMethods.AddRange(context.PaymentMethods.Select(p => new SelectListItem
            {
                Text = p.Title,
                Value = p.Id.ToString()
            }));

            ViewBag.PaymentMethods = paymentMethods;
            ViewBag.Page = 1;
            ViewBag.PageNumber = context.Payments.Count();

            var payments = context.Payments.Include("Order.Customer").Include("PaymentMethod").OrderByDescending(p => p.Date).Take(PAGESIZE).ToList();
            List<PaymentDTO> paymentDTOs = new List<PaymentDTO>();
            foreach (var payment in payments)
            {
                paymentDTOs.Add(ConvertToPaymentDTO(payment));
            }
            return View("~/Areas/Finance/Views/OrdersRelated/PaymentsRelated/Index.cshtml", paymentDTOs);
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var payment = context.Payments.Include("Order.Customer").Include("PaymentMethod").Where(p => p.Id == id).FirstOrDefault();
            var paymentDTO = ConvertToPaymentDTO(payment);

            ViewBag.PaymentMethods = context.PaymentMethods.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Title,
                Selected = p.Id == payment.PaymentMethodId
            }
                );


            return View("~/Areas/Finance/Views/OrdersRelated/PaymentsRelated/Edit.cshtml", paymentDTO);
        }

        [HttpPost]
        public ActionResult Edit(Payment payment)
        {
            var fetchedPayment = context.Payments.Where(p => p.Id == payment.Id).FirstOrDefault();

            fetchedPayment.PaymentMethodId = payment.PaymentMethodId;
            fetchedPayment.Amount = payment.Amount;
            fetchedPayment.CheckDateToCash = FormatDate(payment.CheckDateToCash);
            fetchedPayment.Comments = payment.Comments;
            fetchedPayment.Date = FormatDate(payment.Date);

            context.Entry(fetchedPayment).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            return RedirectToAction("Index", new { controller = "Orders", area = "Finance" });

        }
   
        public ActionResult Details(Guid id)
        {
            var orderId = context.Payments.Where(p => p.Id == id).FirstOrDefault().OrderId;
            ViewBag.PaymentIdToShow = id;
            var order = context.Orders.Include("OrderItems.Product.ProductCategory").Include("Customer").Include("Payments.PaymentMethod").Include("OrderItems.OrderItemSendingDetails").Where(o => o.Id == orderId).FirstOrDefault();
            var payments = order.Payments.OrderByDescending(p => p.Date).ToList();
            order.Payments = payments;
            List<OrderItemFinanceDTO> financeDTOs = ConvertOrderToFinanceDTOs(order).ToList();
            return View("~/Areas/Finance/Views/OrdersRelated/PaymentsRelated/Details.cshtml", financeDTOs);
        }

        private PaymentDTO ConvertToPaymentDTO(Payment payment)
        {
            return new PaymentDTO
            {
                Amount = payment.Amount,
                CheckDateToCash = payment.CheckDateToCash,
                Comments = payment.Comments,
                CustomerId = payment.Order.CustomerId,
                CustomerName = payment.Order.Customer.Name,
                Date = payment.Date,
                Id = payment.Id,
                IsCheckCashed = payment.IsCheckCashed,
                OrderId = payment.OrderId,
                PaymentMethodId = payment.PaymentMethodId,
                PaymentMethodTitle = payment.PaymentMethod.Title
            };
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


        private static string FormatDate(string date)
        {
            if (date == null || date == "" || date == " ")
                return "1/1/1";


            string[] strArray = date.Split('/');
            string dateTime = new PersianDateTime(Int32.Parse(strArray[0]), Int32.Parse(strArray[1]), Int32.Parse(strArray[2])).Date.ToString().Substring(0, 10);
            return dateTime;
        }

        [HttpPost]
        public JsonResult Filter(string date, string customerName, string paymentMethodId = "", int page = 1)
        {
            IQueryable<Payment> payments = context.Payments.Include("Order.Customer").Include("PaymentMethod").OrderByDescending(p => p.Date);

            if (date != "" && date != null)
            {
                string[] modelarray = date.Split('/');
                PersianDateTime dateModel = new PersianDateTime(int.Parse(modelarray[0]), int.Parse(modelarray[1]), int.Parse(modelarray[2]));
                string dateInModel = dateModel.Date.ToString().Substring(0, 10);

                payments = payments.Where(i => i.Date.Substring(0, 10).Equals(dateInModel));
            }

            if (customerName != "" && customerName != null)
            {
                payments = payments.Where(i => i.Order.Customer.Name.Contains(customerName));
            }

            var allPayments = payments.ToList();

            if (paymentMethodId != "" && paymentMethodId != null)
            {
                payments = payments.Where(p => p.PaymentMethodId.ToString() == paymentMethodId);
            }

            var paymentsList = payments.OrderByDescending(i => i.Date).ToList();
            var pageNumbers = Math.Ceiling(paymentsList.Count / (decimal)PAGESIZE);
            paymentsList = paymentsList.Skip((page - 1) * PAGESIZE).Take(PAGESIZE).ToList();

            if (page > pageNumbers)
            {
                page = Int32.Parse((pageNumbers).ToString());
            }


            List<PaymentDTO> result = new List<PaymentDTO>();
            foreach (var payment in paymentsList)
            {
                result.Add(ConvertToPaymentDTO(payment));
            }




            if (result.Count > 0)
            {
                result.ElementAt(0).Page = page;
                result.ElementAt(0).PageNumbers = pageNumbers;
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


        private IEnumerable<OrderItemFinanceDTO> ConvertOrderToFinanceDTOs(Order order)
        {
            foreach (var orderItem in order.OrderItems)
            {
                yield return ConvertToFinanceDTO(orderItem);
            }
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


    }
}

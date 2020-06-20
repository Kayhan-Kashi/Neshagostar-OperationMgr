using Neshagostar.DAL.DataModel;
using Neshagostar.DAL.DataModel.CommerceRelated.InquiriesRelated;
using Neshagostar.WebUI.Areas.Commerce.Models.InquiriesRelated;
using Neshagostar.WebUI.Infrastructure.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Neshagostar.WebUI.Areas.Commerce.Controllers.InquiriesRelated
{
    [Authorize(Roles = "commerce-manager, CEO")]
    public class InquiriesController : Controller
    {
        const int PAGESIZE = 30;

        NeshagostarContext context = new NeshagostarContext();
        // GET: Commerce/Inquiries
        public ActionResult Index()
        {

            // getting products for showing in products search drop down
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


            List<List<InquiryBag>> orderBags = new List<List<InquiryBag>>();

            // here we get all of the inquiries so far
            List<Inquiry> inquiries = context.Inquiries.Include("InquiryItems").Include("Customer").Include("InquiryItems.Product").OrderByDescending(i => i.DateTime).ToList();


            // we use this technique to convert it from reference to value type in order to obtain number of page number calculating from all the numbers of inquiries
            ViewBag.PageNumber = Math.Ceiling(decimal.Parse(inquiries.Count.ToString()) / (decimal)PAGESIZE);
            ViewBag.Page = 1;
            ViewBag.PageSize = PAGESIZE;

            var result = inquiries.Take(PAGESIZE).ToList();


            // we convert inquiry items into inquiry bags
            foreach (var inquiry in result)
            {
                orderBags.Add(ConvertInquiryItemsToInquiryBag(inquiry.InquiryItems));
            }

            return View("~/Areas/Commerce/Views/InquiriesRelated/Inquiries/index.cshtml", orderBags);
        }

        public List<InquiryBag> ConvertInquiryItemsToInquiryBag(List<InquiryItem> inquiryItems)
        {
            List<InquiryBag> bags = new List<InquiryBag>(); 
            foreach(InquiryItem item in inquiryItems)
            {
                bags.Add(ConvertInquiryItemToInquiryBag(item));
            }
            return bags;
        }

        private InquiryBag ConvertInquiryItemToInquiryBag(InquiryItem item)
        {
            return new InquiryBag
            {
                CustomerId = item.Inquiry.CustomerId,
                CustomerName = item.Inquiry.Customer.Name,
                CustomerAddress = item.Inquiry.Customer.InquiryAddress,
                CustomerTel = item.Inquiry.Customer.InquiryTel,
                Id = item.Id,
                InquiryId = item.InquiryId,
                ProductId = item.ProductId,
                DateTime = item.Inquiry.DateTime.Substring(0, 10),
                ProductName = item.Product.Title,
                Amount = item.Amount,
                PricePerUnit = item.PricePerUnit,
                PricePerKilo = item.PricePerKilo,
                TotalPrice = item.TotalPrice,
                TotalWeight = item.TotalWeight,
                HDPEPrice = item.HDPEPrice,
                ReasonForFailure = item.ReasonForFailure,
                NominalWeightPerMeter = item.NominalWeightPerMeter,
                IsSuccessful = item.IsSuccessful,
                Comments = item.Comments,
                WasherPrice = item.WasherPrice,
                InquiryTotalPrice = item.Inquiry.InquiryItemsPriceSummedUp

            };
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Products = new SelectList(context.Products, "Id", "Name");
            return View("~/Areas/Commerce/Views/InquiriesRelated/Inquiries/create.cshtml");
          
        }


        [HttpPost]
        [ActivityLogger(ActivityName = "ثبت", ModelNameBeingOperated = "استعلام")]
        public JsonResult Create(InquiryViewModel inquiry)
        {

            var customer = context.Customers.Where(c => c.Name.Equals(inquiry.CustomerName)).FirstOrDefault();
            Guid customerId;

            // if customer is null means we don't have the same user in database and hence the user is new
            if(customer == null)
            {
                customerId = Guid.NewGuid();
                context.Customers.Add(new DAL.DataModel.CommerceRelated.CustomersRelated.Customer
                {
                    Id = customerId,
                    Name = inquiry.CustomerName,
                    InquiryTel = inquiry.ContactNumber,
                    InquiryAddress = inquiry.Address
                });
                context.SaveChanges();
            }
            // if customer name is present in the database we just update the value of tel and address to make sure they are updated if have been changed.
            else
            {
                customerId = customer.Id;
                customer.InquiryTel = inquiry.ContactNumber;
                customer.InquiryAddress = inquiry.Address;
                context.Entry(customer).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }

            // so far the customer values has been handled and added or modified in the database now it's time to add customer information, datetime and id into inquiry

            Inquiry newInquiry = new Inquiry();
            newInquiry.Id = Guid.NewGuid();
            newInquiry.CustomerId = customerId;
            newInquiry.DateTime = PersianDateTime.Now.ToString();
            newInquiry.HasAddedCost = inquiry.HasAddedCost;       
            newInquiry.Comments = inquiry.Comments;
            newInquiry.AddedCostAmount = inquiry.addedCostAmount;
            


            // now it's time to add inquiry information into inquiry items
            newInquiry.InquiryItems = inquiry.InquiryItems;
            foreach(InquiryItem item in newInquiry.InquiryItems)
            {
                item.Id = Guid.NewGuid();
                item.InquiryId = newInquiry.Id;   
            }

            context.Inquiries.Add(newInquiry);
            context.SaveChanges();
            ViewBag.ModelOperatedId = newInquiry.Id.ToString();


            // we return return url in json because in ajax success callback function it's been used in windows.location method
            return Json(Url.Action("Index", "Inquiries", new { area="Commerce" }));

        }

        public ActionResult Add(Guid inquiryId)
        {
            Inquiry inquiry = context.Inquiries.Include("Customer").Where(i => i.Id.Equals(inquiryId)).FirstOrDefault();
            InquiryBag bag = new InquiryBag
            {
                InquiryId = inquiry.Id,
                CustomerId = inquiry.CustomerId,
                CustomerName = inquiry.Customer.Name,
                CustomerTel = inquiry.Customer.InquiryTel,
                CustomerAddress = inquiry.Customer.InquiryAddress
            };
            return View("~/Areas/Commerce/Views/InquiriesRelated/Inquiries/Add.cshtml", bag);
        }

        [HttpPost]
        [ActivityLogger(ActivityName = "اضافه", ModelNameBeingOperated = "استعلام")]
        public ActionResult Add(InquiryBag bag)
        {
            InquiryItem item = new InquiryItem()
            {
                Id = Guid.NewGuid(),
                Amount = bag.Amount,
                Comments = bag.Comments,
                HDPEPrice = bag.HDPEPrice,
                InquiryId = bag.InquiryId,
                IsSuccessful = bag.IsSuccessful,
                NominalWeightPerMeter = bag.NominalWeightPerMeter,
                PricePerKilo = Math.Ceiling((bag.Amount * bag.PricePerUnit) / bag.TotalWeight),
                PricePerUnit = bag.PricePerUnit,
                TotalWeight = bag.TotalWeight,
                ProductId = bag.ProductId,
                ReasonForFailure = bag.ReasonForFailure,
                WasherPrice = bag.WasherPrice,
                TotalPrice = (bag.Amount * bag.PricePerUnit)
            };
            context.InquiryItems.Add(item);
            context.SaveChanges();
            ViewBag.ModelOperatedId = item.Id;
            return RedirectToAction("Index", new { area = "commerce", controller = "Inquiries" });
        }

        public ActionResult Details(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            InquiryItem inquiryItem = context.InquiryItems.Include("Product").Include("Inquiry.Customer").Where(i => i.Id.Equals(id)).FirstOrDefault();
            InquiryBag inquiryBag = null;
            if (inquiryItem != null)
            {
                inquiryBag = ConvertInquiryItemToInquiryBag(inquiryItem);
            }
            return View("~/Areas/Commerce/Views/InquiriesRelated/Inquiries/Details.cshtml", inquiryBag);
        }

        public ActionResult Edit(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            InquiryItem inquiryItem = context.InquiryItems.Include("Product").Include("Inquiry.Customer").Where(i => i.Id.Equals(id)).FirstOrDefault();
            InquiryBag inquiryBag = null;
            if (inquiryItem != null)
            {
                inquiryBag = ConvertInquiryItemToInquiryBag(inquiryItem);
            }
            return View("~/Areas/Commerce/Views/InquiriesRelated/Inquiries/Edit.cshtml", inquiryBag);
        }
        [ActivityLogger(ActivityName = "ویرایش", ModelNameBeingOperated = "استعلام")]
        [HttpPost]

        public ActionResult Edit(InquiryBag bag)
        {
            InquiryItem inquiryItem = context.InquiryItems.Find(bag.Id);
            inquiryItem.IsSuccessful = bag.IsSuccessful;
            inquiryItem.ProductId = bag.ProductId;
            inquiryItem.Amount = bag.Amount;
            inquiryItem.Comments = bag.Comments;
            inquiryItem.HDPEPrice = bag.HDPEPrice;
            inquiryItem.IsSuccessful = bag.IsSuccessful;
            inquiryItem.NominalWeightPerMeter = bag.NominalWeightPerMeter;
            inquiryItem.PricePerUnit = bag.PricePerUnit;
            inquiryItem.ReasonForFailure = bag.ReasonForFailure;
            inquiryItem.TotalPrice = bag.Amount * bag.PricePerUnit;
            inquiryItem.PricePerKilo = Math.Ceiling(inquiryItem.TotalPrice / bag.TotalWeight);
            inquiryItem.WasherPrice = bag.WasherPrice;
            context.Entry(inquiryItem).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
            ViewBag.ModelOperatedId = inquiryItem.Id.ToString();
            return RedirectToAction("Index", new { area = "commerce", controller = "Inquiries" });

        }

        public JsonResult GetProductsChosen(Guid inquiryId)
        {
            var inquiryItems = context.Inquiries.Include("InquiryItems").Where(i => i.Id.Equals(inquiryId)).FirstOrDefault().InquiryItems.ToList();
          

            return Json(inquiryItems.Select(i => new { id = i.ProductId }), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Filter(string date, string customerName, string productId, int page = 1)
        {
            IQueryable<Inquiry> inquiries = context.Inquiries.Include("Customer").Include("InquiryItems.Product");

            if(date != "" && date != null)
            {
                string[] modelarray = date.Split('/');
                PersianDateTime dateModel = new PersianDateTime(int.Parse(modelarray[0]), int.Parse(modelarray[1]), int.Parse(modelarray[2]));
                string dateInModel = dateModel.Date.ToString().Substring(0, 10);

                inquiries = inquiries.Where(i => i.DateTime.Substring(0, 10).Equals(dateInModel));
            }

            if(customerName != "" && customerName != null)
            {
                inquiries = inquiries.Where(i => i.Customer.Name.Contains(customerName));
            }

            var allInquiries = inquiries.ToList();

            if (productId != "")
            {
                // first we get the list of inquiry Items that has the defined product id
                var convertedProductId = Guid.Parse(productId);
                var inquiryItems = context.InquiryItems.Where(i => i.ProductId.Equals(convertedProductId)).ToList();
                // and then we filter inquiries based on the id of inquiryItems that we have found.
                if(inquiryItems != null)
                {
                    List<Inquiry> inqToList = new List<Inquiry>();
                    List<Inquiry> inquiriesWithSameProductId = new List<Inquiry>();
                    foreach(var item in inquiryItems)
                    {
                        inquiriesWithSameProductId.AddRange(allInquiries.Where(i => i.Id == item.InquiryId));
                      
                    }
                    allInquiries = allInquiries.Intersect(inquiriesWithSameProductId).ToList();
                }
            }

            var inquiriesList = allInquiries.OrderByDescending(i => i.DateTime).ToList();
            var pageNumbers = Math.Ceiling(inquiriesList.Count / (decimal)PAGESIZE);
            inquiriesList = inquiriesList.Skip((page - 1) * PAGESIZE).Take(PAGESIZE).ToList();

            if (page > pageNumbers)
            {
                page = Int32.Parse((pageNumbers).ToString());
            }

            List<List<InquiryBag>> result = new List<List<InquiryBag>>();

            foreach(var inq in inquiriesList)
            {
                result.Add(ConvertInquiryItemsToInquiryBag(inq.InquiryItems));

                result.ElementAt(0).ElementAt(0).Page = page;
                result.ElementAt(0).ElementAt(0).PageNumbers = pageNumbers;
            }

            //if (result.Count == 0)
            //{
            //    var bag = new InquiryBag();
            //    var inquiryList = new List<InquiryBag>();
            //    inquiryList.Add(bag);
            //    result.Add(inquiryList);
            //}

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

        private IQueryable<Inquiry> FilterDate(string date)
        {
            return context.Inquiries.Where(i => i.DateTime.Equals(date));
        }

        

    }
}
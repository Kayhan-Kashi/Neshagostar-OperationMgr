using Neshagostar.DAL.DataModel;
using Neshagostar.DAL.DataModel.CommerceRelated.TenderRelated;
using Neshagostar.WebUI.Areas.Commerce.Models.TendersRelated;
using Neshagostar.WebUI.Infrastructure.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Neshagostar.WebUI.Areas.Commerce.Controllers.TendersRelated
{
    [Authorize(Roles = "commerce-manager, CEO")]
    public class TendersController : Controller
    {
        const int PAGESIZE = 20;

        NeshagostarContext context = new NeshagostarContext();

        [HttpGet]
        public ActionResult Index()
        {
            var tenders = context.Tenders.Include("TenderItems").Include("RivalPrices.Rival").Include("Customer").OrderByDescending(t => t.DateTime).ToList();
            List<TenderViewModel> tenderDTOs = new List<TenderViewModel>();
            foreach(Tender tender in tenders)
            {
                tenderDTOs.Add(ConvertToTenderViewModel(tender));
            }

            tenderDTOs = tenderDTOs.OrderByDescending(t => t.DateTime).ToList();

            return View("~/Areas/Commerce/Views/TendersRelated/Tenders/Index.cshtml", tenderDTOs);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Products = new SelectList(context.Products, "Id", "Name");
            return View("~/Areas/Commerce/Views/TendersRelated/Tenders/Create.cshtml");

        }

        [HttpPost]
        //[ActivityLogger(ActivityName = "ثبت", ModelNameBeingOperated = "مناقصه")]
        public JsonResult Create(TenderViewModel tender)
        {

            var customer = context.Customers.Where(c => c.Name.Equals(tender.CustomerName)).FirstOrDefault();
            Guid customerId;

            // if customer is null means we don't have the same user in database and hence the user is new
            if (customer == null)
            {
                customerId = Guid.NewGuid();
                context.Customers.Add(new DAL.DataModel.CommerceRelated.CustomersRelated.Customer
                {
                    Id = customerId,
                    Name = tender.CustomerName,
                    InquiryTel = tender.ContactNumber,
                    InquiryAddress = tender.Address
                });
                context.SaveChanges();
            }
            // if customer name is present in the database we just update the value of tel and address to make sure they are updated if have been changed.
            else
            {
                customerId = customer.Id;
                customer.InquiryTel = tender.ContactNumber;
                customer.InquiryAddress = tender.Address;
                context.Entry(customer).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }

            // so far the customer values has been handled and added or modified in the database now it's time to add customer information, datetime and id into inquiry

            string[] datetimeArray = tender.DateTime.Split('/');

            Tender newTender = new Tender();
            newTender.Id = Guid.NewGuid();
            newTender.CustomerId = customerId;
            newTender.DateTime = new PersianDateTime(Int32.Parse(datetimeArray[0]), Int32.Parse(datetimeArray[1]), Int32.Parse(datetimeArray[2])).ToString().Substring(0, 10);
            newTender.Comments = tender.Comments;
            newTender.CarriageCost = tender.CarriageCost;
            newTender.InspectionCost = tender.InspectionCost;
            newTender.GuarantyCost = tender.GuarantyCost;
            newTender.PredictionPrice = tender.PredictionPrice;
            newTender.ParticipatingGuarantyPrice = tender.ParticipatingGuarantyPrice;
            newTender.Title = tender.TenderTitle;

            // now it's time to add inquiry information into inquiry items
            newTender.TenderItems = tender.TenderItems;
            foreach (TenderItem item in newTender.TenderItems)
            {
                item.Id = Guid.NewGuid();
                item.TenderId = newTender.Id;
            }

            context.Tenders.Add(newTender);
            context.SaveChanges();
            ViewBag.ModelOperatedId = newTender.Id.ToString();


            // we return return url in json because in ajax success callback function it's been used in windows.location method
            return Json(Url.Action("Index", "Tenders", new { area = "Commerce" }));

        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var tender = context.Tenders.Include("TenderItems.Product").Include("Customer").Where(t => t.Id.Equals(id)).FirstOrDefault();
           
            return View("~/Areas/Commerce/Views/TendersRelated/Tenders/Edit.cshtml", ConvertToTenderViewModel(tender));
        }

        public ActionResult Edit(TenderViewModel tender)
        {
            var newTender = context.Tenders.Include("TenderItems.Product").Include("Customer").Where(t => t.Id.Equals(tender.TenderId)).FirstOrDefault();
            newTender.CarriageCost = tender.CarriageCost;
            newTender.Comments = tender.Comments;
            newTender.DateTime = tender.DateTime;
            newTender.GuarantyCost = tender.GuarantyCost;
            newTender.InspectionCost = tender.InspectionCost;
            newTender.ParticipatingGuarantyPrice = tender.ParticipatingGuarantyPrice;
            newTender.Title = tender.TenderTitle;
            newTender.ReasonForFailure = tender.ReasonForFailure;
            newTender.PredictionPrice = tender.PredictionPrice;
            newTender.IsSuccessful = tender.IsSuccessful;
            context.Entry(newTender).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
            return RedirectToAction("Index", new { area = "Commerce", controller = "Tenders" });

        }

        public ActionResult AddTenderPrice(Guid tenderId)
        {
            ViewBag.Rivals = context.Rivals.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id.ToString()
            }).ToList();

            var tender = context.Tenders.Find(tenderId);
            var rivalPrice = new RivalPrice
            {
                TenderId = tenderId,
                Tender = tender
            };
            return View("~/Areas/Commerce/Views/TendersRelated/Tenders/AddTenderPrice.cshtml", rivalPrice);
        }

        [HttpPost]
        public ActionResult AddTenderPrice(RivalPrice rivalPrice)
        {
            rivalPrice.Id = Guid.NewGuid();
            rivalPrice.Tender = null;
            context.RivalPrices.Add(rivalPrice);
            context.SaveChanges();
            return RedirectToAction("Index", new { area = "commerce", controller = "Tenders" });
        }

        private TenderViewModel ConvertToTenderViewModel(Tender tender)
        {
            TenderViewModel tenderViewModel = new TenderViewModel
            {
                TenderId = tender.Id,
                CustomerId = tender.CustomerId.ToString(),
                CustomerName = tender.Customer.Name,
                Address = tender.Customer.InquiryAddress,
                ContactNumber = tender.Customer.InquiryTel,

                CarriageCost = tender.CarriageCost,
                GuarantyCost = tender.GuarantyCost,
                InspectionCost = tender.InspectionCost,
                ParticipatingGuarantyPrice = tender.ParticipatingGuarantyPrice,
                PredictionPrice = tender.PredictionPrice,
                TotalWeight = tender.TotalWeight,

                Comments = tender.Comments,
                IsSuccessful = tender.IsSuccessful,
                ReasonForFailure = tender.ReasonForFailure,
                DateTime = tender.DateTime,
                
                FinalPrice = tender.FinalPrice,
                TenderTitle = tender.Title
            };

            List<TenderPrice> tenderPrices = new List<TenderPrice>();
            tenderPrices.Add(new TenderPrice
            {
                IsOurCompany = true,
                TenderId = tender.Id,
                ParticipantName = "نشاگستر پردیس",
                Place = "قزوین",
                TotalPrice = tender.FinalPrice,
                TenderTitle = tender.Title,
                TotalWeight = tender.TotalWeight,             
            });

           

            if (tender.RivalPrices != null)
            {
                tender.RivalPrices = tender.RivalPrices.OrderBy(t => t.Price).ToList();
                foreach (var rivalPrice in tender.RivalPrices)
                {
                    TenderPrice tenderPrice = new TenderPrice
                    {
                        Id = rivalPrice.Id,
                        IsOurCompany = false,
                        ParticipantName = rivalPrice.Rival.Name,
                        TenderId = tender.Id,
                        TenderTitle = tender.Title,
                        TotalPrice = rivalPrice.Price,
                        TotalWeight = tender.TotalWeight,
                        Place = rivalPrice.Rival.City,
                    };

                    tenderPrices.Add(tenderPrice);
                }
            }

            tenderViewModel.TenderPrices = tenderPrices.OrderBy(t => t.TotalPrice).ToList();
            tenderViewModel.TenderItems = tender.TenderItems;

            return tenderViewModel;
        }

        public ActionResult EditTenderPrice(Guid id)
        {
            var rivalPrice = context.RivalPrices.Include("Rival").Include("Tender").Where(r => r.Id.Equals(id)).FirstOrDefault();
            ViewBag.RivalName = rivalPrice.Rival.Name;
            return View("~/Areas/Commerce/Views/TendersRelated/Tenders/EditTenderPrice.cshtml", rivalPrice);
        }

        [HttpPost]
        public ActionResult EditTenderPrice(RivalPrice rivalPrice)
        {
            var newRivalPrice = context.RivalPrices.Where(r => r.Id.Equals(rivalPrice.Id)).FirstOrDefault();
            newRivalPrice.Price = rivalPrice.Price;
            context.Entry(newRivalPrice).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
            return RedirectToAction("Index", new { area = "Commerce", controller = "Tenders" });
         
        }

        [HttpGet]
        public ActionResult DeleteTenderPrice(Guid id)
        {
            var rivalPrice = context.RivalPrices.Include("Rival").Include("Tender").Where(r => r.Id.Equals(id)).FirstOrDefault();
            ViewBag.RivalName = rivalPrice.Rival.Name;
            return View("~/Areas/Commerce/Views/TendersRelated/Tenders/DeleteTenderPrice.cshtml", rivalPrice);
        }

        [HttpPost]
        public ActionResult DeleteTenderPrice(RivalPrice rivalPrice)
        {
            var newRivalPrice = context.RivalPrices.Where(r => r.Id.Equals(rivalPrice.Id)).FirstOrDefault();        
            context.Entry(newRivalPrice).State = System.Data.Entity.EntityState.Deleted;
            context.SaveChanges();
            return RedirectToAction("Index", new { area = "Commerce", controller = "Tenders" });

        }

        [HttpGet]
        public ActionResult EditTenderItem(Guid id)
        {
            var tenderItem = context.TenderItems.Include("Product").Include("Tender").Where(t => t.Id.Equals(id)).FirstOrDefault();
            return View("~/Areas/Commerce/Views/TendersRelated/Tenders/EditTenderItem.cshtml", tenderItem);
        }

        [HttpPost]
        public ActionResult EditTenderItem(TenderItem tenderItem)
        {
            var newItem = context.TenderItems.Find(tenderItem.Id);
            newItem.NominalWeightPerMeter = tenderItem.NominalWeightPerMeter;
            newItem.HDPEPrice = tenderItem.HDPEPrice;
            newItem.PricePerUnit = tenderItem.PricePerUnit;
            newItem.TotalWeight = tenderItem.TotalWeight;
            newItem.WasherPrice = tenderItem.WasherPrice;
            newItem.TotalPrice = tenderItem.TotalPrice;
            newItem.Amount = tenderItem.Amount;
            newItem.Comments = tenderItem.Comments;
            context.Entry(newItem).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
            return RedirectToAction("Edit", new { area = "commerce", controller = "Tenders", id = tenderItem.TenderId });
        }


    }
}
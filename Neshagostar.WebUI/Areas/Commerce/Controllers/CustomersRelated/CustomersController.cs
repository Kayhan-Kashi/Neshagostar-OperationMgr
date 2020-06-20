using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Neshagostar.DAL.DataModel;
using Neshagostar.DAL.DataModel.CommerceRelated.CustomersRelated;
using Neshagostar.WebUI.Infrastructure.Filters;

namespace Neshagostar.WebUI.Areas.Commerce.Controllers.CustomersRelated
{
    [Authorize(Roles = "commerce-manager, CEO")]
    public class CustomersController : Controller
    {
        private NeshagostarContext db = new NeshagostarContext();
        const int PAGESIZE = 3;
        // GET: Commerce/Customers
        public ActionResult Index(int page = 1)
        {
            var result = db.Customers.OrderBy(c => c.Name).Take(PAGESIZE).ToList();
            ViewBag.PageNumber = Math.Ceiling(decimal.Parse(db.Callers.ToList().Count.ToString()) / (decimal)PAGESIZE);
            ViewBag.Page = 1;
            return View("~/areas/commerce/views/customers/index.cshtml", result);
        }

        // GET: Commerce/Customers/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View("~/areas/commerce/views/customers/Details.cshtml", customer);
        }

        // GET: Commerce/Customers/Create
        public ActionResult Create()
        {
            return View("~/areas/commerce/views/customers/Create.cshtml");
        }

        // POST: Commerce/Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ActivityLogger(ActivityName = "ثبت", ModelNameBeingOperated = "مشتری")]
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Include = "Id,Name,MobileNumber,TelephoneNumber,City,Address,Date, Comments")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.Id = Guid.NewGuid();
                customer.Date = PersianDateTime.Now.Date.ToString().Remove(10);
                db.Customers.Add(customer);
                db.SaveChanges();
                ViewBag.ModelOperatedId = customer.Id.ToString();
                return RedirectToAction("Index", new { controller="customers", area="commerce" });
            }

            return View("~/areas/commerce/views/customers/Create.cshtml", customer);
        }

        // GET: Commerce/Customers/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View("~/areas/commerce/views/customers/Edit.cshtml", customer);
        }

        // POST: Commerce/Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLogger(ActivityName = "ویرایش", ModelNameBeingOperated = "مشتری")]
        public ActionResult Edit( Customer customer)
        {
            if (ModelState.IsValid)
            {
                var cust = db.Customers.Find(customer.Id);
                cust.Address = customer.Address;
                cust.City = customer.City;
                cust.Comments = customer.Comments;
                cust.Date = customer.Date;
                cust.MobileNumber = customer.MobileNumber;
                cust.TelephoneNumber = customer.TelephoneNumber;
                db.Entry(cust).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.ModelOperatedId = customer.Id.ToString();
                return RedirectToAction("Index", new { controller="customers", area="commerce" });
            }
            return View("~/areas/commerce/views/customers/Edit.cshtml", customer);
        }

        // GET: Commerce/Customers/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View("~/areas/commerce/views/customers/Delete.cshtml", customer);
        }

        // POST: Commerce/Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [ActivityLogger(ActivityName = "حذف", ModelNameBeingOperated = "مشتری")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();
            ViewBag.ModelOperatedId = customer.Id.ToString();
            return RedirectToAction("Index", new { controller="Customers", area="commerce" });
        }

        public JsonResult Filter(string name, string mobile, string tel, string city, int page = 1)
        {

   
          IQueryable<Customer> customers = db.Customers;
            if (name != "")
            {
                customers = customers.Where(r => r.Name.Contains(name));
            }
            var d = customers.ToList();
            if (mobile != "")
            {
                customers = customers.Where(r => r.MobileNumber.Contains(mobile));
            }
            if (tel != "")
            {
                customers = customers.Where(r => r.TelephoneNumber.Contains(tel));
            }
            if (city != "")
            {
                customers = customers.Where(r => r.City.Contains(city));
            }

            customers = customers.OrderBy(r => r.Name);
    

            var customerList = customers.ToList();
            var pageNumbers = Math.Ceiling(customerList.Count / (decimal)PAGESIZE);
            customerList = customerList.Skip((page - 1) * PAGESIZE).Take(PAGESIZE).ToList();

            if (page > pageNumbers)
            {
                page = Int32.Parse((pageNumbers).ToString());
            }

            var results = customerList.Select(r => new
            {
                customerName = r.Name,
                mobile = r.MobileNumber,
                tel = r.TelephoneNumber,
                city = r.City,
                id = r.Id,
                page = page,
                pageNumbers = pageNumbers,
                pageSize = PAGESIZE
            }).ToList();

            ViewBag.PageNumbers = Math.Ceiling(results.Count / (decimal)PAGESIZE);

            ViewBag.Page = page;
            ViewBag.PageSize = PAGESIZE;
            //results = results.Skip((page - 1) * PAGESIZE).Take(PAGESIZE).ToList();



            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Search(string term)
        {
            var result = Json(db.Customers.Where(c => c.Name.Contains(term)).Select(ca => ca.Name).ToList(), JsonRequestBehavior.AllowGet);
            return result;
        }

        public JsonResult GetTelAndAddress(string name)
        {
            var customer = db.Customers.Where(c => c.Name == name).FirstOrDefault();
            return Json(new { tel = customer.TelephoneNumber, address = customer.City, id= customer.Id }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTelAndAddressForInquiry(string name)
        {
            var customer = db.Customers.Where(c => c.Name == name).FirstOrDefault();
            return Json(new { tel = customer.InquiryTel, address = customer.InquiryAddress, id = customer.Id }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult CreateInquirier()
        {
            return View();
        }

        [HttpPost]
        [ActivityLogger(ActivityName = "ثبت", ModelNameBeingOperated = "استعلام گر")]
        public ActionResult CreateInquirier(Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.Id = Guid.NewGuid();
                customer.Date = PersianDateTime.Now.Date.ToString().Remove(10);
                db.Customers.Add(customer);
                db.SaveChanges();
                ViewBag.ModelOperatedId = customer.Id.ToString();
                return RedirectToAction("IndexInquirier", new { controller="customers", area="commerce" });
            }

            return View("~/areas/commerce/views/customers/CreateInquirier.cshtml", customer);
        }

        public ActionResult IndexInquirier()
        {
            var result = db.Customers.OrderBy(c => c.Name).Take(PAGESIZE).ToList();
            ViewBag.PageNumber = Math.Ceiling(decimal.Parse(db.Callers.ToList().Count.ToString()) / (decimal)PAGESIZE);
            ViewBag.Page = 1;
            return View("~/areas/commerce/views/customers/IndexInquirier.cshtml", result);
        }

        public JsonResult FilterInquirier(string name, string tel, string address, int page = 1)
        {


            IQueryable<Customer> customers = db.Customers;
            if (name != "")
            {
                customers = customers.Where(r => r.Name.Contains(name));
            }
            var d = customers.ToList();
            if (tel != "")
            {
                customers = customers.Where(r => r.InquiryTel.Contains(tel));
            }
            if (address != "")
            {
                customers = customers.Where(r => r.InquiryAddress.Contains(address));
            }

            customers = customers.OrderBy(r => r.Name);


            var customerList = customers.ToList();
            var pageNumbers = Math.Ceiling(customerList.Count / (decimal)PAGESIZE);
            customerList = customerList.Skip((page - 1) * PAGESIZE).Take(PAGESIZE).ToList();

            if (page > pageNumbers)
            {
                page = Int32.Parse((pageNumbers).ToString());
            }

            var results = customerList.Select(r => new
            {
                customerName = r.Name,
                mobile = r.MobileNumber,
                tel = r.TelephoneNumber,
                city = r.City,
                id = r.Id,
                inquirierTel = r.InquiryTel,
                inquirierAddress = r.InquiryAddress,
                page = page,
                pageNumbers = pageNumbers,
                pageSize = PAGESIZE
            }).ToList();

            ViewBag.PageNumbers = Math.Ceiling(results.Count / (decimal)PAGESIZE);

            ViewBag.Page = page;
            ViewBag.PageSize = PAGESIZE;
            //results = results.Skip((page - 1) * PAGESIZE).Take(PAGESIZE).ToList();



            return Json(results, JsonRequestBehavior.AllowGet);
        }


        public ActionResult EditInquirier(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View("~/areas/commerce/views/customers/EditInquirier.cshtml", customer);
        }

        // POST: Commerce/Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLogger(ActivityName = "ویرایش", ModelNameBeingOperated = "استعلام گر")]
        public ActionResult EditInquirier(Customer customer)
        {
            if (ModelState.IsValid)
            {
                var cust = db.Customers.Find(customer.Id);
                cust.InquiryTel = customer.InquiryTel;
                cust.InquiryAddress = customer.InquiryAddress;
                db.Entry(cust).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.ModelOperatedId = customer.Id.ToString();
                return RedirectToAction("IndexInquirier", new { controller="customers", area="commerce"});
            }
            return View("~/areas/commerce/views/customers/EditInquirier.cshtml", customer);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Neshagostar.DAL.DataModel;
using Neshagostar.DAL.DataModel.CallManagement;
using Neshagostar.WebUI.Areas.CallManagement.Models;
using Neshagostar.WebUI.Infrastructure.Filters;

namespace Neshagostar.WebUI.Areas.CallManagement.Controllers
{
    [Authorize(Roles = "secretary, CEO")]
    public class RecievedCallsController : Controller
    {

        const int PAGESIZE = 40;
        private NeshagostarContext db = new NeshagostarContext();

        // GET: CallManagement/RecievedCalls
        public ActionResult Index()
        {
            ViewBag.Departments = db.Departments.Select(d => new SelectListItem
            {
                Text = d.Name,
                Value = d.Id.ToString()
            });

            List<PersianDateTime> days = GetPreviousDays(5);
            List<string> daysTrimmed = new List<string>();
            foreach(PersianDateTime day in days)
            {
                daysTrimmed.Add(day.ToString().Substring(0, 10));
            }

            var dates = new List<SelectListItem>();
            dates.Add(new SelectListItem
            {
                Text = "همه تواریخ",
                Value = ""
            });



            ViewBag.Dates = daysTrimmed.OrderByDescending(d => d).Select(d => new SelectListItem
            {
                Text = d,
                Value = d
            });
      


            // var previousDay = new PersianDateTime(previous);

            var recievedCalls = db.RecievedCalls.Include(r => r.Caller).Include(r => r.Department);

            var toDay = PersianDateTime.Now;
            int numberOfDays = 5;
            string[] daysArray = new string[numberOfDays];
            daysArray[0] = toDay.ToString().Substring(0, 10);

            for(int i = -1; i > (numberOfDays * -1); i--)
            {
                daysArray[i * -1] = toDay.AddDays(i).ToString().Substring(0, 10);
            }

            foreach (string str in daysArray)
            {
                recievedCalls = recievedCalls.Union(recievedCalls.Where(r => r.Date.Equals(str)));
            }
            var result = recievedCalls.OrderByDescending(r => r.Date).ThenByDescending(r => r.Time).ToList();
            ViewBag.PageNumber = Math.Ceiling(decimal.Parse(result.Count.ToString()) / (decimal)PAGESIZE);
            ViewBag.Page = 1;
            ViewBag.PageSize = PAGESIZE;

            result = result.Take(PAGESIZE).OrderByDescending(r => r.Date).ToList();
            int pageRecording = result.Count;

            if(result.Count < PAGESIZE)
            {
                for(int i = 0; i < PAGESIZE - pageRecording; i++)
                {
                    result.Add(new RecievedCall { Id = Guid.Empty });
                }
            }
            return View(result);
        }

        private static List<PersianDateTime> GetPreviousDays(double previousdays)
        {
            double numberOfDayspreviousdays =( previousdays + 1) * -1;
            var today = PersianDateTime.Now.Date;
            List<PersianDateTime> days = new List<PersianDateTime>();
            days.Add(today);
            for (int i = -1; i > numberOfDayspreviousdays; i--)
            {
                days.Add(today.AddDays(i));
            }

            return days;
        }

        // GET: CallManagement/RecievedCalls/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecievedCall recievedCall = db.RecievedCalls.Include(r => r.Department).Include(r => r.Caller).FirstOrDefault(r => r.Id == id);
            if (recievedCall == null)
            {
                return HttpNotFound();
            }
            return View(recievedCall);
        }

        // GET: CallManagement/RecievedCalls/Create
  
        public ActionResult Create()
        {
            
            ViewBag.Departments = db.Departments.Select(d => new SelectListItem
            {
                Text = d.Name,
                Value = d.Id.ToString()
            });
            return View();
        }

        [ActivityLogger(ActivityName = "ثبت", ModelNameBeingOperated = "تماس")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RecievedCallViewModel recievedCall)
        {
            var caller = db.Callers.Where(c => c.Name.Equals(recievedCall.CallerName) && c.Tel.Equals(recievedCall.CallerTel)).FirstOrDefault();
            if(Int32.Parse(recievedCall.Hour) < 10)
            {
                recievedCall.Hour = 0 + recievedCall.Hour;
            }

            if (Int32.Parse(recievedCall.Minute) < 10)
            {
                recievedCall.Minute = 0 + recievedCall.Minute;
            }

            RecievedCall newRecievedCall = new RecievedCall
            {
                Id = Guid.NewGuid(),
                //Date = new PersianDateTime(recievedCall.Year, +"/" + recievedCall.Month + "/" + recievedCall.Day,
                Date = new PersianDateTime(int.Parse(recievedCall.Year), int.Parse(recievedCall.Month), int.Parse(recievedCall.Day)).ToString().Substring(0,10),
                Time = recievedCall.Hour + ":"+ recievedCall.Minute,
                //DateTime = new PersianDateTime(int.Parse(recievedCall.Year), int.Parse(recievedCall.Month), int.Parse(recievedCall.Day), int.Parse(recievedCall.Hour), int.Parse(recievedCall.Minute), 0).ToString(),
                Comments = recievedCall.Comments,
                DepartmentId = recievedCall.DepartmentId
            };
            if (caller == null)
            {
                var callerId = Guid.NewGuid();
                db.Callers.Add(new Caller
                {
                    Id = callerId,
                    Name = recievedCall.CallerName,
                    Comments = recievedCall.CallerComments,
                    Tel = recievedCall.CallerTel,
                });
                db.SaveChanges();
              
                newRecievedCall.CallerId = callerId;
               
            }
            else
            {
                newRecievedCall.CallerId = caller.Id;
            }

            newRecievedCall.IsCallingFromOutside = recievedCall.IsCallingFromOutside;
            db.RecievedCalls.Add(newRecievedCall);

            //string datetime = recievedCall.Year + "//" + recievedCall.Month + "//" + recievedCall.Day + " " + recievedCall.Hour + ":" + recievedCall.Minute;
            db.SaveChanges();
            ViewBag.ModelOperatedId = newRecievedCall.Id.ToString();
            return RedirectToAction("Index");


        }


        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecievedCall recievedCall = db.RecievedCalls.Include(r => r.Caller).Where(r => r.Id == id).FirstOrDefault();
            ViewBag.Departments = db.Departments.Select(d => new SelectListItem
            {
                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = d.Id == recievedCall.DepartmentId
                
            });
            if (recievedCall == null)
            {
                return HttpNotFound();
            }

            return View(recievedCall);
        }

        // POST: CallManagement/RecievedCalls/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ActivityLogger(ActivityName = "ویرایش", ModelNameBeingOperated = "تماس")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RecievedCall recievedCall)
        {
            var caller = db.Callers.Where(c => c.Name.Equals(recievedCall.Caller.Name) && c.Tel.Equals(recievedCall.Caller.Tel)).FirstOrDefault();
            Guid callerId;
            //recievedCall.CallerId = caller.Id;
            //recievedCall.Caller.Id = caller.Id;
           
            // caller has changed so it has to be added
            if (caller == null)
            {
                callerId = Guid.NewGuid();
                db.Callers.Add(new Caller
                {
                    Id = callerId,
                    Name = recievedCall.Caller.Name,
                    Comments = recievedCall.Caller.Comments,
                    Tel = recievedCall.Caller.Tel,
                });
                db.SaveChanges();
                ViewBag.ModelIdBeingOperated = callerId;


            }
            else
            {
                callerId = caller.Id;
            }

            recievedCall.CallerId = callerId;


            if (ModelState.IsValid)
            {
                recievedCall.Caller = null;
                db.Entry(recievedCall).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Departments = db.Departments.Select(d => new SelectListItem
            {
                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = d.Id == recievedCall.DepartmentId

            });
            return View(recievedCall);
        }

        [ActivityLogger(ActivityName = "حذف", ModelNameBeingOperated = "تماس")]
        public ActionResult Delete(Guid? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecievedCall recievedCall = db.RecievedCalls.Include(r => r.Caller).Include(r => r.Department).FirstOrDefault(r => r.Id == id);
            if (recievedCall == null)
            {
                return HttpNotFound();
            }
            return View(recievedCall);
        }

        // POST: CallManagement/RecievedCalls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            RecievedCall recievedCall = db.RecievedCalls.Find(id);
            db.RecievedCalls.Remove(recievedCall);
            db.SaveChanges();
            ViewBag.ModelIdBeingOperated = recievedCall.Id;
            return RedirectToAction("Index");
        }

        //public JsonResult FilterRecievedCalls(string callerName, string departmentId, string date)
        //{

        //    Guid departId;
        //    bool departmentIsEmpty =  Guid.TryParse(departmentId, out departId);
        //    IQueryable<RecievedCall> recievedCalls = db.RecievedCalls;
        //    if (callerName != "")
        //    {
        //        recievedCalls = recievedCalls.Where(r => r.Caller.Name.Contains(callerName));
        //    }
        //    var d = recievedCalls.ToList();
        //    if (departmentId != "")
        //    {
        //        recievedCalls = recievedCalls.Where(r => r.DepartmentId.Equals(departId));
        //    }
        //    if (date != "")
        //    {
        //        string[] modelarray = date.Split('/');
        //        PersianDateTime dateModel = new PersianDateTime(int.Parse(modelarray[0]), int.Parse(modelarray[1]), int.Parse(modelarray[2]));
        //        string dateInModel = dateModel.Date.ToString().Substring(0,10);


        //        recievedCalls = recievedCalls.Where(r => r.Date.Equals(dateInModel));
        //    }

        //    var results = recievedCalls.Select(r => new
        //    {
        //        callerName = r.Caller.Name,
        //        departmentId = r.DepartmentId.ToString(),
        //        departmentName = r.Department.Name,
        //        date = r.Date,
        //        time = r.Time,
        //        id= r.Id
        //    }).OrderByDescending(r => r.date).OrderByDescending(r => r.time).ToList();
        //    return Json(results, JsonRequestBehavior.AllowGet);

        //}

        public JsonResult FilterRecievedCalls(string callerName, string departmentId, string date, int page = 1)
        {

            Guid departId;
            bool departmentIsEmpty = Guid.TryParse(departmentId, out departId);
            IQueryable<RecievedCall> recievedCalls = db.RecievedCalls.Include(r => r.Caller).Include(r => r.Department);
            if (callerName != "")
            {
                recievedCalls = recievedCalls.Where(r => r.Caller.Name.Contains(callerName));
            }
            var d = recievedCalls.ToList();
            if (departmentId != "")
            {
                recievedCalls = recievedCalls.Where(r => r.DepartmentId.Equals(departId));
            }
            if (date != "" && date != "previousDates")
            {
                string[] modelarray = date.Split('/');
                PersianDateTime dateModel = new PersianDateTime(int.Parse(modelarray[0]), int.Parse(modelarray[1]), int.Parse(modelarray[2]));
                string dateInModel = dateModel.Date.ToString().Substring(0, 10);


                recievedCalls = recievedCalls.Where(r => r.Date.Equals(dateInModel));
            }
            recievedCalls = recievedCalls.OrderByDescending(r => r.Date).ThenByDescending(r => r.Time);
            //var x = y.OrderByDescending(r => r.Time).ToList();

            //var newRecievedCalls = recievedCalls.OrderByDescending(r => r.Date).OrderByDescending(r => r.Time).ToList();
            //var s = recievedCalls.ToList();

            var recievedCallsList = recievedCalls.ToList();
            var pageNumbers = Math.Ceiling(recievedCallsList.Count / (decimal)PAGESIZE);
            recievedCallsList = recievedCallsList.Skip((page - 1) * PAGESIZE).Take(PAGESIZE).ToList();

            if(page > pageNumbers)
            {
                page = Int32.Parse((pageNumbers).ToString());
            }
         
            var results = recievedCallsList.Select(r => new
            {
                callerName = r.Caller.Name,
                departmentId = r.DepartmentId.ToString(),
                departmentName = r.Department.Name,
                date = r.Date,
                time = r.Time,
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





        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

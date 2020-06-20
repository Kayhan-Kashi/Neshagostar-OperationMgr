using Neshagostar.DAL.DataModel.PersonnelRelated;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.DataModel.CallManagement
{
    public class RecievedCall
    {
        public Guid Id { get; set; }
        public Guid CallerId { get; set; }
        public Guid DepartmentId { get; set; }
        [Display(Name = "تاریخ تماس ")]
        public string Date { get; set; }
        [Display(Name = "زمان تماس ")]
        public string Time { get; set; }
        [Display(Name = "آیا تماس ورودی است ")]
        public bool IsCallingFromOutside { get; set; }

        [Display(Name = "توضیحات تماس ")]
        public string Comments { get; set; }
        public Caller Caller { get; set; }
        public Department Department { get; set; }
    }
}

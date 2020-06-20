using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Neshagostar.WebUI.Areas.CallManagement.Models
{
    public class RecievedCallViewModel
    {
        public Guid CallerId { get; set; }
        public string CallerName { get; set; }
        public string CallerComments { get; set; }
        public string CallerTel { get; set; }
        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        [Display(Name = "سال تماس ")]
        public string Year { get; set; }
        [Display(Name = "ماه تماس ")]
        public string Month { get; set; }
        [Display(Name = "روز تماس ")]
        public string Day { get; set; }
        [Display(Name = "ساعت تماس ")]
        public string Hour { get; set; }
        [Display(Name = "دقیقه تماس ")]
        public string Minute { get; set; }
        [Display(Name = "آیا تماس ورودی است")]
        public bool IsCallingFromOutside { get; set; }

        [Display(Name = "توضیحات تماس ")]
        public string Comments { get; set; }
 
    }
}
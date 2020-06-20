using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Neshagostar.WebUI.Areas.Production.Models.SaloonRelated
{
    public class SaloonDTO
    {
        public Guid Id { get; set; }

        public Guid ManagerId { get; set; }
        [Display(Name = "نام مدیر")]
        public string ManagerName { get; set; }

        [Display(Name = "نام سالن")]
        public string Name { get; set; }

        [Display(Name = "توضیحات")]
        public string Comments { get; set; }

    }
}
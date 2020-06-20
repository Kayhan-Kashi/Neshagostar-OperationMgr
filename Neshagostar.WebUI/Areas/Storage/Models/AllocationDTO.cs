using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Neshagostar.WebUI.Areas.Storage.Models
{
    public class AllocationDTO
    {
        public Guid Id { get; set; }

        public Guid OrderItemId { get; set; }
        public Guid ImportedProductId { get; set; }

        [Display(Name = "مقدار تخصیص یافته")]
        public double Amount { get; set; }


        [Display(Name = "سفارش")]
        public string CustomerName { get; set; }


        [Display(Name = "تاریخ تخصیص")]
        public string AllocatingDate { get; set; }

        [Display(Name = "تاریخ تولید")]
        public string ProductionDate { get; set; }

    }
}
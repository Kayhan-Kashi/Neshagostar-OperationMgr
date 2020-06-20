using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Neshagostar.WebUI.Areas.Storage.Models
{
    public class ProductStorageDTO
    {
        [Display(Name = "شناسه کالا")]
        public Guid ProductId { get; set; }

        [Display(Name = "نوع کالا")]
        public string ProductName { get; set; }

        [Display(Name = "کد کالا")]
        public string ProductCode { get; set; }

        [Display(Name = "مقدار")]
        public double ExistingAmount { get; set; }

        [Display(Name = "مقدار مانده تخصیص نیافته")]
        public double UnAllocatedAmount { get; set; }

    }
}
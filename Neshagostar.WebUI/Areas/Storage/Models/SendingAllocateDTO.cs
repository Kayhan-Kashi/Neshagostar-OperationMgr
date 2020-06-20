using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Neshagostar.WebUI.Areas.Storage.Models
{
    public class SendingAllocateDTO
    {

        public Guid Id { get; set; }
        public Guid AllocatedProductId { get; set; }

        [Display(Name = "تاریخ تولید")]
        public string DateProducted { get; set; }

        [Display(Name = "مقدار قابل ارسال")]
        public double AvailableAmountToSend { get; set; }

        [Display(Name = "مقدار ارسالی")]
        public double SendingAmount { get; set; }
    }
}
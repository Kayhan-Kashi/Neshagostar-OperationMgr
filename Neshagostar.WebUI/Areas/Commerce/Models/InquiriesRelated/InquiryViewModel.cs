using Neshagostar.DAL.DataModel.CommerceRelated.InquiriesRelated;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Neshagostar.WebUI.Areas.Commerce.Models.InquiriesRelated
{
    public class InquiryViewModel
    {
        public string CustomerId { get; set; }

        [Required(ErrorMessage = "لطفا نام را وارد فرمائید")]
        [Display(Name = "نام مشتری")]
        public string CustomerName { get; set; }
        [Display(Name = "شماره تلفن")]
        public string ContactNumber { get; set; }
        [Display(Name = "آدرس")]
        public string Address { get; set; }
        [Display(Name = "توضیحات")]
        public string Comments { get; set; }
        public bool HasAddedCost { get; set; }
        public double FinalPrice { get; set; }
        public double PriceWithoutAddedCost { get; set; }
        public double addedCostAmount { get; set; }



        public List<InquiryItem> InquiryItems { get; set; }


    }
}
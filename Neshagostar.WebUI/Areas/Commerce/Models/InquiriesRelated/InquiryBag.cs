using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Neshagostar.WebUI.Areas.Commerce.Models.InquiriesRelated
{
    public class InquiryBag
    {
        #region Keys
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid InquiryId { get; set; }
        public Guid ProductId { get; set; }
        #endregion

        public string DateTime { get; set; }
        public bool IsSuccessful { get; set; }
        public string ReasonForFailure { get; set; }
        public string InquiryComments { get; set; }

        public string ProductName { get; set; }
    
        public string CustomerName { get; set; }
        public string CustomerTel { get; set; }
        public string CustomerAddress { get; set; }

        [Display(Name = "مقدار")]
        public double Amount { get; set; }
        [Display(Name = "قیمت به ازای واحد")]
        [DisplayFormat(DataFormatString = "{0:#,##0.##}")]
        public double PricePerUnit { get; set; }
        public double PricePerKilo { get; set; }

        [Display(Name = "توضیحات")]
        public string Comments { get; set; }
        [Display(Name = "وزن به ازای هر واحد")]
        public double NominalWeightPerMeter { get; set; }
        [Display(Name = "قیمت مواد اولیه")]
        [DisplayFormat(DataFormatString = "{0:#,##0.##}")]
        public double HDPEPrice { get; set; }
        [Display(Name = "قیمت واشر")]
        [DisplayFormat(DataFormatString = "{0:#,##0.##}")]
        public double WasherPrice { get; set; }
        [Display(Name = "کل قیمت آیتم")]
        [DisplayFormat(DataFormatString = "{0:#,##0.##}")]
        public double TotalPrice { get; set; }
        [Display(Name = "کل وزن")]
        public double TotalWeight { get; set; }
        [Display(Name = "کل مبلغ سفارش")]
        [DisplayFormat(DataFormatString = "{0:#,##0.##}")]
        public double InquiryTotalPrice { get; set; }
        public decimal PageNumbers { get; set; }
        public int Page { get; set; }
    }
}
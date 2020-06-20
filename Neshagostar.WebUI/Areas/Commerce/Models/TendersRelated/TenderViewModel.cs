using Neshagostar.DAL.DataModel.CommerceRelated.TenderRelated;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Neshagostar.WebUI.Areas.Commerce.Models.TendersRelated
{
    public class TenderViewModel
    {
        public Guid TenderId { get; set; }
        public string CustomerId { get; set; }

        [Display(Name = "تاریخ")]
        public string DateTime { get; set; }

        [Display(Name = "مجموع وزن")]
        [DisplayFormat(DataFormatString = "{0:#,##0.##}")]
        public double TotalWeight { get; set; }

        [Required(ErrorMessage = "لطفا نام را وارد فرمائید")]
        [Display(Name = "نام مشتری")]
        public string CustomerName { get; set; }

        [Display(Name = "عنوان مناقصه")]
        public string TenderTitle { get; set; }

        [Display(Name = "شماره تلفن")]
        public string ContactNumber { get; set; }

        [Display(Name = "آدرس")]
        public string Address { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,##0.##}")]
        public double FinalPrice { get; set; }

        [Display(Name = "مبلغ ضمانت نامه")]
        [DisplayFormat(DataFormatString = "{0:#,##0.##}")]
        public double GuarantyCost { get; set; }

        [Display(Name = "مبلغ حمل")]
        [DisplayFormat(DataFormatString = "{0:#,##0.##}")]
        public double CarriageCost { get; set; }

        [Display(Name = "مبلغ بازرسی")]
        [DisplayFormat(DataFormatString = "{0:#,##0.##}")]
        public double InspectionCost { get; set; }

        [Display(Name = "آیا به خرید منجر شد ؟")]
        public bool IsSuccessful { get; set; }

        [Display(Name = "دلایلی که منجر به برنده نشدن در مناقصه شده است")]
        public string ReasonForFailure { get; set; }

        [Display(Name = "توضیحات")]
        public string Comments { get; set; }

        [Display(Name = "مبلغ برآورد")]
        [DisplayFormat(DataFormatString = "{0:#,##0.##}")]
        public double PredictionPrice { get; set; }

        [Display(Name = "مبلغ ضمانت نامه شرکت در مناقصه")]
        [DisplayFormat(DataFormatString = "{0:#,##0.##}")]
        public double ParticipatingGuarantyPrice { get; set; }

        public List<TenderItem> TenderItems { get; set; }
        public List<TenderPrice> TenderPrices { get; set; }
    }
}
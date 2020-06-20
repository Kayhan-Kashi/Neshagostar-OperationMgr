using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Neshagostar.WebUI.Areas.Commerce.Models.OrdersRelated
{
    public class OrderBag
    {
        #region Keys
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid InquiryId { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        #endregion

        public string DateTime { get; set; }
        public bool IsSuccessful { get; set; }
        public string ReasonForFailure { get; set; }
        [Display(Name = "توضیح در مورد آیتم سفارش")]
        public string OrderItemComments { get; set; }

        public string ProductName { get; set; }

        public string CustomerName { get; set; }
        public string CustomerTel { get; set; }
        public string CustomerCity { get; set; }

        [Display(Name = "مقدار")]
        public double Amount { get; set; }
        [Display(Name = "قیمت به ازای واحد")]
        [DisplayFormat(DataFormatString = "{0:#,##0.##}")]
        public double PricePerUnit { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,##0.##}")]
        public double PricePerKilo { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,##0.##}")]
        [Display(Name = "قیمت کل سفارش")]
        public double PriceSummation { get; set; }


        [Display(Name = "واحد")]
        public string Unit { get; set; }

        [Display(Name = "مقدار ارسال شده")]
        [DisplayFormat(DataFormatString = "{0:#,##0.##}")]
        public double SentAmount { get; set; }

        [Display(Name = "مقدار ارسال نشده")]
        [DisplayFormat(DataFormatString = "{0:#,##0.##}")]
        public double UnSentAmount { get; set; }


        [Display(Name = "توضیحات کل سفارش")]
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

        [Display(Name = "آیا ابلاغ شده است")]
        public bool IsDispatched { get; set; }

        [Display(Name = "تاریخ ابلاغ")]
        public string DateOfDispatch { get; set; }

        [Display(Name = "مقدار ابلاغی")]
        public double AmountDispatched { get; set; }

        [Display(Name = "توضیح در مورد ابلاغ محصول")]
        public string DispatchComments { get; set; }


        [Display(Name = "تاریخ تحویل خواسته شده از سوی مشتری")]
        public string DateToRecieve { get; set; }
    }
}
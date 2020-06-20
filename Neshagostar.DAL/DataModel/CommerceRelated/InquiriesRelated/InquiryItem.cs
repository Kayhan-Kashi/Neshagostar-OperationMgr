using Neshagostar.DAL.DataModel.CommerceRelated.ProductsRelated;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.DataModel.CommerceRelated.InquiriesRelated
{
    public class InquiryItem
    {
        #region Keys
        public Guid Id { get; set; }
        public Guid InquiryId { get; set; }
        public Guid ProductId { get; set; }
        #endregion

        #region Scalar properties
        [Display(Name = "مقدار")]
        public double Amount { get; set; }
        [Display(Name = "قیمت به ازای واحد")]
        public double PricePerUnit { get; set; }
        [Display(Name = "قیمت به ازای کیلو")]
        public double PricePerKilo { get; set; }
        [Display(Name = "آیا به خرید منجر شد ؟")]
        public bool IsSuccessful { get; set; }
        [Display(Name = "دلایلی که منجر به عدم خرید شده است")]
        public string ReasonForFailure { get; set; }

        [Display(Name = "توضیحات")]
        public string Comments { get; set; }
        [Display(Name = "وزن به ازای هر واحد")]
        public double NominalWeightPerMeter { get; set; }
        [Display(Name = "قیمت مواد اولیه")]
        public double HDPEPrice { get; set; }
        [Display(Name = "قیمت واشر")]
        public double WasherPrice { get; set; }
        [Display(Name = "کل قیمت آیتم")]
        public double TotalPrice { get; set; }
        [Display(Name = "کل وزن")]
        public double TotalWeight { get; set; }
        #endregion

        #region Navigational properties
        public Inquiry Inquiry { get; set; }
        public Product Product { get; set; }
        #endregion
    }
}

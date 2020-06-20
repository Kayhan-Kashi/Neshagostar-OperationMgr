using Neshagostar.DAL.DataModel.CommerceRelated.ProductsRelated;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.DataModel.CommerceRelated.TenderRelated
{
    public class TenderItem
    {
        #region Keys
        public Guid Id { get; set; }
        public Guid TenderId { get; set; }
        public Guid ProductId { get; set; }
        #endregion

        #region Scalar properties
        [Display(Name = "مقدار")]
        public double Amount { get; set; }
        [Display(Name = "قیمت به ازای واحد")]
        [DisplayFormat(DataFormatString = "{0:#,##0.##}")]
        public double PricePerUnit { get; set; }
        [Display(Name = "قیمت به ازای کیلو")]
        [DisplayFormat(DataFormatString = "{0:#,##0.##}")]
        public double PricePerKilo { get; set; }

        [Display(Name = "توضیحات")]
        public string Comments { get; set; }
        [Display(Name = "وزن به ازای هر واحد")]
        [DisplayFormat(DataFormatString = "{0:#,##0.##}")]
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
        [DisplayFormat(DataFormatString = "{0:#,##0.##}")]
        public double TotalWeight { get; set; }
        #endregion

        #region Navigational properties
        public Tender Tender { get; set; }
        public Product Product { get; set; }
        #endregion
    }
}

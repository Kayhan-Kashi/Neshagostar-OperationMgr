using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.DataModel.StorageRelated.WastageRelated
{
    public class DispatchedToBeGrindedProduct
    {
        public Guid Id { get; set; }
        public Guid ImportedUnstandardProductId { get; set; }

        [Display(Name = "تاریخ ارسال")]
        public string Date { get; set; }

        [Display(Name = "مقدار ورودی به کیلو")]
        [Range(0, double.MaxValue, ErrorMessage = "مقدار ورودی صحیح نیست")]
        [Required(ErrorMessage = "لطفا مقدار ورودی به کیلو را وارد فرمائید")]
        public double AmountImportedInKilo  {get; set;}

        [Display(Name = "مقدار ورودی به متر")]
        [Range(0, double.MaxValue, ErrorMessage = "مقدار ورودی صحیح نیست")]
        public double AmountImportedInMeter { get; set; }

        public ImportedUnStandardProduct ImportedUnStandardProduct { get; set; }
    }
}

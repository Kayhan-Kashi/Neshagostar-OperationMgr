using Neshagostar.DAL.DataModel.CommerceRelated.OrdersRelated;
using Neshagostar.DAL.DataModel.CommerceRelated.ProductsRelated;
using Neshagostar.DAL.DataModel.StorageRelated.ProductsRelated;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.DataModel.StorageRelated.WastageRelated
{
    public class ImportedUnStandardProduct
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid? OrderItemId { get; set; }
        public Guid? ProductIssueId { get; set; }

        [Display(Name = "تاریخ")]
        [Required(ErrorMessage = "لطفا تاریخ را وارد فرمائید")]
        public string Date { get; set; }

        [Display(Name = "مقدار ورودی به متر")]
        [Range(0, double.MaxValue, ErrorMessage = "مقدار ورودی صحیح نیست")]
        [Required(ErrorMessage = "لطفا مقدار ورودی به متر را وارد فرمائید")]
        public double AmountImportedInMeter { get; set; }

        [Display(Name = "مقدار ورودی به کیلو")]
        [Range(0, double.MaxValue, ErrorMessage = "مقدار ورودی صحیح نیست")]
        [Required(ErrorMessage = "لطفا مقدار ورودی به کیلو را وارد فرمائید")]
        public double AmountImportedInKilo { get; set; }

        [Display(Name = "مقدار ارسالی برای آسیاب به متر")]
        public double AmountInMeterDispatchedToBeGrinded
        {
            get
            {
                if(DispatchedToBeGrindedProducts != null && DispatchedToBeGrindedProducts.Count > 0)
                {
                    double totalAmount = 0;
                    foreach(DispatchedToBeGrindedProduct item in DispatchedToBeGrindedProducts)
                    {
                        totalAmount += item.AmountImportedInMeter;
                    }
                    return totalAmount;
                }
                return AmountImportedInMeter;
            }
        }

        [Display(Name = "مقدار ارسالی برای آسیاب به کیلو")]
        public double AmountInKiloDispatchedToBeGrinded
        {
            get
            {
                if (DispatchedToBeGrindedProducts != null && DispatchedToBeGrindedProducts.Count > 0)
                {
                    double totalAmount = 0;
                    foreach (DispatchedToBeGrindedProduct item in DispatchedToBeGrindedProducts)
                    {
                        totalAmount += item.AmountImportedInKilo;
                    }
                    return totalAmount;
                }
                return AmountImportedInKilo;
            }
        }

        [Display(Name = "مقدار اصلاح شده به متر")]
        public double AmountInMeterIssueSolved
        {
            get
            {
                if (ImportedProducts != null && ImportedProducts.Count > 0)
                {
                    double total = 0;
                    foreach (ImportedProduct item in ImportedProducts)
                    {
                        total += item.AmountImported;
                    }
                    return total;
                }
                return 0;
            }
        }


        [Display(Name = "جزئیات ایراد")]
        public string Issue { get; set; }

        [Display(Name = "توضیحات")]
        public string Comments { get; set; }

        public OrderItem OrderItem { get; set; }
        public Product Product { get; set; }
        public ProductIssue ProductIssue { get; set; }

        public List<ImportedProduct> ImportedProducts { get; set; }
        public List<DispatchedToBeGrindedProduct> DispatchedToBeGrindedProducts { get; set; }
    }
}

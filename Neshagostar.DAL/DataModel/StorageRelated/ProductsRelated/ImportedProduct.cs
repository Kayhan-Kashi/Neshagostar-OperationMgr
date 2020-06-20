using Neshagostar.DAL.DataModel.CommerceRelated.OrdersRelated;
using Neshagostar.DAL.DataModel.CommerceRelated.ProductsRelated;
using Neshagostar.DAL.DataModel.OrdersRelated.ProductsRelated;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.DataModel.StorageRelated.ProductsRelated
{
    public class ImportedProduct
    {
        public Guid Id { get; set; }

        [Display(Name = "نوع کالا")]
        [Required]
        public Guid ProductId { get; set; }
        
        [Display(Name = "تاریخ")]
        [Required(ErrorMessage = "لطفا تاریخ را وارد فرمائید")]
        public string Date { get; set; }

        [Display(Name = "مقدار ورودی")]
        [Range(0, double.MaxValue,ErrorMessage ="مقدار ورودی صحیح نیست")]
        [Required(ErrorMessage = "لطفا مقدار ورودی را وارد فرمائید")]
        public double AmountImported { get; set; }

        [Display(Name = "توضیحات")]
        public string Comments { get; set; }

        [Display(Name = "مقدار تخصیص داده شده")]
        public double AmountAllocated
        {
            get
            {
                double sum = 0;
                if(AllocatedProducts != null && AllocatedProducts.Count > 0)
                {
                    foreach(var allocated in AllocatedProducts)
                    {
                        sum += allocated.Amount;
                    }
                }
                return sum;
            }
        }


        [Display(Name = "مقدار ارسال شده")]
        public double AmountSent
        {
            get
            {
                double sum = 0;
                if (AllocatedProducts != null && AllocatedProducts.Count > 0)
                {
                    foreach (var allocated in AllocatedProducts)
                    {
                        sum += allocated.AmountSent;
                    }
                }
                return sum;
            }
        }


        [Display(Name = "مقدار ارسال نشده")]
        public double AmountUnSent
        {
            get
            {
                return AmountImported - AmountSent;
            }
        }

        [Display(Name = "مقدار تخصیص داده نشده")]
        public double AmountUnAllocated
        {
            get
            {
                return AmountImported - AmountAllocated;
            }
        }


        [Display(Name = "به طور کامل تخصیص یافته")]
        public bool IsAllocatedCompletely
        {
            get
            {
                return AmountUnAllocated == 0;
            }
        }

        [Display(Name = "به طور کامل ارسال شده")]
        public bool IsSentCompletely
        {
            get
            {
                return AmountUnSent == 0;
            }
        }



        public Product Product { get; set; }
        public List<AllocatedProduct> AllocatedProducts { get; set; }


  

    }
}

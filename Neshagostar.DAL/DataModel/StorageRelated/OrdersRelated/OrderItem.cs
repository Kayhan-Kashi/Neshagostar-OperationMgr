using Neshagostar.DAL.DataModel.OrdersRelated.ProductsRelated;
using Neshagostar.DAL.DataModel.StorageRelated.OrdersRelated;
using Neshagostar.DAL.DataModel.StorageRelated.ProductsRelated;
using Neshagostar.DAL.DataModel.StorageRelated.WastageRelated;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.DataModel.CommerceRelated.OrdersRelated
{
    public partial class OrderItem
    {
        

        [Display(Name = "مقدار ارسال شده")]
        public double AmountSent
        {
            get
            {
                double sum = 0;

                if (Sendings != null && Sendings.Count > 0)
                {
                    foreach(var sending in Sendings)
                    {
                        sum += sending.AmountSent;
                    }                         
                }

                return sum;

            }
        }


        [Display(Name = "مقدار ارسال نشده")]
        public double AmountNotSent
        {
            get
            {
                return AmountDispatched - AmountSent;
            }
        }

        [Display(Name = "اتمام ارسال")]
        public bool IsSendingCompleted
        {
            get
            {
                return AmountNotSent == 0;
            }
        }

        [Display(Name = "آخرین تاریخ ارسال")]
        public string LastSendingDate
        {
            get
            {
                if(Sendings != null && Sendings.Count != 0)
                {
                   return Sendings.OrderByDescending(o => o.DateSent).FirstOrDefault().DateSent;
                }
                else
                {
                    return "";
                }
            }
        }

        [Display(Name = "مقدار تخصیص یافته")]
        public double AllocatedAmount
        {
            get
            {
                double sum = 0;
                if (AllocatedProducts != null && AllocatedProducts.Count != 0)
                {
                    foreach(var allocated in AllocatedProducts)
                    {
                        sum += allocated.Amount;
                    }              
                }
                return sum;
            }
        }

        [Display(Name = "مقدار تخصیص نیافته")]
        public double UnAllocatedAmount
        {
            get
            {
                return Amount - AllocatedAmount;
            }
        }

        public bool IsProductionCompleted
        {
            get
            {
                return UnAllocatedAmount == 0;
            }
        }

   

        public List<Sending> Sendings { get; set; }
        public List<AllocatedProduct> AllocatedProducts { get; set; }
        public List<ImportedUnStandardProduct> ImportedUnStandardProducts { get; set; }
    }
}

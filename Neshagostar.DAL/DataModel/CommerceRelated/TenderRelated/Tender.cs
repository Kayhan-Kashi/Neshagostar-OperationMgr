using Neshagostar.DAL.DataModel.CommerceRelated.CustomersRelated;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.DataModel.CommerceRelated.TenderRelated
{
    public class Tender
    {
        #region Keys
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        #endregion

        #region Scalar properties
        public string DateTime { get; set; }

        [Display(Name = "آیا برنده شدیم ؟")]
        public bool IsWinner { get; set; }

        [Display(Name = "عنوان مناقصه")]
        public string Title { get; set; }

        [Display(Name = "مبلغ برآورد")]
        public double PredictionPrice { get; set; }

        [Display(Name = "مبلغ ضمانت نامه شرکت در مناقصه")]
        public double ParticipatingGuarantyPrice { get; set; }

        [Display(Name = "مبلغ ضمانت نامه")]
        public double GuarantyCost { get; set; }
   
        [Display(Name = "مبلغ حمل")]
        public double CarriageCost { get; set; }

        [Display(Name = "مبلغ حمل به ازای کیلو")]
        public double CarriageCostPerKilo
        {
            get
            {
                return CarriageCost / TotalWeight;
            }
        }

        [Display(Name = "قیمت به ازای هر کیلو")]
        public double PricePerKilo
        {
            get
            {
                return FinalPrice / TotalWeight;
            }
        }

        [Display(Name = "مبلغ بازرسی")]
        public double InspectionCost { get; set; }

        [Display(Name = "آیا به خرید منجر شد ؟")]
        public bool IsSuccessful { get; set; }

        [Display(Name = "دلایلی که منجر به برنده نشدن در مناقصه شده است")]
        public string ReasonForFailure { get; set; }

        [Display(Name = "توضیحات")]
        public string Comments { get; set; }

        [Display(Name = "مبلغ نهایی")]
        public double FinalPrice
        {
            get
            {
                return (TenderItemsPriceSummedUp);
            }             
        }

        [Display(Name = "مجموع وزن")]
        public double TotalWeight
        {
            get
            {
                if(TenderItems != null)
                {
                    double sum = 0;
                    foreach (var tenderItem in TenderItems)
                    {
                        sum += tenderItem.TotalWeight;
                    }
                    return sum;
                }
                return 1;
      
            }
        }

        public double FinalPriceWithAddedCost
        {
            get
            {
                return FinalPrice + AddedCostAmount;
            }
        }

        [Display(Name = "مجموع مبلغ سفارشات")]
        public double TenderItemsPriceSummedUp
        {
            get
            {
                if(TenderItems != null)
                {
                    double sum = 0;
                    foreach (var inquiry in TenderItems)
                    {
                        sum += inquiry.TotalPrice;
                    }
                    return sum;
                }
                return 1;
 
            }
        }

        public double AddedCostAmount
        {
            get
            {
                return FinalPrice * 0.09;
            }
        }

        #endregion

        #region Navigational Properties
        public Customer Customer { get; set; }
        public List<TenderItem> TenderItems { get; set; }
        public List<RivalPrice> RivalPrices { get; set; }
        #endregion
    }
}

using Neshagostar.DAL.DataModel.CommerceRelated.CustomersRelated;
using Neshagostar.DAL.DataModel.CommerceRelated.ProductsRelated;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.DataModel.CommerceRelated.InquiriesRelated
{
    public class Inquiry
    {
        #region Keys
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        #endregion

        #region Scalar properties
        public string DateTime { get; set; }
        [Display(Name = "آیا به خرید منجر شد ؟")]
        public bool IsSuccessful { get; set; }
        [Display(Name = "دلایلی که منجر به عدم خرید شده است")]
        public string ReasonForFailure { get; set; }
        [Display(Name = "توضیحات")]
        public string Comments { get; set; }
        [Display(Name = "ارزش افزوده دارد ؟")]
        public bool HasAddedCost { get; set; }
        [Display(Name = "مبلغ نهایی")]
        public double FinalPrice
        {
            get
            {
                if (HasAddedCost)
                {
                    return InquiryItemsPriceSummedUp * 1.09;
                }

                else
                {
                    return InquiryItemsPriceSummedUp;
                }
                
            }
        }

        
        [Display(Name = "مجموع مبلغ سفارشات")]
        public double InquiryItemsPriceSummedUp
        {
            get
            {
                double sum = 0;
                foreach (var inquiry in InquiryItems)
                {
                    sum += inquiry.TotalPrice;
                }
                return sum;
            }
        }

        public double AddedCostAmount { get; set; }

        #endregion

        #region Navigational Properties
        public Customer Customer { get; set; }
        public List<InquiryItem> InquiryItems { get; set; }
        #endregion
    }
}

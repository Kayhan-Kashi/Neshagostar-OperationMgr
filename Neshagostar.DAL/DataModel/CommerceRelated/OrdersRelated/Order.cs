using Neshagostar.DAL.DataModel.CommerceRelated.CustomersRelated;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.DataModel.CommerceRelated.OrdersRelated
{
    public partial class Order
    {

        #region Keys
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid InquiryId { get; set; }
        #endregion

        #region Scalar properties
        [Display(Name = "آیا ارزش افزوده دارد ؟")]
        public bool HasAddedCost { get; set; }

        [Display(Name = "توضیحات سفارش")]
        public string Comments { get; set; }

        [Display(Name= "مجموع قیمت آیتم های سفارش")]
        public double OrderItemsPriceSummation
        {
            get
            {
                if(OrderItems != null)
                {
                    double sum = 0;
                    foreach(var orderItem in OrderItems)
                    {
                        sum += orderItem.TotalPrice;
                    }
                    return sum;
                }
                return 1;
            }
        }

        public double AddedCost
        {
            get
            {
                if(HasAddedCost)
                return OrderItemsPriceSummation * 0.09;
                return 0;
            }
        }

        public double TotalPriceWithAddedCost
        {
            get
            {
                return AddedCost + OrderItemsPriceSummation;
            }
        }

        [Display(Name = "تاریخ سفارش")]
        public string Date { get; set; }
        #endregion




        #region Navigational properties
        public Customer Customer { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        #endregion





    }
}

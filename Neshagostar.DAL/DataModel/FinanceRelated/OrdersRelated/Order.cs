using Neshagostar.DAL.DataModel.FinanceRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.DataModel.CommerceRelated.OrdersRelated
{
    public partial class Order
    {
        public double TotalPayedAmounts
        {
            get
            {
                double sum = 0;
                if(Payments != null)
                {
                    foreach(var item in Payments)
                    {
                        sum += item.Amount;
                    }
                    return sum;
                }
                return 0;
            }
        }

        public double RemainingAmount
        {
            get
            {
                return TotalPriceWithAddedCost - TotalPayedAmounts;
            }
        }

        public double RemainingAmountToPayFromSentItems
        {
            get
            {
                double val = TotalSentItemsValues - TotalPayedAmounts;
                if(val < 0 )
                {
                    return 0;
                }
                return val;
            }
        }

        public double TotalSentItemsValues
        {
            get
            {
                double sum = 0;
                if (OrderItems != null)
                {
                    foreach (var orderItem in OrderItems)
                    {
                        var sentAmount = orderItem.AmountSent;
                        var fee = orderItem.PricePerUnit;
                        sum += (sentAmount * fee);
                    }
                    return sum;
                }
                return sum;
            }
        }

        public string LastPayingDate
        {
            get
            {
                if(Payments != null && Payments.Count > 0)
                {
                    return Payments.OrderByDescending(o => o.Date).FirstOrDefault().Date;
                }
                return "";
            }
        }

        public bool IsPayingCompleted
        {
            get
            {
                return TotalPriceWithAddedCost - TotalPayedAmounts == 0;
            }
        }

        #region Navigational properties
        public List<Payment> Payments { get; set; }
        #endregion
    }
}

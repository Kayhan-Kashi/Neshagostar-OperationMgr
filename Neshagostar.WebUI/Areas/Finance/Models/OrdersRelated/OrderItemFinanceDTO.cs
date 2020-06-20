using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Neshagostar.WebUI.Areas.Finance.Models.OrdersRelated
{
    public class OrderItemFinanceDTO
    {
        public Guid OrderId { get; set; }
        public Guid OrderItemId { get; set; }
        public Guid ProductId { get; set; }
        public Guid CustomerId { get; set; }

        public string CustomerTel { get; set; }
        public string CustomerCity { get; set; }

        public string OrderDate { get; set; }

        public string CustomerName { get; set; }
        public string ProductName { get; set; }

        public string Unit { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,##0.##}")]
        public double OrderItemAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,##0.##}")]
        public double OrderItemSentAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,##0.##}")]
        public double OrderItemUnsentAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,##0.##}")]
        public double PricePerUnit { get; set; }


        public bool IsPayingCompleted { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,##0.##}")]
        public double OrderItemTotalPrice { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,##0.##}")]
        public double SentAmountTotalPrice { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,##0.##}")]
        public double UnsentAmountTotalPrice { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,##0.##}")]
        public double OrderTotalPrice { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,##0.##}")]
        public double OrderPayedAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,##0.##}")]
        public double OrderRemainingToPayAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,##0.##}")]
        public double OrderRemainingAmountToPayFromSentItems { get; set; }

        public string LastPayingDate { get; set; }

        public bool HasAddedCost { get; set; }  

        public int Page { get; set; }
        public decimal PageNumbers { get; set; }

        public List<PaymentDTO> Payments { get; set; }


 
    }
}
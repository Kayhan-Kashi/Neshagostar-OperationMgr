using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Neshagostar.WebUI.Areas.Finance.Models.OrdersRelated
{
    public class PaymentDTO
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid PaymentMethodId { get; set; }

        public string CustomerName { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,##0.##}")]
        public double Amount { get; set; }
        public string Date { get; set; }
        public string Comments { get; set; }
        public string PaymentMethodTitle { get; set; }

        public string CheckDateToCash { get; set; }
        public bool IsCheckCashed { get; set; }

        public int Page { get; set; }
        public decimal PageNumbers { get; set; }
    }
}
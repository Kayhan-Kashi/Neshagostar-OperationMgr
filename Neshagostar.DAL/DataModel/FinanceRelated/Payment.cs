using Neshagostar.DAL.DataModel.CommerceRelated.CustomersRelated;
using Neshagostar.DAL.DataModel.CommerceRelated.OrdersRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.DataModel.FinanceRelated
{
    public class Payment
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid PaymentMethodId { get; set; }

        public double Amount { get; set; }
        public string Date { get; set; }
        public string Comments { get; set; }

        public string CheckDateToCash { get; set; }
        public bool IsCheckCashed { get; set; }

        public Order Order { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}

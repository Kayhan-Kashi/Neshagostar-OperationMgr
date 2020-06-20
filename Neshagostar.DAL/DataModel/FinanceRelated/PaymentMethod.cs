using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.DataModel.FinanceRelated
{
    public class PaymentMethod
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        
        public List<Payment> Payments { get; set; }
    }
}

using Neshagostar.DAL.DataModel.OrdersRelated.ProductsRelated;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.DataModel.StorageRelated.OrdersRelated
{
    public class SendingAllocated
    {
        public Guid Id { get; set; }
        public Guid AllocatedProductId { get; set; }
        public Guid SendingId { get; set; }

        [Display(Name = "مقدار ارسالی")]
        public double AmountSent {get;set;}

        public AllocatedProduct AllocatedProduct { get; set; }
        public Sending Sending { get; set; }
    }
}

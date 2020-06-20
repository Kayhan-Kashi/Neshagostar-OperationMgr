using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.DataModel.CommerceRelated.ProductsRelated
{
    public class RingStiffness
    {
        public Guid Id { get; set; }
        [Display(Name ="عنوان")]
        public string Description { get; set; }
        [Display(Name = "کد")]
        public string Code { get; set; }
        public List<Product> Products { get; set; }
    }
}

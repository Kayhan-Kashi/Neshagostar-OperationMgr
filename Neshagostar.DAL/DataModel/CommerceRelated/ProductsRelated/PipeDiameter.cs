using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.DataModel.CommerceRelated.ProductsRelated
{
    public class PipeDiameter
    {
        public Guid Id { get; set; }
        [Display(Name ="اندازه قطر")]
        public string Size { get; set; }
        [Display(Name = "کد")]
        public string Code { get; set; }
        public List<ProductGenre> Products { get; set; }
    }
}

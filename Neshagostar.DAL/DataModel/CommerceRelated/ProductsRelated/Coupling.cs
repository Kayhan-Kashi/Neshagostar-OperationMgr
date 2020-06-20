using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.DataModel.CommerceRelated.ProductsRelated
{
    public class Coupling
    {
        public Guid Id { get; set; }
        [Display(Name = "کوپلر دارد ؟")]
        public string HasCoupling { get; set; }
        [Display(Name = "توضیحات")]
        public string Comments { get; set; }
        [Display(Name = "کد")]
        public string Code { get; set; }

        public List<Product> Products { get; set; }
    }
}

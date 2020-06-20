using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.DataModel.ProductionRelated
{
    public class Device
    {
        public Guid Id { get; set; }

        public Guid SaloonId { get; set; }

        [Display(Name = "نام دستگاه")]
        public string Name { get; set; }

        [Display(Name = "توضیحات")]
        public string Comments { get; set; }

        [Display(Name = "کد")]
        public string Code { get; set; }

        public Saloon Saloon { get; set; }
        public List<ProductionLine> ProductionLines { get; set; }
    }
}

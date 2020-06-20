using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.DataModel.ProductionRelated
{
    public class Saloon
    {
        public Guid Id { get; set; }

        public Guid ManagerId { get; set; }

        [Display(Name = "نام سالن")]
        public string Name { get; set; }

        [Display(Name = "توضیحات")]
        public string Comments { get; set; }

        [Display(Name = "کد")]
        public string Code { get; set; }

        public List<Device> Devices { get; set; }
    }
}

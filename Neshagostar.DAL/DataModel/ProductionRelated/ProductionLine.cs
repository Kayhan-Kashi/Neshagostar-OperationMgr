using Neshagostar.DAL.DataModel.CommerceRelated.ProductsRelated;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.DataModel.ProductionRelated
{
    public class ProductionLine
    {
        public Guid Id { get; set; }
        public Guid DeviceId { get; set; }
        public Guid ProductGenreId { get; set; }

        [Display(Name = "نوع خط")]
        public string Description { get; set; }
        [Display(Name = "توضیحات")]
        public string Comments { get; set; }
        [Display(Name = "کد")]
        public string Code { get; set; }

        public Device Device { get; set; }
        public ProductGenre ProductGenre { get; set; }
        public List<DailyProductionReport> DailyProductionReports { get; set; }
    }
}

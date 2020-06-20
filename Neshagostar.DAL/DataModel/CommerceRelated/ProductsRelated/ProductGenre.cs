using Neshagostar.DAL.DataModel.ProductionRelated;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.DataModel.CommerceRelated.ProductsRelated
{
    public class ProductGenre
    {
        public Guid Id { get; set; }
        public Guid ProductCategoryId { get; set; }
        public Guid PipeProfileId { get; set; }
        public Guid PipeDiameterId { get; set; }

        [Display(Name = "عنوان ")]
        public string Name { get; set; }

        [Display(Name = "کد ")]
        public string Code { get; set; }

        public ProductCategory ProductCategory { get; set; }
        public PipeDiameter PipeDiameter { get; set; }
        public PipeProfile PipeProfile { get; set; }
        public List<Product> Products { get; set; }
        public List<DailyProductionReport> DailyProductionReports { get; set; }
    }
}

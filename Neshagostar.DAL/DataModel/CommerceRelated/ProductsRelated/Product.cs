using Neshagostar.DAL.DataModel.CommerceRelated.InquiriesRelated;
using Neshagostar.DAL.DataModel.CommerceRelated.OrdersRelated;
using Neshagostar.DAL.DataModel.ProductionRelated;
using Neshagostar.DAL.DataModel.StorageRelated.WastageRelated;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.DataModel.CommerceRelated.ProductsRelated
{
    public class Product
    {
        #region Keys
        public Guid Id { get; set; }
        public Guid ProductGenreId { get; set; }
        public Guid RingStiffnessId { get; set; }
        public Guid CouplingId { get; set; }
        
        #endregion

        #region Scalar properties
        [Display(Name = "کد محصول")]
        public string Code { get; set; }
        [Display(Name = "عنوان محصول")]
        public string Title { get; set; }
        #endregion

        #region Navigational Properties
        public ProductGenre ProductGenre { get; set; }
        public RingStiffness RingStiffness { get; set; }
        public Coupling Coupling { get; set; }
        public List<InquiryItem> InquiryItems { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public List<ImportedUnStandardProduct> ImportedUnStandardProducts { get; set; }
        //public List<DailyProductionReport> DailyProductionReports { get; set; }
        #endregion

    }
}

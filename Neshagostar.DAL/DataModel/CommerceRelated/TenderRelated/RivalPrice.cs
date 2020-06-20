using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.DataModel.CommerceRelated.TenderRelated
{
    public class RivalPrice
    {
        #region Keys
        public Guid Id { get; set; }
        public Guid RivalId { get; set; }
        public Guid TenderId { get; set; }
        #endregion

        #region Scalar properties
        [Display(Name = "قیمت پیشنهادی")]
        [DisplayFormat(DataFormatString = "{0:#,##0.##}")]
        public double Price { get; set; }
        #endregion

        #region Navigational properties
        public Rival Rival { get; set; }
        public Tender Tender { get; set; }
        #endregion

    }
}

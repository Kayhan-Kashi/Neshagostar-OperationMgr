using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.DataModel.StorageRelated.MaterialRelated
{
    public class MaterialImported
    {
        public Guid Id { get; set; }
        public Guid MaterialPurchasedId { get; set; }

        [Display(Name = "مقدار ورودی")]
        public double AmountImported { get; set; }
        [Display(Name = "تاریخ حمل")]
        public string DateOfImport { get; set; }
        [Display(Name = "شماره بارنامه")]
        public string CarriageNumber { get; set; }
        [Display(Name = "مبدا بارگیری")]
        public string Location { get; set; }
        [Display(Name = "نام راننده")]
        public string DriverName { get; set; }
        [Display(Name = "هزینه حمل")]
        public double CarriagePrice { get; set; }
        [Display(Name = "مجموع هزینه های جانبی")]
        public double OtherCost { get; set; }
        [Display(Name = "توضیحات")]
        public string Comments { get; set; }
        [Display(Name = "مقدار کسری بار")]
        public double LostAmount { get; set; }

        public MaterialPurchased MaterialPurchased { get; set; }
    }
}

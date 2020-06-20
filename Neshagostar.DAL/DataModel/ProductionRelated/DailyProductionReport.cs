using Neshagostar.DAL.DataModel.CommerceRelated.ProductsRelated;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.DataModel.ProductionRelated
{
    public class DailyProductionReport
    {
        public Guid Id { get; set; }
        [Display(Name = "خط تولید")]
        public Guid ProductionLineId { get; set; }
        [Display(Name = "محصول تولیدی")]
        public Guid ProductId { get; set; }

        [Display(Name = "تاریخ تولید")]
        public string DateOfProduction { get; set; }
        [Display(Name = "تاریخ ارائه گزارش")]
        public string DateOfReport { get; set; }

        [Display(Name = "مقدار تولید منطبق")]
        public double AmountOfAcceptedProduct { get; set; }
        [Display(Name = "مقدار تولید نامنطبق")]
        public double AmountOfUnacceptedProduct { get; set; }

        [Display(Name = "میانگین وزن در هر کیلو")]
        public double WeightInUnit { get; set; }

        [Display(Name = "میزان مواد نو مصرفی")]
        public double OrdinaryMaterialConsumedAmount { get; set; }
        [Display(Name = "میزان گرانول مصرفی")]
        public double GranuleMaterialConsumedAmount { get; set; }
        [Display(Name = "میزان مستربچ مشکی مصرفی")]
        public double BlackMasterbatchConsumedAmount { get; set; }
        [Display(Name = "میزان مستربچ زرد مصرفی")]
        public double YellowMasterbatchConsumedAmount { get; set; }
        [Display(Name = "میزان آسیابی مصرفی")]
        public double RecycledConsumedAmount { get; set; }

        [Display(Name = "میزان کل مواد مصرفی")]
        public double AllMaterialsConsumedAmountInKilo { get; set; }

        [Display(Name = "میزان ضایعات تولیدی به کیلو")]
        public double WastageProducedInKilo { get; set; }

        public ProductionLine ProductionLine { get; set; }
        public Product Product { get; set; }
    }
}

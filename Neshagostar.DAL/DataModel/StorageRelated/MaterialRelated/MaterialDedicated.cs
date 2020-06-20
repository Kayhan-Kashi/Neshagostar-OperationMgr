using Neshagostar.DAL.DataModel.ProductionRelated;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.DataModel.StorageRelated.MaterialRelated
{
    public class MaterialDedicated
    {
        public Guid Id { get; set; }
        public Guid MaterialPurchasedId { get; set; }
        public Guid DeviceId { get; set; }


        [Display(Name = "تاریخ تحویل")]
        public string DateOfDedication { get; set; }
        [Display(Name = "مقدار تحویلی")]
        public double AmountDedicated { get; set; }

        [Display(Name = "تاریخ برگشت")]
        public string DateOfWithdrawal { get; set; }
        [Display(Name = "مقدار برگشتی")]
        public double AmountWithdrawed { get; set; }

        [Display(Name = "توضیحات تحویل")]
        public string DedicationComments { get; set; }
        [Display(Name = "توضیحات برگشت مواد")]
        public string WithdrawalComments { get; set; }

        [Display(Name = "مقدار واقعی مصرف شده")]
        public double RealConsumedAmount
        {
            get
            {
                return AmountDedicated - AmountWithdrawed;
            }
        }

        public MaterialPurchased MaterialPurchased { get; set; }
        public Device Device { get; set; }

    }
}

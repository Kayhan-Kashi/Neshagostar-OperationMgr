using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Neshagostar.WebUI.Areas.Commerce.Models.TendersRelated
{
    public class TenderPrice
    {
        public Guid Id { get; set;
        }
        [Display(Name ="نام شرکت کننده")]
        public string ParticipantName { get; set; }

        [Display(Name = "عنوان مناقصه")]
        public string TenderTitle { get; set; }

        [Display(Name = "شناسه مناقصه")]
        public Guid TenderId { get; set; }

        [Display(Name = "محل")]
        public string Place { get; set; }

        [Display(Name = "قیمت پیشنهادی")]
        [DisplayFormat(DataFormatString = "{0:#,##0.##}")]
        public double TotalPrice { get; set; }

        [Display(Name = "قیمت به ازای هر کیلو")]
        [DisplayFormat(DataFormatString = "{0:#,##0.##}")]
        public double PricePerKilo
        {
            get
            {
                return Math.Ceiling(TotalPrice / TotalWeight);
            }
        }

        [Display(Name = "مجموع وزن")]
        [DisplayFormat(DataFormatString = "{0:#,##0.##}")]
        public double TotalWeight { get; set; }

        [Display(Name = "آیا قیمت شرکت خودمان است ؟")]
        public bool IsOurCompany { get; set; }


        [Display(Name = "رتبه قیمت")]
        public int RankingNumber { get; set; }

    }
}
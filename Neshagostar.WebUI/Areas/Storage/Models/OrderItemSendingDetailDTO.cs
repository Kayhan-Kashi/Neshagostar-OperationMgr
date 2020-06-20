using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Neshagostar.WebUI.Areas.Storage.Models
{
    public class OrderItemSendingDetailDTO
    {
        public Guid Id { get; set; }
        public Guid OrderItemId { get; set; }

        [Display(Name = "نام مشتری")]
        public string CustomerName { get; set; }

        [Display(Name = "نام محصول")]
        public string ProductName { get; set; }

        [Display(Name = "تاریخ ارسال بار")]
        public string DateSent { get; set; }

        [Display(Name = "تاریخ دریافت بار")]
        public string DateArrived { get; set; }

        [Display(Name = "مقدار ارسال شده")]
        public double SendingAmount { get; set; }

        [Display(Name = "مقصد بار")]
        public string Destination { get; set; }

        [Display(Name = "شماره بارنامه")]
        public string CarrierNumberCode { get; set; }

        [Display(Name = "اسم راننده")]
        public string DriverName { get; set; }

        [Display(Name = "شماره راننده")]
        public string DriverTel { get; set; }

        [Display(Name = "وزن باسکول")]
        public string BasculeWeight { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,##0.##}")]
        [Display(Name = "کرایه")]
        public double Price { get; set; }

        [Display(Name = "توضیحات")]
        public string Comments { get; set; }

        [Display(Name = "واحد")]
        public string Unit { get; set; }

        public int Page { get; set; }
        public decimal PageNumbers { get; set; }

        public List<SendingAllocateDTO> SendingAllocateds { get; set; }


    }
}
using Neshagostar.DAL.DataModel.CommerceRelated.OrdersRelated;
using Neshagostar.DAL.DataModel.OrdersRelated.ProductsRelated;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.DataModel.StorageRelated.OrdersRelated
{
    public class Sending
    {
        public Guid Id { get; set; }
        public Guid OrderItemId { get; set; }

        [Display(Name = "مقدار ارسال شده")]
        public double AmountSent
        {
            get
            {
                double sum = 0;
                if (SendingAllocateds != null && SendingAllocateds.Count > 0)
                {
                    foreach(var sendAlloc in SendingAllocateds)
                    {
                        sum += sendAlloc.AmountSent;
                    }
                    
                }
                return sum;
            }
        }

        [Display(Name = "تاریخ ارسال بار")]
        public string DateSent { get; set; }

        [Display(Name = "تاریخ دریافت بار")]
        public string DateArrived { get; set; }

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

        public OrderItem OrderItem { get; set; }
        public List<SendingAllocated> SendingAllocateds { get; set; }
    }
}

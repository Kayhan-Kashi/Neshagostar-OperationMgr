using Neshagostar.DAL.DataModel.CommerceRelated.OrdersRelated;
using Neshagostar.DAL.DataModel.StorageRelated.OrdersRelated;
using Neshagostar.DAL.DataModel.StorageRelated.ProductsRelated;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.DataModel.OrdersRelated.ProductsRelated
{
    public class AllocatedProduct
    {
        private double _amount;

        public Guid Id { get; set; }
        public Guid ImportedProductId { get; set; }
        public Guid OrderItemId { get; set; }



        [Display(Name = "مقدار تخصیص")]
        [Range(0, double.MaxValue, ErrorMessage = "مقدار ورودی صحیح نیست")]
        [Required(ErrorMessage = "لطفا مقدار ورودی را وارد فرمائید")]
        public double Amount
        {
            get
            {
                return _amount;
            }
            set
            {
                if (value < AmountSent)
                {
                    _amount = AmountSent;
                    return;
                }

                _amount = value;
            }
        }

        [Display(Name = "تاریخ")]
        [Required(ErrorMessage = "لطفا تاریخ را وارد فرمائید")]
        public string Date { get; set; }

        [Display(Name = "مقدار ارسال شده")]

        public double AmountSent
        {
            get
            {
                double sum = 0;
                if (SendingAllocateds != null && SendingAllocateds.Count > 0)
                {
                    foreach(var sent in SendingAllocateds)
                    {
                        sum += sent.AmountSent;
                    }
                }
                return sum;
            }
        }

        [Display(Name = "مقدار ارسال نشده")]
        public double AmountUnSent
        {
            get
            {
                return Amount - AmountSent;
            }
        }

        [Display(Name = "ارسال کامل")]
        public bool IsSentCompletely
        {
            get
            {
                return AmountSent == Amount;
            }
        }

        public ImportedProduct ImportedProduct { get; set; }
        public List<SendingAllocated> SendingAllocateds { get; set; }
        public OrderItem OrderItem { get; set; }

    }
}

using Neshagostar.DAL.DataModel.CommerceRelated.ProductsRelated;
using Neshagostar.DAL.DataModel.OrdersRelated.ProductsRelated;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Neshagostar.WebUI.Areas.Storage.Models
{
    public class ImportedProductDTO
    {
        public Guid Id { get; set; }

        [Display(Name = "نوع کالا")]
        [Required]
        public Guid ProductId { get; set; }

        [Display(Name = "تاریخ")]
        [Required(ErrorMessage = "لطفا تاریخ را وارد فرمائید")]
        public string Date { get; set; }

        [Display(Name = "مقدار ورودی")]
        [Range(0, double.MaxValue, ErrorMessage = "مقدار ورودی صحیح نیست")]
        [Required(ErrorMessage = "لطفا مقدار ورودی را وارد فرمائید")]
        public double AmountImported { get; set; }

        [Display(Name = "توضیحات")]
        public string Comments { get; set; }

        [Display(Name = "مقدار تخصیص داده شده")]


        public double AmountAllocated { get; set; }

        [Display(Name = "مقدار ارسال شده")]
        public double AmountSent { get; set; }

        [Display(Name = "مقدار ارسال نشده")]
        public double AmountUnSent { get; set; }

        public double AmountUnAllocated { get; set; }


        public bool IsAllocatedCompletely { get; set; }

        public bool IsSentCompletely { get; set; }

        public string ProductName { get; set; }



        public int Page { get; set; }
        public decimal PageNumbers { get; set; }


    }
}
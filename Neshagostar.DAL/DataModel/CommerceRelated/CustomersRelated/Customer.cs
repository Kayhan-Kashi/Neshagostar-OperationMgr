using Neshagostar.DAL.DataModel.CommerceRelated.InquiriesRelated;
using Neshagostar.DAL.DataModel.CommerceRelated.OrdersRelated;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.DataModel.CommerceRelated.CustomersRelated
{
    public class Customer
    {
        public System.Guid Id { get; set; }
        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا نام را وارد فرمائید")]
        public string Name { get; set; }
        [Display(Name = "شماره موبایل")]
        public string MobileNumber { get; set; }
        [Display(Name = "شماره تلفن")]
        public string TelephoneNumber { get; set; }
        [Display(Name = "شهر")]
        public string City { get; set; }
        [Display(Name = "آدرس")]
        public string Address { get; set; }
        [Display(Name = "تاریخ")]
        public string Date { get; set; }
        [Display(Name = "توضیحات")]
        public string Comments { get; set; }
        [Display(Name = "آدرس استعلام")]
        public string InquiryAddress { get; set; }
        [Display(Name = "تلفن استعلام")]
        public string InquiryTel { get; set; }


        public List<Inquiry> Inquiries { get; set; }
        public List<Order> Orders { get; set; }
    }
}

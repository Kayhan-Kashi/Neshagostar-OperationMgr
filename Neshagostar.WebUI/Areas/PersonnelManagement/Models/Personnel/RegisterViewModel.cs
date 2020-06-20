using Neshagostar.DAL.DataModel.PersonnelRelated;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Neshagostar.WebUI.Areas.PersonnelManagement.Models
{
    public class RegisterViewModel
    {

        [Required(ErrorMessage = "نام را وارد فرمائید")]
        [Display(Name="نام")]
        public string Name { get; set; }

        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "نام کاربری را وارد فرمائید")]
        public string UserName { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "لطفا رمز عبور را وارد فرمائید")]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "باید بیشتر از 3 حرف باشد", MinimumLength = 3)]
        public string Password { get; set; }

        [Display(Name = "تکرار رمز عبور")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "لطفا رمز عبور را صحیح تکرار کنید ")]
        public string ConfirmPass { get; set; }

        [Display(Name = "نام بخش")]
        public string DepartmentName { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
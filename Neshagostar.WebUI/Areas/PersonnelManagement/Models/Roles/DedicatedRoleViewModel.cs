using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Neshagostar.WebUI.Areas.PersonnelManagement.Models.Roles
{
    public class DedicatedRoleViewModel
    {
        public string PersonnelId { get; set; }
        [Display(Name = "نام پرسنل")]
        public string PersonName { get; set; }
        public string RoleId { get; set; }
        [Display(Name = "Role name")]
        public string RoleName { get; set; }
    }
}
using Neshagostar.DAL.DataModel.PersonnelRelated;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Neshagostar.WebUI.Areas.PersonnelManagement.Models.Roles
{
    public class RoleViewModel
    {
        public RoleViewModel() { }

        public RoleViewModel(PersonnelRole role)
        {
            Id = role.Id;
            Name = role.Name;
            Description = role.Description;
        }

        public string Id { get; set; }
        [Display(Name="عنوان به لاتین و خلاصه شده")]
        public string Name { get; set; }
        [Display(Name = "عنوان به فارسی و کامل")]
        public string Description { get; set; }
    }
}
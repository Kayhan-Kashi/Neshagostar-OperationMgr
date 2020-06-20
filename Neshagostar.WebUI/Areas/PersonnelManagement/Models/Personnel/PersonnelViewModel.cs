using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neshagostar.WebUI.Areas.PersonnelManagement.Models.Personnel
{
    public class PersonnelViewModel
    {
        public Guid Id { get; set; }
        public Guid DepartmentId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string DeprtmentName { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neshagostar.WebUI.Areas.PersonnelManagement.Models.JustifyRelated
{
    public class JustifyDTO
    {
        public Guid Id { get; set; }
        public string PersonnelId { get; set; }
        public string PersonnelName { get; set; }

        public string Action { get; set; }
        public string Entity { get; set; }
        public string Reason { get; set; }
        public string Comments { get; set; }
    }
}
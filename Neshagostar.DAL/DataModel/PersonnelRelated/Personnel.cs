using Microsoft.AspNet.Identity.EntityFramework;
using Neshagostar.DAL.DataModel.ActivityLogging.PersonnelActivity;
using Neshagostar.DAL.DataModel.ActivityTracker.PersonnelActivity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.DataModel.PersonnelRelated
{
    public class Personnel : IdentityUser
    {
        public Guid DepartmentId { get; set; }
        [Required(ErrorMessage = "نام")]
        public string Name { get; set; }
        public Department Department { get; set; }
        public List<PersonnelRole> PersonnelRoles { get; set; }
        public List<ActivityLog> ActivityLogs { get; set; }
        public List<Justify> Justifies { get; set; }
    }
}

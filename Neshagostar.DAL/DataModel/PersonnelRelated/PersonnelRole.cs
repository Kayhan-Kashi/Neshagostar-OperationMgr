using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.DataModel.PersonnelRelated
{
    public class PersonnelRole : IdentityRole
    {
        public PersonnelRole() : base() { }
        public PersonnelRole(string roleName) : base(roleName) { }

        public string Description { get; set; }
    }
}

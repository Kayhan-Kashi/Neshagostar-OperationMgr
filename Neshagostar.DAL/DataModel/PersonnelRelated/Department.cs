using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.DataModel.PersonnelRelated
{
    public class Department
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Personnel> Personnels { get; set; }
    }
}

using Neshagostar.DAL.DataModel.PersonnelRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.DataModel.ActivityLogging.PersonnelActivity
{
    public class Justify
    {
        #region Keys
        public Guid Id { get; set; }
        public string PersonnelId { get; set; }
        #endregion

        public string Action { get; set; }
        public string Entity { get; set; }
        public string Reason { get; set; }
        public string Comments { get; set; }

        #region Navigational properties
        public Personnel Personnel { get; set; }
        #endregion
    }
}

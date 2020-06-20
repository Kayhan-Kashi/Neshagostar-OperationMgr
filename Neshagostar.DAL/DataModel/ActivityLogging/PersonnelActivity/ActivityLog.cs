using Neshagostar.DAL.DataModel.PersonnelRelated;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.DataModel.ActivityTracker.PersonnelActivity
{
    public class ActivityLog
    {

        #region Keys
        public Guid Id { get; set; }
        public string PersonnelId { get; set; }
        #endregion

        #region Scalar properties
        public string ControllerName { get; set; }
        public string ActionMethodName { get; set; }
        public string ActivityName { get; set; }
        public string ModelNameBeingOperated { get; set; }
        public string ModelIdBeingOperated { get; set; }
        public string PersonnelName { get; set; }
        public string DateTime { get; set; }
        public string Url { get; set; }

        #endregion


        #region Navigational properties
        public Personnel Personnel { get; set; }
        #endregion


    }
}

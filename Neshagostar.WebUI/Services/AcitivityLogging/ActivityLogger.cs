using Neshagostar.DAL.DataModel;
using Neshagostar.DAL.DataModel.ActivityTracker.PersonnelActivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Neshagostar.WebUI.Services.AcitivityLogging
{
    public class ActivityLogger
    {
        private string controllerName;
        private string activityName;
        private string actionMethodName;
        private string modelBeingOperated;
        private string userId;
        private string personnelName;
        private string url;
        private string operatedModelId;

        //private JavaScriptSerializer js = new JavaScriptSerializer();
        private NeshagostarContext context = new NeshagostarContext();

        public ActivityLogger(string activityName, string modelBeingOperated, string ctrlName, string actionMethodName, string usId, string nameOfUser, string operatedModelId, string url)
        {
            controllerName = ctrlName;
            this.actionMethodName = actionMethodName;
            this.modelBeingOperated = modelBeingOperated;
            this.activityName = activityName;
            userId = usId;
            personnelName = nameOfUser;
            this.url = url;
            this.operatedModelId = operatedModelId;
        }

        public void Save()
        {
            context.ActivityLogs.Add(CreateActivityLog());
            context.SaveChanges();
        }

        private ActivityLog CreateActivityLog()
        {

            return new ActivityLog
            {
                Id = Guid.NewGuid(),
                ActivityName = activityName,
                ControllerName = controllerName,
                ModelNameBeingOperated = modelBeingOperated,
                ActionMethodName = actionMethodName,
                DateTime = PersianDateTime.Now.ToString(),
                PersonnelId = userId,
                PersonnelName = personnelName,
                Url = url,
                ModelIdBeingOperated = operatedModelId
            };
        }
    }
}
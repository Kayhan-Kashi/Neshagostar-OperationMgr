using Neshagostar.WebUI.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Neshagostar.WebUI.Services.AcitivityLogging;
using Microsoft.AspNet.Identity;

namespace Neshagostar.WebUI.Infrastructure.Filters
{
    public class ActivityLoggerAttribute : ActionFilterAttribute
    {
        public string ActivityName { get; set; }
        public string ModelNameBeingOperated { get; set; }


        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            string controllerName = filterContext.RequestContext.RouteData.Values["Controller"] as string;
            string actionMethodName = filterContext.RequestContext.RouteData.Values["Action"] as string;
            var personnelMgr = filterContext.HttpContext.Request.GetOwinContext().GetUserManager<PersonnelManager>();
            var user = personnelMgr.FindByName(filterContext.HttpContext.User.Identity.Name);
            var url = filterContext.HttpContext.Request.Url.PathAndQuery;
            var viewBag = filterContext.Controller.ViewBag;
            string modelOperatedId = viewBag.ModelOperatedId as string;
            ActivityLogger activityLogger = new ActivityLogger(ActivityName, ModelNameBeingOperated, controllerName, actionMethodName, user.Id, user.Name, viewBag.ModelOperatedId as string, url);
            activityLogger.Save();
            base.OnActionExecuted(filterContext);
        }
    }
}
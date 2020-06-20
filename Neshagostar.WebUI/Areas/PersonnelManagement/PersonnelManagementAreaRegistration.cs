using System.Web.Mvc;

namespace Neshagostar.WebUI.Areas.PersonnelManagement
{
    public class PersonnelManagementAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "PersonnelManagement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "PersonnelManagement_default",
                "PersonnelManagement/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
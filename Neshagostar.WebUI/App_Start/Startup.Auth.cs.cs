using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Neshagostar.DAL.DataModel;
using Neshagostar.WebUI.App_Start;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neshagostar.WebUI
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext(NeshagostarContext.Create);
            app.CreatePerOwinContext<PersonnelManager>(PersonnelManager.Create);
            app.CreatePerOwinContext<PersonnelSignInManager>(PersonnelSignInManager.Create);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString(@"/PersonnelManagement/Account/Login")
            });
            app.CreatePerOwinContext<PersonnelRoleManager>(PersonnelRoleManager.Create);
        }
    }
}
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Neshagostar.DAL.DataModel;
using Neshagostar.DAL.DataModel.PersonnelRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neshagostar.WebUI.App_Start
{

    #region Personnel
    public class PersonnelStore : UserStore<Personnel>
    {
        public PersonnelStore(NeshagostarContext context) : base(context)
        {

        }
    }

    public class PersonnelManager : UserManager<Personnel>
    {
        public PersonnelManager(IUserStore<Personnel> store) : base(store)
        {

        }
        public static PersonnelManager Create(IdentityFactoryOptions<PersonnelManager> options, IOwinContext context)
        {
            var store = new PersonnelStore(context.Get<NeshagostarContext>());
            var manager = new PersonnelManager(store);
            return manager;
        }
    }

    public class PersonnelSignInManager : SignInManager<Personnel, string>
    {
        public PersonnelSignInManager(PersonnelManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public static PersonnelSignInManager Create(IdentityFactoryOptions<PersonnelSignInManager> options, IOwinContext context)
        {
            return new PersonnelSignInManager(context.GetUserManager<PersonnelManager>(), context.Authentication);
        }
    }
    #endregion

    #region Roles
    public class PersonnelRoleManager : RoleManager<PersonnelRole>
    {
        public PersonnelRoleManager(IRoleStore<PersonnelRole, string> roleStore) : base(roleStore)
        {

        }

        public static PersonnelRoleManager Create(IdentityFactoryOptions<PersonnelRoleManager> options, IOwinContext context)
        {
            var personnelRoleManager = new PersonnelRoleManager(new RoleStore<PersonnelRole>(context.Get<NeshagostarContext>()));
            return personnelRoleManager;
        }
    }
    #endregion



}
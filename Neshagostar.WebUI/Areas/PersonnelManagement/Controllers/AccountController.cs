using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Neshagostar.DAL.DataModel;
using Neshagostar.DAL.DataModel.PersonnelRelated;
using Neshagostar.WebUI.App_Start;
using Neshagostar.WebUI.Areas.PersonnelManagement.Models;
using Neshagostar.WebUI.Areas.PersonnelManagement.Models.Personnel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Neshagostar.WebUI.Areas.PersonnelManagement.Controllers
{
    public class AccountController : Controller
    {
        private NeshagostarContext context = new NeshagostarContext();
        private PersonnelSignInManager _signInManager;
        private PersonnelManager _userManager;
        private PersonnelRoleManager _roleManager;

        public PersonnelRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<PersonnelRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        public PersonnelSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<PersonnelSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        public PersonnelManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<PersonnelManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
         
            var ds = context.Departments.ToList();
            ViewBag.Departments = context.Departments.Select(d => new SelectListItem
            {
                Text = d.Name,
                Value = d.Id.ToString()
            });
       
            return View("~/Areas/PersonnelManagement/Views/Account/Register.cshtml");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Personnel { Name=model.Name, UserName = model.UserName, DepartmentId = model.DepartmentId };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            ViewBag.Departments = context.Departments.Select(d => new SelectListItem
            {
                Text = d.Name,
                Value = d.Id.ToString()
            });
            return View("~/Areas/PersonnelManagement/Views/Account/Register.cshtml", model);
        }

                [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            

            //ViewBag.returnUrl = HttpContext.Request.UrlReferrer.ToString();


            //if (HttpContext.Current.User.Identity.IsAuthenticated)
            //{
            //    var username = HttpContext.Current.GetOwinContext().Authentication.User.Identity.Name;
            //    var name = HttpContext.Current.GetOwinContext().GetUserManager<PersonnelManager>().FindByName(username).Name;
            //    ViewBag.ReturnUrl = returnUrl;
            return View("~/Areas/PersonnelManagement/Views/Account/Login.cshtml");
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            
            //RedirectToAction("LogOut", new { controller = "Account", area = "personnelmanagement" });
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, false, true);
            switch (result)
            {
                case SignInStatus.Success:
                    var roleId = UserManager.Find(model.UserName, model.Password).Roles.FirstOrDefault().RoleId;
                    var roleName = RoleManager.Roles.Where(r => r.Id.Equals(roleId)).FirstOrDefault().Name;
                    switch (roleName)
                    {
                        case "secretary":
                            return RedirectToLocal(Url.Action("Index", "RecievedCalls", new {  area="CallManagement" }));
                        case "commerce-manager":
                            return RedirectToLocal(Url.Action("Index", "Inquiries", new { area = "Commerce" }));
                        case "storage-manager":
                            return RedirectToLocal(Url.Action("Index", "Orders", new { area = "Storage" }));
                        case "financial-manager":
                            return RedirectToLocal(Url.Action("Index", "Orders", new { area = "Finance" }));
                        case "CEO":
                            return RedirectToLocal(Url.Action("Index", "Orders", new { area = "commerce" }));
                        default:
                            return RedirectToLocal(returnUrl);
                    }
                   
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View("~/Areas/PersonnelManagement/Views/Account/Login.cshtml", model);
            }
        }

        [HttpGet]
        public ActionResult LogOut()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account", new { area="PersonnelManagement" });
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: PersonnelManagement/Account
        public ActionResult Index()
        {

            var list = context.Users.ToList();

            List<PersonnelViewModel> personnels = new List<PersonnelViewModel>();
            if (list.Count != 0)
            {
                foreach (var user in list)
                {


                    personnels.Add(new PersonnelViewModel
                    {
                        Name = UserManager.FindById(user.Id).Name,
                        Id = Guid.Parse(user.Id),
                        UserName = user.UserName
                        //DeprtmentName = UserManager.FindById(user.Id).Department.Name,
                        //DepartmentId = UserManager.FindById(user.Id).Department.Id
                    });
                    
                }
            }




            return View("~/areas/personnelmanagement/views/account/index.cshtml", personnels);
        }
    }
}
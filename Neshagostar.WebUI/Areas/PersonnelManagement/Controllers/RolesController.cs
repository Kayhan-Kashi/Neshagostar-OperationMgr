using Microsoft.AspNet.Identity.Owin;
using Neshagostar.DAL.DataModel;
using Neshagostar.DAL.DataModel.PersonnelRelated;
using Neshagostar.WebUI.App_Start;
using Neshagostar.WebUI.Areas.PersonnelManagement.Models.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Neshagostar.WebUI.Areas.PersonnelManagement.Controllers
{
    public class RolesController : Controller
    {
        private PersonnelRoleManager _roleManager;
        private PersonnelManager _userManager;

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

        public PersonnelManager PersonnelManager
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


        // GET: Role
        public ActionResult Index()
        {
            List<RoleViewModel> list = new List<RoleViewModel>();
            foreach (var role in RoleManager.Roles)
                list.Add(new RoleViewModel(role));
            return View("~/Areas/PersonnelManagement/Views/Roles/Index.cshtml", list);
        }

        public ActionResult Create()
        {
            return View("~/Areas/PersonnelManagement/Views/Roles/Create.cshtml");
        }

        [HttpPost]
        public async Task<ActionResult> Create(RoleViewModel model)
        {
            var role = new PersonnelRole() { Name = model.Name, Description = model.Description };
            await RoleManager.CreateAsync(role);
            return RedirectToAction("Index", new { controller = "Roles", area = "PersonnelManagement" });
        }

        public async Task<ActionResult> Edit(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            return View("~/Areas/PersonnelManagement/Views/Roles/Edit.cshtml", new RoleViewModel(role));
        }

        [HttpPost]
        public async Task<ActionResult> Edit(RoleViewModel model)
        {
            var role = new PersonnelRole() { Id = model.Id, Name = model.Name, Description = model.Description };
            await RoleManager.UpdateAsync(role);
            return RedirectToAction("Index", new { controller = "Roles", area = "PersonnelManagement" });
        }

        public async Task<ActionResult> Details(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            return View( new RoleViewModel(role));
        }

        public async Task<ActionResult> Delete(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            //await RoleManager.DeleteAsync(role);
            return View("~/Areas/PersonnelManagement/Views/Roles/Delete.cshtml", new RoleViewModel(role));
        }


        [HttpPost]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            //var role = await RoleManager.FindByIdAsync(id);
            var  personellRole = await RoleManager.FindByIdAsync(id);
            //PersonnelRole personellRole = new PersonnelRole
            //{
            //    Id = role.Id,
            //    Description = role.Description,
            //    Name = role.Name
            //};
            
            await RoleManager.DeleteAsync(personellRole);
            return RedirectToAction("Index", new { controller = "Roles", area = "PersonnelManagement" });
        }

        [HttpGet]
        public ActionResult Dedicate()
        {
            ViewBag.Roles = RoleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.Id.ToString()
            });

            ViewBag.Personnels = PersonnelManager.Users.Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            return View("~/areas/personnelmanagement/views/roles/dedicate.cshtml");
        }

        [HttpPost]
        public async Task<ActionResult> Dedicate(DedicatedRoleViewModel model)
        {
            var roles = RoleManager.Roles.ToList();
            List<string> strRoles = new List<string>();
            var user = await PersonnelManager.FindByIdAsync(model.PersonnelId);
        

            foreach (var rl in roles)
            {
                strRoles.Add(rl.Name);
                await PersonnelManager.RemoveFromRoleAsync(user.Id, rl.Name);
            }
      
            string[] strArray = strRoles.ToArray();
            var role =  await RoleManager.FindByIdAsync(model.RoleId);
           
           //await PersonnelManager.RemoveFromRolesAsync(user.Id, strArray);
           await PersonnelManager.AddToRoleAsync(user.Id, role.Name);
           return RedirectToAction("DedicatedRoleList", new { controller = "Roles", area = "PersonnelManagement" });

        }

        public ActionResult DedicatedRoleList()
        {


            var model = PersonnelManager.Users.Select(u => new DedicatedRoleViewModel
            {
                PersonName = u.Name,
                RoleId = u.Roles.FirstOrDefault().RoleId
            }).ToList();

            foreach(var item in model)
            {
                if(item.RoleId == null)
                {
                    continue;
                }
                item.RoleName = RoleManager.FindByIdAsync(item.RoleId).Result.Description;
            }

            return View("~/Areas/PersonnelManagement/Views/Roles/DedicatedRoleList.cshtml", model);
        }

    }
}
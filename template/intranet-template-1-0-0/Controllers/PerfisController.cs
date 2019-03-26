using $safeprojectname$.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace $safeprojectname$.Controllers
{
    public class PerfisController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationGroupManager _groupManager;

        public ApplicationGroupManager GroupManager
        {
            get
            {
                return _groupManager ?? new ApplicationGroupManager();
            }
            private set
            {
                _groupManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext()
                           .Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        public PerfisController()
        {
            ViewBag.Area = "Gestão do Sistema";
            ViewBag.Title = "Gerenciar Perfis";
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Lista(string busca)
        {
            var perfis = GroupManager.Groups;
            if (!string.IsNullOrEmpty(busca))
            {
                perfis = perfis.Where(p => p.Name.Contains(busca) || p.Description.Contains(busca));
            }
            return PartialView(perfis.ToList());
        }

        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationGroup applicationgroup =
                await this.GroupManager.Groups.FirstOrDefaultAsync(g => g.Id == id);
            if (applicationgroup == null)
            {
                return HttpNotFound();
            }
            var groupRoles = this.GroupManager.GetGroupRoles(applicationgroup.Id);
            string[] RoleNames = groupRoles.Select(p => p.Name).ToArray();
            ViewBag.RolesList = RoleNames;
            ViewBag.RolesCount = RoleNames.Count();
            return View(applicationgroup);
        }

        public ActionResult Novo()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(
            [Bind(Include = "Name,Description")] ApplicationGroup applicationgroup, params string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                applicationgroup.Data = DateTime.Now;
                // Create the new Group:
                var result = await this.GroupManager.CreateGroupAsync(applicationgroup);
                if (result.Succeeded)
                {
                    selectedRoles = selectedRoles ?? new string[] { };

                    // Add the roles selected:
                    await this.GroupManager.SetGroupRolesAsync(applicationgroup.Id, selectedRoles);
                }
                return RedirectToAction("Index");
            }

            // Otherwise, start over:
            ViewBag.RoleId = new SelectList(
                this.RoleManager.Roles.ToList(), "Id", "Name");
            return View("Novo", applicationgroup);
        }

        public async Task<ActionResult> Editar(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationGroup applicationgroup = await this.GroupManager.FindByIdAsync(id);
            if (applicationgroup == null)
            {
                return HttpNotFound();
            }

            // Get a list, not a DbSet or queryable:
            var groupRoles = await this.GroupManager.GetGroupRolesAsync(id);

            var model = new GroupViewModel()
            {
                Id = applicationgroup.Id,
                Name = applicationgroup.Name,
                Description = applicationgroup.Description
            };

            // load the roles/Roles for selection in the form:
            foreach (var Role in groupRoles)
            {
                var listItem = new SelectListItem()
                {
                    Text = Role.Name,
                    Value = Role.Id,
                    Selected = true
                };
                model.RolesList.Add(listItem);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(
            [Bind(Include = "Id,Name,Description")] GroupViewModel model, params string[] selectedRoles)
        {
            var group = await this.GroupManager.FindByIdAsync(model.Id);
            if (group == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                group.Name = model.Name;
                group.Description = model.Description;
                await this.GroupManager.UpdateGroupAsync(group);

                selectedRoles = selectedRoles ?? new string[] { };
                await this.GroupManager.SetGroupRolesAsync(group.Id, selectedRoles);
                return RedirectToAction("Index");
            }
            return View("Editar", model);
        }

        [HttpPost]
        public void Excluir(string id)
        {
            if (id == null)
            {
                throw new ArgumentException("Item inválido");
            }
            ApplicationGroup applicationgroup = GroupManager.FindById(id);
            GroupManager.DeleteGroup(id);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
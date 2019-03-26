using $safeprojectname$.Entities;
using $safeprojectname$.Enums;
using $safeprojectname$.Models;
using $safeprojectname$.Util;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Simple.ImageResizer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace $safeprojectname$.Controllers
{
    public class ColaboradoresController : BaseController
    {
        public ColaboradoresController()
        {
            ViewBag.Area = "Gestão do Sistema";
            ViewBag.Title = "Colaboradores";
        }

        public ColaboradoresController(ApplicationUserManager userManager,
            ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext()
                           .GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // Add the Group Manager (NOTE: only access through the public
        // Property, not by the instance variable!)
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

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Lista(string busca, Ordenacao? ordenacao)
        {
            var usuarios = UserManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(busca))
            {
                usuarios = usuarios.Where(c => c.Nome.Contains(busca));
            }

            if (ordenacao != null)
            {
                usuarios = ordenacao == Ordenacao.MaisAntigo ? usuarios.OrderBy(u => u.DataCadastro) : usuarios.OrderByDescending(u => u.DataCadastro);
            }
            else
            {
                usuarios = usuarios.OrderByDescending(u => u.DataCadastro);
            }

            return PartialView(usuarios.ToList());
        }

        public ActionResult Novo()
        {
            ViewBag.Grupos = GroupManager.Groups;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Novo(ApplicationUser user, string grupo, HttpPostedFileBase foto, HttpPostedFileBase arquivo)
        {
            if (ModelState.IsValid)
            {
                var senha = Static.GerarSenha();

                user.UserName = user.Email;
                user.DataCadastro = DateTime.Now;

                if (arquivo != null)
                {
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(arquivo.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Colaboradores/"), fileName);
                    arquivo.SaveAs(path);
                    user.Curriculo = fileName;
                }

                if (foto != null)
                {
                    MemoryStream target = new MemoryStream();
                    foto.InputStream.CopyTo(target);
                    var imageResizer = new ImageResizer(target.ToArray());
                    var nomeFoto = DateTime.Now.ToString("yyyyMMddHHmmss") + foto.FileName;
                    imageResizer.SaveToFile(Path.Combine(Server.MapPath("~/Content/Colaboradores/"), nomeFoto));

                    user.Avatar = nomeFoto;
                }

                var adminresult = await UserManager
                    .CreateAsync(user, senha);

                if (adminresult.Succeeded)
                {
                    GroupManager.SetUserGroups(user.Id, grupo);

                    try
                    {
                        var conteudoEmail = System.IO.File.ReadAllText(Server.MapPath("/Views/Colaboradores/mailAcesso.html"));
                        conteudoEmail = conteudoEmail.Replace("##usuario##", user.Email);
                        conteudoEmail = conteudoEmail.Replace("##senha##", senha);
                        Email.EnviaEmail("informatica@partage.com.br", user.Email, "Intranet Partage - Dados de Acesso", conteudoEmail);
                    }
                    catch (Exception e)
                    {
                    }

                    return RedirectToAction("Index");
                }
            }
            ViewBag.Grupos = GroupManager.Groups;
            return View();
        }

        [HttpPost]
        public void Excluir(string id)
        {
            if (id == null)
            {
                throw new ArgumentException("Item inválido");
            }

            var user = UserManager.FindById(id);
            if (user == null)
            {
                throw new ArgumentException("Item inválido");
            }

            GroupManager.ClearUserGroups(id);

            var result = UserManager.Delete(user);
            if (!result.Succeeded)
            {
                throw new ArgumentException("Item inválido");
            }
        }

        public ActionResult Editar(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = UserManager.FindById(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var allGroups = GroupManager.Groups;
            var userGroups = GroupManager.GetUserGroups(id);

            ViewBag.Grupos = new List<SelectListItem>();

            foreach (var group in allGroups)
            {
                var listItem = new SelectListItem()
                {
                    Text = group.Name,
                    Value = group.Id,
                    Selected = userGroups.Any(g => g.Id == group.Id)
                };
                ViewBag.Grupos.Add(listItem);
            }

            List<Empreendimento> emp = ViewBag.Empreendimentos;
            ViewBag.Empreendimentos = emp.Where(e => e.Nome != "Todos").ToList();

            return View(user);
        }

        [HttpPost]
        public ActionResult Editar(ApplicationUser usuario, string grupo, HttpPostedFileBase foto, HttpPostedFileBase arquivo)
        {

            List<Empreendimento> emp = ViewBag.Empreendimentos;
            ViewBag.Empreendimentos = emp.Where(e => e.Nome != "Todos").ToList();

            if (!ModelState.IsValid)
            {
                var allGroups = GroupManager.Groups;
                var userGroups = GroupManager.GetUserGroups(usuario.Id);

                ViewBag.Grupos = new List<SelectListItem>();

                foreach (var group in allGroups)
                {
                    var listItem = new SelectListItem()
                    {
                        Text = group.Name,
                        Value = group.Id,
                        Selected = userGroups.Any(g => g.Id == group.Id)
                    };
                    ViewBag.Grupos.Add(listItem);
                }

                return View(usuario);
            }

            var user = UserManager.FindById(usuario.Id);

            user.Nome = usuario.Nome;
            user.Cargo = usuario.Cargo;
            user.IdEmpreendimento = usuario.IdEmpreendimento;
            //user.Sexo = usuario.Sexo;
            user.DataNascimento = usuario.DataNascimento;
            user.Telefone = usuario.Telefone;
            user.TipoSangue = usuario.TipoSangue;
            user.Alergias = usuario.Alergias;
            user.TelefoneResidencial = usuario.TelefoneResidencial;
            user.TelefoneComercial = usuario.TelefoneComercial;
            user.TelefoneEmergencia = usuario.TelefoneEmergencia;
            user.EmailParticular = usuario.EmailParticular;
            user.Skype = usuario.Skype;
            user.Linkedin = usuario.Linkedin;
            user.LocalNascimento = usuario.LocalNascimento;
            user.Hobbies = usuario.Hobbies;
            user.Interesses = usuario.Interesses;
            user.AtividadesVoluntarias = usuario.AtividadesVoluntarias;
            user.Esporte = usuario.Esporte;

            if (arquivo != null)
            {
                var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(arquivo.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/Colaboradores/"), fileName);
                arquivo.SaveAs(path);
                user.Curriculo = fileName;
            }

            if (foto != null)
            {
                MemoryStream target = new MemoryStream();
                foto.InputStream.CopyTo(target);
                var imageResizer = new ImageResizer(target.ToArray());
                var nomeFoto = DateTime.Now.ToString("yyyyMMddHHmmss") + foto.FileName;
                imageResizer.SaveToFile(Path.Combine(Server.MapPath("~/Content/Colaboradores/"), nomeFoto));

                user.Avatar = nomeFoto;
            }

            var teste = UserManager.Update(user);

            GroupManager.SetUserGroups(user.Id, grupo);
            return RedirectToAction("Index", "Colaboradores");
        }
    }
}
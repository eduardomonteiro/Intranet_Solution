using System;
using System.IO;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Mvc;
using $safeprojectname$.Models;
using Simple.ImageResizer;

namespace $safeprojectname$.Controllers
{
    public class MeuPerfilController : BaseController
    {
        public MeuPerfilController()
        {
            ViewBag.Area = "Meu Perfil";
        }

        public MeuPerfilController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
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

        public ActionResult Index()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.Area = "Colaborador";
            ViewBag.Title = "Meu Perfil";
            return View(user);
        }

        [HttpPost]
        public ActionResult Editar(ApplicationUser usuario, HttpPostedFileBase foto, HttpPostedFileBase arquivo)
        {

            var user = UserManager.FindById(usuario.Id);

            user.Nome = usuario.Nome;
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

            return RedirectToAction("Index", "MeuPerfil");
        }

        public ActionResult Senha()
        {
            ViewBag.Area = "Colaborador";
            ViewBag.Title = "Alterar Senha";
            return View();
        }

        [HttpPost]
        public ActionResult AlterarSenha(string senha)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }

            user.PasswordHash = UserManager.PasswordHasher.HashPassword(senha);
            var result = UserManager.Update(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "MeuPerfil");
            }
            return View("Senha");
        }
    }
}
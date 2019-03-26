using $safeprojectname$.Entities;
using $safeprojectname$.Enums;
using $safeprojectname$.Util;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace $safeprojectname$.Controllers
{
    public class ContatoController : BaseController
    {
        public ContatoController()
        {
            ViewBag.Area = "Contato";
            ViewBag.Texto =
                "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aliquam interdum pulvinar nibh. Maecenas eget nunc in justo rhoncus aliquam. Phasellus nisl mi, convallis ut, lacinia vel, sodales tempus, ante. Praesent eros. Aenean feugiat fringilla odio. Donec porttitor volutpat elit. Proin metus justo, accumsan vitae, egestas nec, porttitor nec, felis. Nam vel odio a arcu consectetuer fringilla. Vivamus eget arcu eget nisi adipiscing egestas. Donec venenatis pretium sem. Etiam nec sapien. Cras mauris. Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aliquam interdum pulvinar nibh. Maecenas eget nunc in justo rhoncus aliquam. Phasellus nisl mi, convallis ut, lacinia vel, sodales tempus, ante. Praesent eros. Aenean feugiat fringilla odio. Donec porttitor volutpat elit. Proin metus justo, accumsan vitae, egestas nec, porttitor nec, felis. Nam vel odio a arcu consectetuer fringilla. Vivamus eget arcu eget nisi adipiscing egestas. Donec venenatis pretium sem. Etiam nec sapien. Cras mauris.";
        }

        // GET: Contato
        public ActionResult Index(TipoContato tipo)
        {
            ViewBag.Title = EnumExtensions.EnumDisplayNameFor(tipo);
            ViewBag.Tipo = tipo;
            return View();
        }

        public ActionResult Lista(TipoContato tipo, string busca, int? empreendimento, bool? finalizado)
        {
            var contatos = _banco.Contato.Where(c => c.Tipo == tipo).AsQueryable();

            if (!User.IsInRole(tipo + "-Admin"))
            {
                contatos = contatos.Where(c => c.IdUsuario == UsuarioId).AsQueryable();
            }

            if (!string.IsNullOrEmpty(busca))
            {
                contatos = contatos.Where(l => l.Assunto.Contains(busca) || l.Mensagem.Contains(busca));
            }

            if (empreendimento != null)
            {
                contatos = contatos.Where(l => l.IdEmpreendimento == empreendimento);
            }

            if (finalizado != null)
            {
                contatos = contatos.Where(l => l.Finalizado == finalizado);
            }
            return PartialView(contatos.OrderByDescending(l => l.DataCriacao).ToList());
        }

        public ActionResult Novo(TipoContato tipo)
        {
            ViewBag.Title = EnumExtensions.EnumDisplayNameFor(tipo);
            ViewBag.Tipo = tipo;

            var idUsuario = User.Identity.GetUserId();

            var usuario = _banco.Usuarios.First(u => u.Id == idUsuario);

            var contato = new Contato
            {
                Tipo = tipo,
                Finalizado = false,
                IdUsuario = usuario.Id,
                Usuario = usuario,
                IdEmpreendimento = usuario.IdEmpreendimento,
                Empreendimento = usuario.Empreendimento
            };

            return View(contato);
        }

        public FileResult Download(string arquivo)
        {
            if (arquivo.Contains("../"))
            {
                throw new ArgumentException("Arquivo inválido");
            }
            var caminho = Server.MapPath("~/Content/Contato/") + arquivo;
            var file = new FileInfo(caminho);

            var extensoesPermitidas = ".jpg,.jpeg,.png,.gif,.pdf,.doc,.docx,.txt,.odt,.rtf,.xls,.xlsx,.ai,.eps";

            if (file.Exists)
            {
                var extensao = file.Extension;

                if (extensoesPermitidas.ToLower().Contains(extensao.ToLower()))
                {
                    return File("~/Content/Contato/" + arquivo, System.Net.Mime.MediaTypeNames.Application.Octet, arquivo);
                }
            }
            throw new ArgumentException("Arquivo inválido");
        }

        [HttpPost]
        public ActionResult Novo(Contato model, HttpPostedFileBase[] arquivos)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (arquivos != null)
            {
                foreach (var anexo in arquivos)
                {
                    if (anexo != null)
                    {
                        var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(anexo.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/Contato/"), fileName);
                        anexo.SaveAs(path);

                        model.Anexos.Add(new ContatoAnexo
                        {
                            Arquivo = fileName,
                            Data = DateTime.Now,
                            IdUsuario = model.IdUsuario
                        });
                    }
                }
            }
            model.DataCriacao = DateTime.Now;
            _banco.Contato.Add(model);
            _banco.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Interacao(TipoContato tipo, int id)
        {
            ViewBag.Title = EnumExtensions.EnumDisplayNameFor(tipo);
            ViewBag.Tipo = tipo;

            var contato = _banco.Contato.Find(id);
            return View(contato);
        }

        [HttpPost]
        public ActionResult Anexos(TipoContato tipo, int id, HttpPostedFileBase[] arquivos)
        {
            var model = _banco.Contato.Find(id);

            if (arquivos != null)
            {
                foreach (var anexo in arquivos)
                {
                    if (anexo != null)
                    {
                        var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(anexo.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/Contato/"), fileName);
                        anexo.SaveAs(path);

                        model.Anexos.Add(new ContatoAnexo
                        {
                            Arquivo = fileName,
                            Data = DateTime.Now,
                            IdUsuario = model.IdUsuario
                        });
                    }
                }

                _banco.Contato.AddOrUpdate(model);
                _banco.SaveChanges();
            }
            return RedirectToAction("Interacao", "Contato", new { tipo = model.Tipo, id = model.Id });
        }

        public void ExcluirAnexo(TipoContato tipo, int id)
        {
            var anexo = _banco.ContatoAnexo.Find(id);

            if (anexo == null)
            {
                throw new ArgumentException("Item inválido");
            }

            if (anexo.IdUsuario == User.Identity.GetUserId())
            {
                _banco.ContatoAnexo.Remove(anexo);
                _banco.SaveChanges();
            }
        }

        public ActionResult Finalizar(TipoContato tipo, int id)
        {
            var contato = _banco.Contato.Find(id);

            if (contato == null)
            {
                return RedirectToAction("Index", new { tipo });
            }

            contato.Finalizado = true;
            contato.DataFinalizacao = DateTime.Now;
            _banco.SaveChanges();

            return RedirectToAction("Index", new { tipo });
        }

        [HttpPost]
        public void Mensagem(TipoContato tipo, int id, string mensagem)
        {
            var msg = new ContatoInteracao
            {
                IdContato = id,
                IdUsuario = User.Identity.GetUserId(),
                Mensagem = mensagem,
                Data = DateTime.Now
            };

            _banco.ContatoInteracao.AddOrUpdate(msg);
            _banco.SaveChanges();
        }
    }
}
using Intranet.Data.Entities;
using Intranet.Data.Enums;
using Intranet.UI.Models;
using Intranet.UI.Util;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Intranet.UI.Controllers
{
    public class ContatoController : BaseController
    {
        public ContatoController()
        {
            ViewBag.Area = "Contato";
        }

        // GET: Contato
        public ActionResult Index(TipoContato tipo)
        {
            ViewBag.Title = EnumExtensions.EnumDisplayNameFor(tipo);
            ViewBag.Tipo = tipo;
            if(tipo == TipoContato.FaleComRh)
            {
                ViewBag.Assuntos = new SelectList(_banco.AssuntoContato.Include("Assuntos").Where(x => x.IdContato == tipo).Select(x => x.Assuntos.Nome).ToList(), "Nome");
                ViewBag.Texto =
               "<p>Espaço destinado para que os colaboradores esclareçam dúvidas com a área de Recursos Humanos, encaminhem sugestões, elogios e indiquem oportunidades de melhoria. A previsão de retorno será de 48 horas, sendo que de acordo com a complexidade da demanda o prazo pode ser estendido.</p>";
            }else if (tipo == TipoContato.Ouvidoria)
            {
                ViewBag.Assuntos = new SelectList(_banco.AssuntoContato.Include("Assuntos").Where(x => x.IdContato == tipo).Select(x => x.Assuntos.Nome).ToList(), "Nome");
                ViewBag.Texto =
               "<p>Caso se sinta mais confortável e/ou tratando-se de um assunto confidencial, você pode utilizar este canal de OUVIDORIA.</p><p> O intuito da OUVIDORIA é o de captar impressões e percepções do ambiente e das relações de trabalho que não estão sendo devidamente evidenciadas nos canais de comunicação formais da organização e mediá-las de maneira imparcial, harmoniosa e acolhedora.</p><p> O retorno será realizado no prazo médio de 72 horas.De acordo com o nível de complexidade do assunto, o prazo será estendido.</p>";
            }

            return View();
        }

        public ActionResult Lista(TipoContato tipo, string busca, int? empreendimento, bool? finalizado, string Assunto)
        {
            var contatos = _banco.Contato.Where(c => c.Tipo == tipo).AsQueryable();

            if (User.IsInRole(tipo + "-Localidade"))
            {                
                contatos = contatos.Where(c => (Usuario.Empreendimento.Id == 10 ? true : (c.IdEmpreendimento.ToString().Contains(Usuario.Empreendimento.Id.ToString()) || c.IdEmpreendimento.ToString() == "10")));
            }
            else if(!User.IsInRole(tipo + "-Admin")) {

                 contatos = contatos.Where(c => c.IdUsuario == UsuarioId).AsQueryable();
            }


            if (!string.IsNullOrEmpty(Assunto))
            {
                contatos = contatos.Where(l => l.Assunto.Contains(Assunto));
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
            ViewBag.Assuntos = new SelectList(_banco.AssuntoContato.Include("Assuntos").Where(x => x.IdContato == tipo).Select(x => x.Assuntos.Nome).ToList(), "Nome");
            

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
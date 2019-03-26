using $safeprojectname$.Entities;
using $safeprojectname$.Enums;
using $safeprojectname$.Util;
using System;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace $safeprojectname$.Controllers
{
    public class DocumentosController : BaseController
    {
        // GET: Documentos
        public ActionResult Index(TipoDocumento tipo)
        {
            ViewBag.Area = "Documentos";
            ViewBag.Title = EnumExtensions.EnumDisplayNameFor(tipo);
            ViewBag.Tipo = tipo;
            ViewBag.Categorias = _banco.DocumentosCategorias.Where(d => d.Tipo == tipo).OrderBy(e => e.Nome).ToList();

            return View();
        }

        public ActionResult Lista(TipoDocumento tipo, string busca, int? idEmpreendimento, Ordenacao? ordenacao, int? idCategoria)
        {
            ViewBag.Tipo = tipo;
            var documentos = _banco.Documentos.Where(d => d.Tipo == tipo).AsQueryable();

            if (!string.IsNullOrEmpty(busca))
            {
                documentos = documentos.Where(l => l.Nome.Contains(busca));
            }

            if (idEmpreendimento != null)
            {
                documentos = documentos.Where(l => l.IdEmpreendimento == idEmpreendimento);
            }

            if (idCategoria != null)
            {
                documentos = documentos.Where(l => l.IdCategoria == idCategoria);
            }

            if (ordenacao != null)
            {
                documentos = ordenacao.Value == Ordenacao.MaisAntigo
                    ? documentos.OrderBy(l => l.Data)
                    : documentos.OrderByDescending(l => l.Data);
            }
            else
            {
                documentos = documentos.OrderByDescending(d => d.Data);
            }

            return PartialView(documentos.ToList());
        }

        public FileResult Download(TipoDocumento tipo, string arquivo)
        {
            if (arquivo.Contains("../"))
            {
                throw new ArgumentException("Arquivo inválido");
            }
            var caminho = Server.MapPath("~/Content/Documentos/") + arquivo;
            var file = new FileInfo(caminho);

            var extensoesPermitidas = ".jpg,.jpeg,.png,.gif,.pdf,.doc,.docx,.txt,.odt,.rtf,.xls,.xlsx,.ai,.eps";

            if (file.Exists)
            {
                var extensao = file.Extension;

                if (extensoesPermitidas.ToLower().Contains(extensao.ToLower()))
                {
                    return File("~/Content/Documentos/" + arquivo, System.Net.Mime.MediaTypeNames.Application.Octet, arquivo);
                }
            }
            throw new ArgumentException("Arquivo inválido");
        }

        public void Excluir(int id)
        {
            var item = _banco.Documentos.Find(id);
            if (item == null)
            {
                throw new ArgumentException("Item inválido");
            }

            _banco.Documentos.Remove(item);
            _banco.SaveChanges();
        }

        public ActionResult Novo(TipoDocumento tipo)
        {
            ViewBag.Area = "Documentos";
            ViewBag.Title = EnumExtensions.EnumDisplayNameFor(tipo);
            ViewBag.Categorias = _banco.DocumentosCategorias.Where(d => d.Tipo == tipo).OrderBy(e => e.Nome).ToList();

            var doc = new Documento
            {
                Tipo = tipo
            };
            return View(doc);
        }

        [HttpPost]
        public ActionResult Novo(Documento model, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Documentos/"), fileName);
                    file.SaveAs(path);

                    model.Arquivo = fileName;
                }

                _banco.Documentos.Add(model);
                _banco.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.Categorias = _banco.DocumentosCategorias.Where(d => d.Tipo == model.Tipo).OrderBy(e => e.Nome).ToList();

            return View(model);
        }

        public ActionResult Editar(TipoDocumento tipo, int id)
        {
            ViewBag.Area = "Documentos";
            ViewBag.Title = EnumExtensions.EnumDisplayNameFor(tipo);
            ViewBag.Categorias = _banco.DocumentosCategorias.Where(d => d.Tipo == tipo).OrderBy(e => e.Nome).ToList();

            var documento = _banco.Documentos.Find(id);
            if (documento == null)
            {
                return RedirectToAction("Index");
            }

            return View(documento);
        }

        [HttpPost]
        public ActionResult Editar(Documento model, HttpPostedFileBase file)
        {
            ViewBag.Categorias = _banco.DocumentosCategorias.Where(d => d.Tipo == model.Tipo).OrderBy(e => e.Nome).ToList();

            if (!ModelState.IsValid) return View(model);

            if (file != null)
            {
                var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/Documentos/"), fileName);
                file.SaveAs(path);

                model.Arquivo = fileName;
            }

            _banco.Documentos.AddOrUpdate(model);
            _banco.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
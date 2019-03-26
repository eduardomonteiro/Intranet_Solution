using Intranet.Data.Entities;
using Simple.ImageResizer;
using System;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Intranet.UI.Controllers
{
    [Authorize(Roles = "Noticias,Noticias-Admin")]
    public class NoticiasController : BaseController
    {
        public NoticiasController()
        {
            ViewBag.Categorias = _banco.NoticiasCategorias.OrderBy(e => e.Nome).ToList();

            ViewBag.Title = "Notícias";
        }

        // GET: Noticias
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Lista(string busca, int? idCategoria)
        {
            var noticias = _banco.Noticias.AsQueryable();

            if (!string.IsNullOrEmpty(busca))
            {
                noticias = noticias.Where(l => l.Titulo.Contains(busca) || l.Texto.Contains(busca));
            }

            if (idCategoria != null)
            {
                noticias = noticias.Where(l => l.IdCategoria == idCategoria);
            }
            return PartialView(noticias.OrderByDescending(l => l.Data).ToList());
        }

        public void Excluir(int id)
        {
            var item = _banco.Noticias.Find(id);
            if (item == null)
            {
                throw new ArgumentException("Item inválido");
            }

            _banco.Noticias.Remove(item);
            _banco.SaveChanges();
        }

        public ActionResult Novo()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Novo(Noticia model, HttpPostedFileBase foto, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (foto != null)
                {
                    MemoryStream target = new MemoryStream();
                    foto.InputStream.CopyTo(target);
                    var imageResizer = new ImageResizer(target.ToArray());
                    //imageResizer.Resize(237, 139, ImageEncoding.Png);
                    var nomeFoto = DateTime.Now.ToString("yyyyMMddHHmmss") + foto.FileName;
                    imageResizer.SaveToFile(Path.Combine(Server.MapPath("~/Content/Noticias/"), nomeFoto));

                    model.Imagem = nomeFoto;
                }

                if (file != null)
                {
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Noticias/"), fileName);
                    file.SaveAs(path);

                    model.Arquivo = fileName;
                }

                _banco.Noticias.Add(model);
                _banco.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Editar(int id)
        {
            var noticia = _banco.Noticias.Find(id);
            if (noticia == null)
            {
                return RedirectToAction("Index");
            }

            return View(noticia);
        }

        public FileResult Download(string arquivo)
        {
            if (arquivo.Contains("../"))
            {
                throw new ArgumentException("Arquivo inválido");
            }
            var caminho = Server.MapPath("~/Content/Noticias/") + arquivo;
            var file = new FileInfo(caminho);

            var extensoesPermitidas = ".jpg,.jpeg,.png,.gif,.pdf,.doc,.docx,.txt,.odt,.rtf,.xls,.xlsx,.ai,.eps";

            if (file.Exists)
            {
                var extensao = file.Extension;

                if (extensoesPermitidas.ToLower().Contains(extensao.ToLower()))
                {
                    return File("~/Content/Noticias/" + arquivo, System.Net.Mime.MediaTypeNames.Application.Octet, arquivo);
                }
            }
            throw new ArgumentException("Arquivo inválido");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Editar(Noticia model, HttpPostedFileBase foto, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (foto != null)
                {
                    MemoryStream target = new MemoryStream();
                    foto.InputStream.CopyTo(target);
                    var imageResizer = new ImageResizer(target.ToArray());
                    //imageResizer.Resize(237, 139, ImageEncoding.Png);
                    var nomeFoto = DateTime.Now.ToString("yyyyMMddHHmmss") + foto.FileName;
                    imageResizer.SaveToFile(Path.Combine(Server.MapPath("~/Content/Noticias/"), nomeFoto));

                    model.Imagem = nomeFoto;
                }

                if (file != null)
                {
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Noticias/"), fileName);
                    file.SaveAs(path);

                    model.Arquivo = fileName;
                }

                _banco.Noticias.AddOrUpdate(model);
                _banco.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Detalhes(int id)
        {
            var noticia = _banco.Noticias.Find(id);
            if (noticia == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Recomendado = _banco.Noticias.Where(c => c.IdCategoria == noticia.IdCategoria && c.Id != noticia.Id).OrderByDescending(n => n.Data).Take(4).ToList();

            return View(noticia);
        }

        public ActionResult NovaFoto()
        {
            return View();
        }
    }
}
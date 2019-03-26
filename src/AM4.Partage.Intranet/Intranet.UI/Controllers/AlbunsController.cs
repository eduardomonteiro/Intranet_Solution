using Intranet.Data.Entities;
using Simple.ImageResizer;
using System;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Intranet.UI.Controllers
{
    [Authorize(Roles = "Albuns,Albuns-Admin")]
    public class AlbunsController : BaseController
    {
        public AlbunsController()
        {
            ViewBag.Area = "Marketing";
            ViewBag.Title = "Fotos dos Shoppings";
        }

        // GET: Albuns
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Lista(string busca, int? idEmpreendimento)
        {
            var albuns = _banco.Albuns.AsQueryable();

            if (User.IsInRole("Albuns-Localidade"))
            {
                albuns = albuns.Where(c => (Usuario.Empreendimento.Id == 10 ? true : (c.IdEmpreendimento.ToString().Contains(Usuario.Empreendimento.Id.ToString()) || c.IdEmpreendimento.ToString() == "10")));
            }

            if (!string.IsNullOrEmpty(busca))
            {
                albuns = albuns.Where(l => l.Titulo.Contains(busca));
            }

            if (idEmpreendimento != null)
            {
                albuns = albuns.Where(l => l.IdEmpreendimento == idEmpreendimento);
            }
            return PartialView(albuns.OrderByDescending(l => l.Id).ToList());
        }

        public void Excluir(int id)
        {
            var item = _banco.Albuns.Find(id);
            if (item == null)
            {
                throw new ArgumentException("Item inválido");
            }

            _banco.Albuns.Remove(item);
            _banco.SaveChanges();
        }

        public ActionResult Novo()
        {

            var idUsuario = User.Identity.GetUserId();

            var usuario = _banco.Usuarios.First(u => u.Id == idUsuario);

            var album = new Album
            {                
                IdEmpreendimento = usuario.IdEmpreendimento,
                Empreendimento = usuario.Empreendimento
            };
            return View(album);
        }

        [HttpPost]
        public ActionResult Novo(Album model, HttpPostedFileBase foto)
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
                    imageResizer.SaveToFile(Path.Combine(Server.MapPath("~/Content/Albuns/"), nomeFoto));

                    model.Capa = nomeFoto;
                }

                _banco.Albuns.Add(model);
                _banco.SaveChanges();

                return RedirectToAction("Index", "Albuns");
            }

            return View(model);
        }

        public ActionResult Editar(int id)
        {
            var album = _banco.Albuns.Find(id);
            if (album == null)
            {
                return RedirectToAction("Index");
            }

            return View(album);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Editar(Album model, HttpPostedFileBase foto)
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
                    imageResizer.SaveToFile(Path.Combine(Server.MapPath("~/Content/Albuns/"), nomeFoto));

                    model.Capa = nomeFoto;
                }

                _banco.Albuns.AddOrUpdate(model);
                _banco.SaveChanges();

                return RedirectToAction("Index", "Albuns");
            }

            return View(model);
        }

        public ActionResult Fotos(int id)
        {
            var album = _banco.Albuns.Find(id);
            if (album == null)
            {
                return RedirectToAction("Index", "Albuns");
            }
            return View(album);
        }

        public ActionResult ListaFotos(int id)
        {
            var album = _banco.Albuns.Find(id);
            return PartialView(album.Fotos.OrderBy(f => f.Id));
        }

        public ActionResult NovaFoto(int id)
        {
            var model = new AlbumFoto
            {
                IdAlbum = id
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult NovaFoto(AlbumFoto model, HttpPostedFileBase[] fotos)
        {
            if (fotos != null)
            {
                foreach (var foto in fotos)
                {
                    MemoryStream target = new MemoryStream();
                    foto.InputStream.CopyTo(target);
                    var imageResizer = new ImageResizer(target.ToArray());
                    //imageResizer.Resize(237, 139, ImageEncoding.Png);
                    var nomeFoto = DateTime.Now.ToString("yyyyMMddHHmmss") + foto.FileName;
                    imageResizer.SaveToFile(Path.Combine(Server.MapPath("~/Content/Albuns/Fotos/"), nomeFoto));

                    var fotoModel = new AlbumFoto
                    {
                        Legenda = model.Legenda,
                        Credito = model.Credito,
                        Data = DateTime.Now,
                        IdAlbum = model.IdAlbum,
                        Imagem = nomeFoto
                    };

                    _banco.AlbumFotos.Add(fotoModel);
                    _banco.SaveChanges();
                }

                return RedirectToAction("Fotos", "Albuns", new { id = model.IdAlbum });
            }

            return View();
        }

        public FileResult Download(string foto)
        {
            if (foto.Contains("../"))
            {
                throw new ArgumentException("Arquivo inválido");
            }
            var caminho = Server.MapPath("~/Content/Albuns/Fotos/") + foto;
            var file = new FileInfo(caminho);

            var extensoesPermitidas = ".jpg,.jpeg,.png,.gif,.ai,.eps";

            if (file.Exists)
            {
                var extensao = file.Extension;

                if (extensoesPermitidas.ToLower().Contains(extensao.ToLower()))
                {
                    return File("~/Content/Albuns/Fotos/" + foto, System.Net.Mime.MediaTypeNames.Application.Octet, foto);
                }
            }
            throw new ArgumentException("Arquivo inválido");
        }

        public void ExcluirFoto(int id)
        {
            var item = _banco.AlbumFotos.Find(id);
            if (item == null)
            {
                throw new ArgumentException("Item inválido");
            }

            _banco.AlbumFotos.Remove(item);
            _banco.SaveChanges();
        }

        public ActionResult EditarFoto(int id)
        {
            var foto = _banco.AlbumFotos.Find(id);
            if (foto == null)
            {
                return RedirectToAction("Index");
            }

            return View(foto);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditarFoto(AlbumFoto model, HttpPostedFileBase foto)
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
                    imageResizer.SaveToFile(Path.Combine(Server.MapPath("~/Content/Albuns/Fotos/"), nomeFoto));

                    model.Imagem = nomeFoto;
                }

                _banco.AlbumFotos.AddOrUpdate(model);
                _banco.SaveChanges();

                return RedirectToAction("Fotos", "Albuns", new { id = model.IdAlbum });
            }

            return View(model);
        }
    }
}
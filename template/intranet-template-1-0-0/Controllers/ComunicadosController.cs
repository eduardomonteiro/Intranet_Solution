using $safeprojectname$.Entities;
using $safeprojectname$.Enums;
using Simple.ImageResizer;
using System;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace $safeprojectname$.Controllers
{
    public class ComunicadosController : BaseController
    {
        public ComunicadosController()
        {
            ViewBag.Title = "Comunicados";
        }

        // GET: Comunicados
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Lista(string busca, TipoComunicado? tipo)
        {
            var comunicados = _banco.Comunicados.AsQueryable();

            if (!User.IsInRole("Comunicados-Admin"))
            {
                comunicados = comunicados.Where(c => (Usuario.Empreendimento.Id == 10 ? true : (c.Shoppings.Contains(Usuario.Empreendimento.Id.ToString()) || c.Shoppings == "10"))
                //|| c.Sexo.Contains(Usuario.Sexo) 
                || c.Cargos.Contains(Usuario.Cargo));
            }

            if (!string.IsNullOrEmpty(busca))
            {
                comunicados = comunicados.Where(c => c.Titulo.Contains(busca));
            }

            if (tipo != null)
            {
                comunicados = comunicados.Where(c => c.Tipo == tipo);
            }

            return PartialView(comunicados.OrderByDescending(c => c.Data).ToList());
        }

        public ActionResult Detalhes(int id)
        {
            var comunicado = _banco.Comunicados.Find(id);
            if (comunicado == null)
            {
                return RedirectToAction("Index");
            }

            return View(comunicado);
        }

        public FileResult Download(string arquivo)
        {
            if (arquivo.Contains("../"))
            {
                throw new ArgumentException("Arquivo inválido");
            }
            var caminho = Server.MapPath("~/Content/Comunicados/") + arquivo;
            var file = new FileInfo(caminho);

            var extensoesPermitidas = ".jpg,.jpeg,.png,.gif,.pdf,.doc,.docx,.txt,.odt,.rtf,.xls,.xlsx,.ai,.eps";

            if (file.Exists)
            {
                var extensao = file.Extension;

                if (extensoesPermitidas.ToLower().Contains(extensao.ToLower()))
                {
                    return File("~/Content/Comunicados/" + arquivo, System.Net.Mime.MediaTypeNames.Application.Octet, arquivo);
                }
            }
            throw new ArgumentException("Arquivo inválido");
        }

        public void Excluir(int id)
        {
            var item = _banco.Comunicados.Find(id);
            if (item == null)
            {
                throw new ArgumentException("Item inválido");
            }

            _banco.Comunicados.Remove(item);
            _banco.SaveChanges();
        }

        public ActionResult Novo()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Novo(Comunicado model, string[] shoppings, string[] sexo, string[] cargo, HttpPostedFileBase[] arquivos, HttpPostedFileBase foto)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (shoppings != null)
            {
                model.Shoppings = string.Join(",", shoppings);
            }
            //if (sexo != null)
            //{
            //    model.Sexo = string.Join(",", sexo);
            //}
            if (cargo != null)
            {
                model.Cargos = string.Join(",", cargo);
            }

            if (arquivos != null)
            {
                foreach (var anexo in arquivos)
                {
                    if (anexo != null)
                    {
                        var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(anexo.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/Comunicados/"), fileName);
                        anexo.SaveAs(path);

                        model.Anexos.Add(new ComunicadoAnexo
                        {
                            Arquivo = fileName,
                            Data = DateTime.Now,
                            IdUsuario = UsuarioId
                        });
                    }
                }
            }

            if (foto != null)
            {
                MemoryStream target = new MemoryStream();
                foto.InputStream.CopyTo(target);
                var imageResizer = new ImageResizer(target.ToArray());
                //imageResizer.Resize(237, 139, ImageEncoding.Png);
                var nomeFoto = DateTime.Now.ToString("yyyyMMddHHmmss") + foto.FileName;
                imageResizer.SaveToFile(Path.Combine(Server.MapPath("~/Content/Comunicados/"), nomeFoto));

                model.Imagem = nomeFoto;
            }

            _banco.Comunicados.Add(model);
            _banco.SaveChanges();

            return RedirectToAction("Index", "Comunicados");
        }

        public ActionResult Editar(int id)
        {
            var comunicado = _banco.Comunicados.Find(id);
            if (comunicado == null)
            {
                return RedirectToAction("Index");
            }

            return View(comunicado);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Editar(Comunicado model, string[] shoppings, string[] sexo, string[] cargo, HttpPostedFileBase foto)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (shoppings != null)
            {
                model.Shoppings = string.Join(",", shoppings);
            }
            //if (sexo != null)
            //{
            //    model.Sexo = string.Join(",", sexo);
            //}
            if (cargo != null)
            {
                model.Cargos = string.Join(",", cargo);
            }

            if (foto != null)
            {
                MemoryStream target = new MemoryStream();
                foto.InputStream.CopyTo(target);
                var imageResizer = new ImageResizer(target.ToArray());
                //imageResizer.Resize(237, 139, ImageEncoding.Png);
                var nomeFoto = DateTime.Now.ToString("yyyyMMddHHmmss") + foto.FileName;
                imageResizer.SaveToFile(Path.Combine(Server.MapPath("~/Content/Comunicados/"), nomeFoto));

                model.Imagem = nomeFoto;
            }

            _banco.Comunicados.AddOrUpdate(model);
            _banco.SaveChanges();

            return RedirectToAction("Index", "Comunicados");
        }

        public void ExcluirAnexo(int id)
        {
            var anexo = _banco.ComunicadoAnexos.Find(id);

            if (anexo == null)
            {
                throw new ArgumentException("Item inválido");
            }

            _banco.ComunicadoAnexos.Remove(anexo);
            _banco.SaveChanges();
        }

        [HttpPost]
        public ActionResult Anexos(int id, HttpPostedFileBase[] arquivos)
        {
            var comunicado = _banco.Comunicados.Find(id);

            if (arquivos != null)
            {
                foreach (var anexo in arquivos)
                {
                    if (anexo != null)
                    {
                        var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(anexo.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/Comunicados/"), fileName);
                        anexo.SaveAs(path);

                        comunicado.Anexos.Add(new ComunicadoAnexo
                        {
                            Arquivo = fileName,
                            Data = DateTime.Now,
                            IdUsuario = UsuarioId
                        });
                    }
                }
            }
            _banco.SaveChanges();

            return RedirectToAction("Editar", "Comunicados", new { id });
        }
    }
}
using $safeprojectname$.Entities;
using $safeprojectname$.Enums;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace $safeprojectname$.Controllers
{
    public class EducacaoContinuadaController : BaseController
    {
        public EducacaoContinuadaController()
        {
            ViewBag.Area = "Gente e Gestão";
            ViewBag.Title = "Educação Continuada";
        }

        // GET: EducacaoContinuada
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Lista(string busca, TipoCurso? categoria, bool? realizado)
        {
            var cursos = _banco.Cursos.Where(c => c.Ativo).AsQueryable();

            if (!string.IsNullOrEmpty(busca))
            {
                cursos = cursos.Where(l => l.Nome.Contains(busca) || l.Descricao.Contains(busca));
            }

            if (categoria != null)
            {
                cursos = cursos.Where(l => l.Categoria == categoria);
            }

            if (realizado != null)
            {
                var listCurso = new List<Curso>();
                if (realizado.Value)
                {
                    foreach (var c in cursos)
                    {
                        if (c.Realizado(User.Identity.GetUserId()))
                        {
                            listCurso.Add(c);
                        }
                    }
                }
                else
                {
                    foreach (var c in cursos)
                    {
                        if (!c.Realizado(User.Identity.GetUserId()))
                        {
                            listCurso.Add(c);
                        }
                    }
                }

                return PartialView(listCurso.OrderByDescending(l => l.Id).ToList());
            }
            return PartialView(cursos.OrderByDescending(l => l.Id).ToList());
        }

        public ActionResult Treinamento(int id)
        {
            var curso = _banco.Cursos.Find(id);
            if (curso == null)
            {
                return RedirectToAction("Index");
            }
            var participacao = new CursoParticipacao
            {
                IdUsuario = User.Identity.GetUserId(),
                DataInicio = DateTime.Now,
                IdCurso = id,
                Finalizado = false
            };

            if (curso.Categoria == TipoCurso.Imagem || curso.Categoria == TipoCurso.Link)
            {
                participacao.Finalizado = true;
                participacao.DataFim = DateTime.Now;
            }

            curso.Participacoes.Add(participacao);
            _banco.SaveChanges();

            if (curso.Categoria == TipoCurso.Link)
            {
                return Redirect(curso.Midia);
            }

            return View(curso);
        }

        public void Finalizar(int id)
        {
            var usuario = User.Identity.GetUserId();
            var item = _banco.CursoParticipacoes.OrderByDescending(c => c.Id).FirstOrDefault(c => c.IdCurso == id && c.IdUsuario == usuario && !c.Finalizado);
            if (item == null)
            {
                throw new ArgumentException("Item inválido");
            }

            item.Finalizado = true;
            item.DataFim = DateTime.Now;
            _banco.SaveChanges();
        }

        public void Excluir(int id)
        {
            var item = _banco.Cursos.Find(id);
            if (item == null)
            {
                throw new ArgumentException("Item inválido");
            }

            _banco.Cursos.Remove(item);
            _banco.SaveChanges();
        }

        public ActionResult Novo()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Novo(Curso model, HttpPostedFileBase arquivoFoto, HttpPostedFileBase arquivoAudio, string youtube)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Categoria == TipoCurso.Imagem)
            {
                if (arquivoFoto == null)
                {
                    return View(model);
                }

                var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(arquivoFoto.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/Treinamentos/"), fileName);
                arquivoFoto.SaveAs(path);
                model.Midia = fileName;
            }

            if (model.Categoria == TipoCurso.Audio)
            {
                if (arquivoAudio == null)
                {
                    return View(model);
                }

                var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(arquivoAudio.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/Treinamentos/"), fileName);
                arquivoAudio.SaveAs(path);
                model.Midia = fileName;
            }

            if (model.Categoria == TipoCurso.Video)
            {
                model.Midia = youtube;
            }

            _banco.Cursos.Add(model);
            _banco.SaveChanges();

            return RedirectToAction("Index", "EducacaoContinuada");
        }

        public ActionResult Editar(int id)
        {
            var curso = _banco.Cursos.Find(id);
            if (curso == null)
            {
                return RedirectToAction("Index");
            }

            return View(curso);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Editar(Curso model, HttpPostedFileBase arquivoFoto, HttpPostedFileBase arquivoAudio, string youtube)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Categoria == TipoCurso.Imagem && arquivoFoto != null)
            {
                var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(arquivoFoto.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/Treinamentos/"), fileName);
                arquivoFoto.SaveAs(path);
                model.Midia = fileName;
            }

            if (model.Categoria == TipoCurso.Audio && arquivoAudio != null)
            {
                var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(arquivoAudio.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/Treinamentos/"), fileName);
                arquivoAudio.SaveAs(path);
                model.Midia = fileName;
            }

            if (model.Categoria == TipoCurso.Video)
            {
                model.Midia = youtube;
            }

            _banco.Cursos.AddOrUpdate(model);
            _banco.SaveChanges();

            return RedirectToAction("Index", "EducacaoContinuada");
        }
    }
}
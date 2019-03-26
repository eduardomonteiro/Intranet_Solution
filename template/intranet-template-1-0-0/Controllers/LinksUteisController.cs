using $safeprojectname$.Entities;
using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Mvc;

namespace $safeprojectname$.Controllers
{
    [Authorize(Roles = "LinksUteis,LinksUteis-Admin")]
    public class LinksUteisController : BaseController
    {
        // GET: LinksUteis
        public ActionResult Index()
        {
            ViewBag.Area = "Gente e Gestão";
            ViewBag.Title = "Links Úteis";

            return View();
        }

        public ActionResult Lista(string busca, int? idEmpreendimento)
        {
            var links = _banco.LinksUteis.AsQueryable();

            if (!string.IsNullOrEmpty(busca))
            {
                links = links.Where(l => l.Titulo.Contains(busca) || l.Descricao.Contains(busca) ||
                                         l.Url.Contains(busca));
            }

            if (idEmpreendimento != null)
            {
                links = links.Where(l => l.IdEmpreendimento == idEmpreendimento);
            }
            return PartialView(links.OrderByDescending(l => l.Data).ToList());
        }

        public void Excluir(int id)
        {
            var item = _banco.LinksUteis.Find(id);
            if (item == null)
            {
                throw new ArgumentException("Item inválido");
            }

            _banco.LinksUteis.Remove(item);
            _banco.SaveChanges();
        }

        public ActionResult Novo()
        {
            ViewBag.Area = "Gente e Gestão";
            ViewBag.Title = "Links Úteis";

            return View();
        }

        [HttpPost]
        public ActionResult Novo(LinkUtil model)
        {
            ViewBag.Area = "Gente e Gestão";
            ViewBag.Title = "Links Úteis";

            if (ModelState.IsValid)
            {
                _banco.LinksUteis.Add(model);
                _banco.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Editar(int id)
        {
            ViewBag.Area = "Gente e Gestão";
            ViewBag.Title = "Links Úteis";

            var link = _banco.LinksUteis.Find(id);
            if (link == null)
            {
                return RedirectToAction("Index");
            }

            return View(link);
        }

        [HttpPost]
        public ActionResult Editar(LinkUtil model)
        {
            ViewBag.Area = "Gente e Gestão";
            ViewBag.Title = "Links Úteis";

            if (ModelState.IsValid)
            {
                _banco.LinksUteis.AddOrUpdate(model);
                _banco.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}
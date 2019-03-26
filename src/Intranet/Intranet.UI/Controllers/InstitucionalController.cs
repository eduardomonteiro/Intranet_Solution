using Intranet.Data.Enums;
using System.Linq;
using System.Web.Mvc;

namespace Intranet.UI.Controllers
{
    public class InstitucionalController : BaseController
    {
        // GET: Institucional
        public ActionResult Index(TipoInstitucional tipo)
        {
            var institucional = _banco.Institucionais.FirstOrDefault(i => i.Tipo == tipo);

            if (institucional == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Area = "Institucional";
            return View(institucional);
        }

        public ActionResult Diretoria()
        {
            ViewBag.Area = "Institucional";
            ViewBag.Title = "Diretoria";
            var diretoria = _banco.Diretorias.Where(d => d.Tipo == TipoDiretoria.Diretoria).OrderBy(x=> x.Ordem);
            return View(diretoria);
        }

        public ActionResult Gestores()
        {
            ViewBag.Area = "Institucional";
            ViewBag.Title = "Holding";
            //var gestores = _banco.Diretorias.Where(d => d.Tipo == TipoDiretoria.Gestores).OrderBy(x=> x.Ordem);
            ViewBag.Gerentes = _banco.Diretorias.Where(d => d.Cargo.Contains("Gerente") || d.Id == 15 || d.Id == 17).OrderBy(x => x.Ordem);
            ViewBag.Superintendentes = _banco.Diretorias.Where(d => d.Cargo.Contains("Superintendente") && d.Id != 15 && d.Id != 17).OrderBy(x => x.Nome);
            return View();
        }
    }
}
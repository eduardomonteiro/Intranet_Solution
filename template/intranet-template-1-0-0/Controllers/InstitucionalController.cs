using $safeprojectname$.Enums;
using System.Linq;
using System.Web.Mvc;

namespace $safeprojectname$.Controllers
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
            ViewBag.Title = "Diretoria Partage";
            var diretoria = _banco.Diretorias.Where(d => d.Tipo == TipoDiretoria.Diretoria);
            return View(diretoria);
        }

        public ActionResult Gestores()
        {
            ViewBag.Area = "Institucional";
            ViewBag.Title = "Gestores";
            var gestores = _banco.Diretorias.Where(d => d.Tipo == TipoDiretoria.Gestores);
            return View("Diretoria", gestores);
        }
    }
}
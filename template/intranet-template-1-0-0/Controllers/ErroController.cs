using System.Web.Mvc;

namespace $safeprojectname$.Controllers
{
    public class ErroController : BaseController
    {
        // GET: Erro
        public ActionResult Index()
        {
            ViewBag.Title = "Erro";
            return View();
        }
    }
}
using System.Web.Mvc;

namespace Intranet.UI.Controllers
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
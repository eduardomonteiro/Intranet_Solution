using Intranet.Data;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;
using Intranet.Data.Entities;

namespace Intranet.UI.Controllers
{
    public class BaseController : Controller
    {
        public readonly DataContext _banco = new DataContext();
        public string UsuarioId;
        public Usuario Usuario;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.Empreendimentos = _banco.Empreendimentos.OrderBy(e => e.Nome).ToList();
            UsuarioId = User.Identity.GetUserId();

            Usuario = _banco.Usuarios.Find(UsuarioId);

            ViewBag.Usuario = Usuario;

            base.OnActionExecuting(filterContext);
        }
    }
}
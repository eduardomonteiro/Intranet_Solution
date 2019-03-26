using $safeprojectname$.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace $safeprojectname$.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            var mes = DateTime.Now.Month;
            var ano = DateTime.Now.Year;
            var dia = DateTime.Now.Day;

            var home = new HomeVm
            {
                ComunicadosDestaque = _banco.Comunicados.Where(c => c.Destaque && ((Usuario.Empreendimento.Id == 10 ? true : (c.Shoppings.Contains(Usuario.Empreendimento.Id.ToString()) || c.Shoppings == "10")) || c.Cargos.Contains(Usuario.Cargo))).OrderByDescending(c => c.Data).Take(3).ToList(),
                Comunicados = _banco.Comunicados.Where(c => !c.Destaque && ((Usuario.Empreendimento.Id == 10 ? true : (c.Shoppings.Contains(Usuario.Empreendimento.Id.ToString()) || c.Shoppings == "10")) || c.Cargos.Contains(Usuario.Cargo))).OrderByDescending(c => c.Data).Take(3).ToList(),
                Eventos = _banco.Eventos
                    .Where(e => e.IdEmpreendimento == Usuario.IdEmpreendimento && e.DataInicio.Month == mes &&
                                e.DataInicio.Year == ano).OrderByDescending(c => c.DataInicio).ToList(),
                NovosColaboradores = _banco.Usuarios.OrderByDescending(u => u.DataCadastro).Take(3).ToList(),
                Aniversariantes = _banco.Usuarios.Where(u => u.DataNascimento.Month > mes || (u.DataNascimento.Day >= dia && u.DataNascimento.Month == mes))
                    .OrderBy(u => u.DataNascimento.Month).ThenBy(u=>u.DataNascimento.Day).Take(3).ToList(),
                LinksUteis = _banco.LinksUteis.OrderByDescending(l=>l.Data).Take(3).ToList(),
                Noticias = _banco.Noticias.OrderByDescending(n=>n.Data).Take(4).ToList()
            };

            return View(home);
        }
    }
}
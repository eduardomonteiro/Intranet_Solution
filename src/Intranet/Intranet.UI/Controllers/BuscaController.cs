using Intranet.UI.Models;
using Intranet.UI.Util;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Intranet.UI.Controllers
{
    public class BuscaController : BaseController
    {
        public ActionResult Index(string busca)
        {
            ViewBag.Area = "Busca";
            ViewBag.Title = busca;

            if (string.IsNullOrEmpty(busca))
            {
                ViewBag.Mensagem = "Preencha um termo na busca acima!";
                return View();
            }

            var buscaVm = new List<BuscaVm>();

            //Noticias
            var noticias = _banco.Noticias.Where(n => n.Titulo.Contains(busca) || n.Texto.Contains(busca));
            foreach (var noticia in noticias)
            {
                buscaVm.Add(new BuscaVm
                {
                    Titulo = noticia.Titulo,
                    Area = "Notícias",
                    Link = Url.Action("Detalhes", "Noticias", new { id = noticia.Id })
                });
            }

            //Institucional
            var institucional = _banco.Institucionais.Where(n => n.Titulo.Contains(busca) || n.Texto.Contains(busca));
            foreach (var insti in institucional)
            {
                buscaVm.Add(new BuscaVm
                {
                    Titulo = insti.Titulo,
                    Area = "Institucional",
                    Link = Url.Action("Index", "Institucional", new { tipo = insti.Tipo })
                });
            }

            //Diretoria
            var diretoria = _banco.Diretorias.Where(n => n.Nome.Contains(busca) || n.Descricao.Contains(busca));
            foreach (var dir in diretoria)
            {
                buscaVm.Add(new BuscaVm
                {
                    Titulo = dir.Nome,
                    Area = "Diretoria",
                    Link = Url.Action("Diretoria", "Institucional")
                });
            }

            //Documentos
            var documentos = _banco.Documentos.Where(n => n.Nome.Contains(busca));
            foreach (var doc in documentos)
            {
                buscaVm.Add(new BuscaVm
                {
                    Titulo = doc.Nome,
                    Area = EnumExtensions.EnumDisplayNameFor(doc.Tipo),
                    Link = Url.Action("Index", "Documentos", new { tipo = doc.Tipo })
                });
            }

            //Educação Continuada
            var educacao = _banco.Cursos.Where(n => n.Nome.Contains(busca) || n.Descricao.Contains(busca));
            foreach (var edu in educacao)
            {
                buscaVm.Add(new BuscaVm
                {
                    Titulo = edu.Nome,
                    Area = "Educação Continuada",
                    Link = Url.Action("Treinamento", "EducacaoContinuada", new { id = edu.Id })
                });
            }

            //Educação Continuada
            var links = _banco.LinksUteis.Where(n => n.Titulo.Contains(busca) || n.Descricao.Contains(busca));
            foreach (var link in links)
            {
                buscaVm.Add(new BuscaVm
                {
                    Titulo = link.Titulo,
                    Area = "Links Úteos",
                    Link = link.Url
                });
            }

            //Albuns
            var albuns = _banco.Albuns.Where(n => n.Titulo.Contains(busca));
            foreach (var album in albuns)
            {
                buscaVm.Add(new BuscaVm
                {
                    Titulo = album.Titulo,
                    Area = "Fotos dos Shoppings",
                    Link = Url.Action("Fotos", "Albuns", new { id = album.Id })
                });
            }

            //Comunicados
            var comunicados = _banco.Comunicados.Where(n => n.Titulo.Contains(busca));
            foreach (var comunic in comunicados)
            {
                buscaVm.Add(new BuscaVm
                {
                    Titulo = comunic.Titulo,
                    Area = "Comunicados",
                    Link = Url.Action("Detalhes", "Comunicados", new { id = comunic.Id })
                });
            }

            return View(buscaVm);
        }
    }
}
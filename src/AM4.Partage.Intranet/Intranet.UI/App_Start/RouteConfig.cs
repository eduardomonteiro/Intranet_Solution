using Intranet.Data.Enums;
using System.Web.Mvc;
using System.Web.Routing;

namespace Intranet.UI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Institucional1",
                "Institucional/Diretoria",
                new { controller = "Institucional", action = "Diretoria" }
            );

            routes.MapRoute(
                "Institucional2",
                "Institucional/Gestores",
                new { controller = "Institucional", action = "Gestores" }
            );

            routes.MapRoute(
                "Institucional3",
                "Institucional/{tipo}",
                new { controller = "Institucional", action = "Index", tipo = typeof(TipoInstitucional) }
            );

            routes.MapRoute(
                "DocumentosDownload",
                "Documentos/Download",
                new { controller = "Documentos", action = "Download" }
            );

            routes.MapRoute(
                "DocumentosExcluir",
                "Documentos/Excluir",
                new { controller = "Documentos", action = "Excluir" }
            );

            routes.MapRoute(
                "Documentos",
                "Documentos/{tipo}",
                new { controller = "Documentos", action = "Index", tipo = typeof(TipoInstitucional) }
            );

            routes.MapRoute(
                "Documentos1",
                "Documentos/{action}/{tipo}",
                new { controller = "Documentos", action = "Index", tipo = typeof(TipoInstitucional) }
            );

            routes.MapRoute(
                "Contato",
                "Contato/{tipo}",
                new { controller = "Contato", action = "Index", tipo = typeof(TipoContato) }
            );

            routes.MapRoute(
                "Contato2",
                "Contato/{action}/{tipo}",
                new { controller = "Contato", action = "Index", tipo = typeof(TipoInstitucional) }
            );

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
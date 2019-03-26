using System.Collections.Generic;
using $safeprojectname$.Entities;

namespace $safeprojectname$.Models
{
    public class HomeVm
    {
        public IEnumerable<Comunicado> ComunicadosDestaque { get; set; }
        public IEnumerable<Evento> Eventos { get; set; }
        public IEnumerable<Comunicado> Comunicados { get; set; }
        public IEnumerable<Usuario> NovosColaboradores { get; set; }
        public IEnumerable<Usuario> Aniversariantes { get; set; }
        public IEnumerable<LinkUtil> LinksUteis { get; set; }
        public IEnumerable<Noticia> Noticias { get; set; }
    }
}
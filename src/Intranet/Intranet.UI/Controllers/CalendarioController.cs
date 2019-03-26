using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Intranet.Data.Entities;
using Intranet.UI.Models;

namespace Intranet.UI.Controllers
{
    [Authorize(Roles = "Calendario,Calendario-Admin")]
    public class CalendarioController : BaseController
    {
        // GET: Calendario
        public ActionResult Index()
        {
            ViewBag.Title = "Calendário";
            var idUsuario = User.Identity.GetUserId();

            var usuario = _banco.Usuarios.First(u => u.Id == idUsuario);

            ViewBag.EmpreendimentoNome = usuario.Empreendimento.Nome;
            ViewBag.EmpreendimentoSigla = usuario.Empreendimento.Sigla;
            ViewBag.Cargo = usuario.Cargo;
            return View();
        }

        public JsonResult Eventos(int? id)
        {
             
             //IQueryable<Evento> eventosSupervisores = Enumerable.Empty<Evento>().AsQueryable();
             
            var eventos = _banco.Eventos.Where(c => c.SomenteDiretores == false).AsQueryable();
            var eventosSupervisores = _banco.Eventos.Where(c => c.SomenteDiretores == true).AsQueryable();


            if (User.IsInRole("Calendario-Localidade"))
            {
                eventos = eventos.Where(c => (Usuario.Empreendimento.Id == 10 ? true : (c.IdEmpreendimento.ToString().Contains(Usuario.Empreendimento.Id.ToString()) || c.IdEmpreendimento.ToString() == "10")));
            }
          

            if (id != null)
            {
                eventos = eventos.Where(e => e.IdEmpreendimento == id);
                if (Usuario.Cargo.ToLower().Contains("diretor") || Usuario.Cargo.ToLower().Contains("superintendentes"))
                {
                    eventos = eventos.Concat(eventosSupervisores.Where(c => c.IdEmpreendimento == id));
                }
            }
            else if (Usuario.Cargo.ToLower().Contains("diretor") || Usuario.Cargo.ToLower().Contains("superintendentes"))
            {    
                eventos = eventos.Concat(eventosSupervisores);                
            }

            

            var eventosVm = new List<CalendarioVm>();
            foreach (var evento in eventos.ToList())
            {
                eventosVm.Add(new CalendarioVm
                {
                    id = evento.Id,
                    start_date = evento.DataInicio.ToString("yyyy-MM-dd HH:mm"),
                    end_date = evento.DataFim.ToString("yyyy-MM-dd HH:mm"),
                    text = evento.Nome,
                    subject = evento.Empreendimento.Sigla,
                    save = evento.Descricao,
                    diretor = evento.SomenteDiretores
                });
            }

          
            

            return Json(eventosVm, JsonRequestBehavior.AllowGet);
        }

        public void Excluir(int id)
        {
            var evento = _banco.Eventos.Find(id);
            _banco.Eventos.Remove(evento);
            _banco.SaveChanges();
        }

        public void Adicionar(CalendarioVm evento)
        {
            var ev = new Evento
            {
                DataInicio = DateTime.Parse(evento.start_date),
                DataFim = DateTime.Parse(evento.end_date),
                Descricao = evento.save,
                Nome = evento.text,
                SomenteDiretores = evento.diretor,
                IdEmpreendimento = _banco.Empreendimentos.First(e=>e.Sigla == evento.subject).Id
            };

            _banco.Eventos.Add(ev);
            _banco.SaveChanges();
        }

        public void Atualizar(CalendarioVm evento)
        {
            var ev = _banco.Eventos.Find(evento.id);

            ev.DataInicio = DateTime.Parse(evento.start_date);
            ev.DataFim = DateTime.Parse(evento.end_date);
            ev.Descricao = evento.save;
            ev.Nome = evento.text;
            ev.SomenteDiretores = evento.diretor;
            ev.IdEmpreendimento = _banco.Empreendimentos.First(e => e.Sigla == evento.subject).Id;


            _banco.Eventos.AddOrUpdate(ev);
            _banco.SaveChanges();
        }
    }
}
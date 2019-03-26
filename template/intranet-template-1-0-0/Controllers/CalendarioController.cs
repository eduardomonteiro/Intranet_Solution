using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using $safeprojectname$.Entities;
using $safeprojectname$.Models;

namespace $safeprojectname$.Controllers
{
    [Authorize(Roles = "Calendario,Calendario-Admin")]
    public class CalendarioController : BaseController
    {
        // GET: Calendario
        public ActionResult Index()
        {
            ViewBag.Title = "Calendário";
            return View();
        }

        public JsonResult Eventos(int? id)
        {
            var eventos = _banco.Eventos.AsQueryable();

            if (id != null)
            {
                eventos = eventos.Where(e => e.IdEmpreendimento == id);
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
                    save = evento.Descricao
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
            ev.IdEmpreendimento = _banco.Empreendimentos.First(e => e.Sigla == evento.subject).Id;


            _banco.Eventos.AddOrUpdate(ev);
            _banco.SaveChanges();
        }
    }
}
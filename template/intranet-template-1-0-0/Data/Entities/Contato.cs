using $safeprojectname$.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace $safeprojectname$.Entities
{
    [Table("Contato")]
    public class Contato
    {
        public Contato()
        {
            Anexos = new HashSet<ContatoAnexo>();
            Interacoes = new HashSet<ContatoInteracao>();
        }

        public int Id { get; set; }

        [Display(Name = "Assunto *")]
        [Required]
        public string Assunto { get; set; }

        [Display(Name = "Mensagem *")]
        [Required]
        public string Mensagem { get; set; }

        [Display(Name = "Tipo de Solicitação *")]
        [Required]
        public TipoContato Tipo { get; set; }

        [Display(Name = "Empreendimento *")]
        [Required]
        public int IdEmpreendimento { get; set; }

        [Display(Name = "Data Abertura *")]
        [Required]
        public DateTime DataCriacao { get; set; }

        [Display(Name = "Data Finalização")]
        public DateTime? DataFinalizacao { get; set; }

        [Display(Name = "Status *")]
        [Required]
        public bool Finalizado { get; set; }

        [Display(Name = "Usuario *")]
        [Required]
        public string IdUsuario { get; set; }

        [ForeignKey("IdEmpreendimento")]
        public virtual Empreendimento Empreendimento { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual Usuario Usuario { get; set; }

        public string Ticket => DataCriacao.Year + Id.ToString("000000");

        public virtual ICollection<ContatoAnexo> Anexos { get; set; }
        public virtual ICollection<ContatoInteracao> Interacoes { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace $safeprojectname$.Entities
{
    [Table("Evento")]
    public class Evento
    {
        public int Id { get; set; }

        [Display(Name = "Nome *")]
        [Required]
        [StringLength(150)]
        public string Nome { get; set; }

        [Display(Name = "Empreendimento *")]
        [Required]
        public int IdEmpreendimento { get; set; }

        [Display(Name = "Descrição")]
        [StringLength(500)]
        public string Descricao { get; set; }

        [Display(Name = "Data Início *")]
        [Required]
        public DateTime DataInicio { get; set; }

        [Display(Name = "Data Fim *")]
        [Required]
        public DateTime DataFim { get; set; }

        [ForeignKey("IdEmpreendimento")]
        public virtual Empreendimento Empreendimento { get; set; }
    }
}
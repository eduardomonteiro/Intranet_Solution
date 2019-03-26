using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace $safeprojectname$.Entities
{
    [Table("LinkUtil")]
    public class LinkUtil
    {
        public int Id { get; set; }

        [Display(Name = "Título *")]
        [Required]
        [StringLength(150)]
        public string Titulo { get; set; }

        [Display(Name = "Empreendimento *")]
        [Required]
        public int IdEmpreendimento { get; set; }

        [Display(Name = "Descrição *")]
        [StringLength(500)]
        [Required]
        public string Descricao { get; set; }

        [Display(Name = "Data *")]
        [Required]
        public DateTime Data { get; set; }

        [Display(Name = "Link *")]
        [Required]
        [StringLength(500)]
        [Url]
        public string Url { get; set; }

        [ForeignKey("IdEmpreendimento")]
        public virtual Empreendimento Empreendimento { get; set; }
    }
}
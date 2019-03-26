using $safeprojectname$.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace $safeprojectname$.Entities
{
    [Table("Documento")]
    public class Documento
    {
        public int Id { get; set; }

        [Display(Name = "Nome do Arquivo *")]
        [Required]
        [StringLength(250)]
        public string Nome { get; set; }

        [Display(Name = "Empreendimento *")]
        [Required]
        public int IdEmpreendimento { get; set; }

        [Display(Name = "Categoria *")]
        [Required]
        public int IdCategoria { get; set; }

        public DateTime Data { get; set; }

        [Display(Name = "Arquivo *")]
        [StringLength(250)]
        public string Arquivo { get; set; }

        public TipoDocumento Tipo { get; set; }

        [ForeignKey("IdEmpreendimento")]
        public virtual Empreendimento Empreendimento { get; set; }

        [ForeignKey("IdCategoria")]
        public virtual DocumentoCategoria Categoria { get; set; }
    }
}
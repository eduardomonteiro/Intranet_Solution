using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intranet.Data.Entities
{
    [Table("ContatoInteracao")]
    public class ContatoInteracao
    {
        public int Id { get; set; }

        [Required]
        public int IdContato { get; set; }

        [Required]
        [Display(Name = "Mensagem *")]
        public string Mensagem { get; set; }

        [Required]
        public DateTime Data { get; set; }

        [Required]
        public string IdUsuario { get; set; }

        [ForeignKey("IdContato")]
        public virtual Contato Contato { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual Usuario Usuario { get; set; }
    }
}
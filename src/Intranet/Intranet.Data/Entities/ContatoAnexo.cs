using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intranet.Data.Entities
{
    [Table("ContatoAnexo")]
    public class ContatoAnexo
    {
        public int Id { get; set; }

        [Required]
        public int IdContato { get; set; }

        [Required]
        public string Arquivo { get; set; }

        [Required]
        public DateTime Data { get; set; }

        [Required]
        public string IdUsuario { get; set; }

        [ForeignKey("IdContato")]
        public virtual Contato Contato { get; set; }
    }
}
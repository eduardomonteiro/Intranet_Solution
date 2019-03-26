using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace $safeprojectname$.Entities
{
    [Table("ComunicadoAnexo")]
    public class ComunicadoAnexo
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Comunicado *")]
        public int IdComunicado { get; set; }

        [Required]
        [Display(Name = "Arquivo *")]
        public string Arquivo { get; set; }

        [Required]
        public DateTime Data { get; set; }

        [Required]
        public string IdUsuario { get; set; }

        [ForeignKey("IdComunicado")]
        public virtual Comunicado Comunicado { get; set; }
    }
}
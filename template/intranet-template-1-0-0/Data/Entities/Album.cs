using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace $safeprojectname$.Entities
{
    [Table("Album")]
    public class Album
    {
        public Album()
        {
            Fotos = new HashSet<AlbumFoto>();
        }

        public int Id { get; set; }

        [Display(Name = "Título *")]
        [Required]
        [StringLength(150)]
        public string Titulo { get; set; }

        [Display(Name = "Empreendimento *")]
        [Required]
        public int IdEmpreendimento { get; set; }

        [Display(Name = "Capa")]
        [StringLength(250)]
        public string Capa { get; set; }

        [ForeignKey("IdEmpreendimento")]
        public virtual Empreendimento Empreendimento { get; set; }

        public virtual ICollection<AlbumFoto> Fotos { get; set; }
    }
}
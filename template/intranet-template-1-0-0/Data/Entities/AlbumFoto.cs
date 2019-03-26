using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace $safeprojectname$.Entities
{
    [Table("AlbumFoto")]
    public class AlbumFoto
    {
        public int Id { get; set; }

        [Display(Name = "Álbum *")]
        [Required]
        public int IdAlbum { get; set; }

        [Display(Name = "Legenda")]
        [StringLength(250)]
        public string Legenda { get; set; }

        [Display(Name = "Imagem *")]
        [StringLength(250)]
        public string Imagem { get; set; }

        [Display(Name = "Crédito")]
        [StringLength(250)]
        public string Credito { get; set; }

        [Display(Name = "Data *")]
        [Required]
        public DateTime Data { get; set; }

        [ForeignKey("IdAlbum")]
        public virtual Album Album { get; set; }
    }
}
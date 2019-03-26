using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intranet.Data.Entities
{
    [Table("Noticia")]
    public class Noticia
    {
        public int Id { get; set; }

        [Display(Name = "Título *")]
        [Required]
        [StringLength(250)]
        public string Titulo { get; set; }

        [Display(Name = "Data *")]
        [Required]
        public DateTime Data { get; set; }

        [Display(Name = "Categoria *")]
        [Required]
        public int IdCategoria { get; set; }

        [Display(Name = "Texto *")]
        [Required]
        [DataType(DataType.Text)]
        public string Texto { get; set; }

        [Display(Name = "Imagem de Destaque")]
        [StringLength(250)]
        public string Imagem { get; set; }

        public bool Destaque { get; set; }

        [Display(Name = "Arquivo")]
        [StringLength(250)]
        public string Arquivo { get; set; }

        [ForeignKey("IdCategoria")]
        public virtual NoticiaCategoria Categoria { get; set; }
    }
}
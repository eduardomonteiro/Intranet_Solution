using $safeprojectname$.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace $safeprojectname$.Entities
{
    [Table("DocumentoCategoria")]
    public class DocumentoCategoria
    {
        public int Id { get; set; }

        public TipoDocumento Tipo { get; set; }

        [StringLength(250)]
        public string Nome { get; set; }
    }
}
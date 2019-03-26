using $safeprojectname$.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace $safeprojectname$.Entities
{
    [Table("Institucional")]
    public class Institucional
    {
        public int Id { get; set; }

        public TipoInstitucional Tipo { get; set; }

        [StringLength(250)]
        public string Titulo { get; set; }

        [StringLength(250)]
        public string Foto { get; set; }

        [DataType(DataType.Text)]
        public string Texto { get; set; }
    }
}
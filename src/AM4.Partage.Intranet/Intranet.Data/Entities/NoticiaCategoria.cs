using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intranet.Data.Entities
{
    [Table("NoticiaCategoria")]
    public class NoticiaCategoria
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string Nome { get; set; }
    }
}
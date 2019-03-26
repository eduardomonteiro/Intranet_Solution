using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intranet.Data.Entities
{
    [Table("Empreedimento")]
    public class Empreendimento
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string Nome { get; set; }

        [StringLength(5)]
        public string Sigla { get; set; }
    }
}
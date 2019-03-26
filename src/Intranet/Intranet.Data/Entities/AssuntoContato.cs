using Intranet.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet.Data.Entities
{
    [Table("AssuntoContato")]
    public class AssuntoContato
    {
        public int Id { get; set; }

        public TipoContato IdContato { get; set; }
        public int AssuntoId { get; set; }
        public virtual Assuntos Assuntos { get; set; }
    }
}

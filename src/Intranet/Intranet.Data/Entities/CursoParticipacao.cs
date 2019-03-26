using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intranet.Data.Entities
{
    [Table("CursoParticipacao")]
    public class CursoParticipacao
    {
        public int Id { get; set; }
        public int IdCurso { get; set; }
        public string IdUsuario { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public bool Finalizado { get; set; }

        [ForeignKey("IdCurso")]
        public virtual Curso Curso { get; set; }
    }
}
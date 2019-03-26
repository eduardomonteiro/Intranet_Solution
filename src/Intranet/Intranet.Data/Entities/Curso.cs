using Intranet.Data.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Intranet.Data.Entities
{
    public class Curso
    {
        public int Id { get; set; }

        [StringLength(100)]
        [Required]
        [Display(Name = "Título do Treinamento *")]
        public string Nome { get; set; }

        public TipoCurso Categoria { get; set; }

        [StringLength(250)]
        [Display(Name = "Arquivo")]
        public string Midia { get; set; }

        [Display(Name = "Tempo Padrão (seg.)")]
        public int? TempoValidacao { get; set; }

        [StringLength(500)]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Status")]
        public bool Ativo { get; set; }

        public virtual ICollection<CursoParticipacao> Participacoes { get; set; }

        public bool Realizado(string idUsuario)
        {
            return Participacoes.Any(p => p.IdUsuario == idUsuario && p.Finalizado);
        }
    }
}
using System.ComponentModel.DataAnnotations;

namespace Intranet.Data.Enums
{
    public enum Ordenacao
    {
        [Display(Name = "Mais Antigo")]
        MaisAntigo,

        [Display(Name = "Mais Recente")]
        MaisRecente,

        [Display(Name = "Nome")]
        Nome,

        [Display(Name = "Localidade")]
        Empreendimento 
    }
}
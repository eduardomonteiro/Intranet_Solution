using System.ComponentModel.DataAnnotations;

namespace Intranet.Data.Enums
{
    public enum TipoInstitucional
    {
        [Display(Name = "Conheça a Holding")]
        Conheca,

        [Display(Name = "Estrutura Organizacional")]
        Estrutura
    }
}
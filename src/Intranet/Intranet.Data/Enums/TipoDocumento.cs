using System.ComponentModel.DataAnnotations;

namespace Intranet.Data.Enums
{
    public enum TipoDocumento
    {
        [Display(Name = "Normas e Procedimentos")]
        NormasProcedimentos,

        [Display(Name = "Instrumentos Contratuais")]
        InstrumentosContratuais,

        [Display(Name = "Manual da Marca")]
        ManualMarca,

        [Display(Name = "Apresentações")]
        Apresentacoes,

        [Display(Name = "Espaço RH")]
        EspacoRh
    }
}
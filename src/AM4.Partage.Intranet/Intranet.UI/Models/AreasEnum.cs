using System.ComponentModel.DataAnnotations;

namespace Intranet.UI.Models
{
    public enum AreasEnum
    {
        [Display(Name = "Fotos dos Shoppings")]
        Albuns,

        [Display(Name = "Apresentações")]
        Apresentacoes,

        [Display(Name = "Calendário")]
        Calendario,

        Colaboradores,
        Comunicados,

        [Display(Name = "Educação Continuada")]
        EducacaoContinuada,

        [Display(Name = "Espaço RH")]
        EspacoRh,

        [Display(Name = "Fale Com RH")]
        FaleComRh,

        [Display(Name = "Instrumentos Contratuais")]
        InstrumentosContratuais,

        [Display(Name = "Links Úteis")]
        LinksUteis,

        [Display(Name = "Manual da Marca")]
        ManualMarca,

        [Display(Name = "Normas e Procedimentos")]
        NormasProcedimentos,

        [Display(Name = "Notícias")]
        Noticias,

        Ouvidoria,
        Perfis,

        [Display(Name = "Usuários")]
        Usuarios
    }
}
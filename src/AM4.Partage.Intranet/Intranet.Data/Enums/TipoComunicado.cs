using System.ComponentModel.DataAnnotations;

namespace Intranet.Data.Enums
{
    public enum TipoComunicado
    {
        [Display(Name = "Geral")]
        Geral,

        [Display(Name = "Boas vindas e transferências")]
        Boasindasetransferencias,

        [Display(Name = "Valores e cultura organizacional")]
        valoreseculturaorganizacional,

        [Display(Name = "Avisos e parcerias")]
        AvisoseParcerias,

        [Display(Name = "Campanhas institucionais")]
        CampanhasInstitucionais,

        [Display(Name = "Projetos e procedimentos internos")]
        ProjetoseProcedimentosInternos,

        [Display(Name = "Benefícios")]
        Beneficios,

        [Display(Name = "Folha de Pagamento")]
        FolhaDePagamento,

        [Display(Name = "Desligamento")]
        Desligamento,

        [Display(Name = "Aniversariantes e datas comemorativas")]
        AniversarianteseDatasComemorativas,

        [Display(Name = "Gestão de pessoas")]
        GestãoDepessoas,       
        
        [Display(Name = "Presidência")]
        Presidencia,

        [Display(Name = "Escritório e prédio corporativo")]
        EscritorioePredioCorporativo

    }
}
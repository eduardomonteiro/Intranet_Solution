using System.ComponentModel.DataAnnotations;

namespace Intranet.Data.Enums
{
    public enum TipoCurso
    {
        [Display(Name = "Vídeo do Youtube")]
        Video,

        Imagem,

        [Display(Name = "Áudio")]
        Audio,

        Link
    }
}
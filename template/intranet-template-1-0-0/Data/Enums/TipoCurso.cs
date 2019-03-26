using System.ComponentModel.DataAnnotations;

namespace $safeprojectname$.Enums
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
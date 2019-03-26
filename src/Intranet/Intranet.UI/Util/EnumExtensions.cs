using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Intranet.UI.Util
{
    public static class EnumExtensions
    {
        public static MvcHtmlString EnumDisplayNameFor(this HtmlHelper html, Enum item)
        {
            var type = item.GetType();
            var member = type.GetMember(item.ToString());
            DisplayAttribute displayname = (DisplayAttribute)member[0].GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();

            if (displayname != null)
            {
                return new MvcHtmlString(displayname.Name);
            }

            return new MvcHtmlString(item.ToString());
        }

        public static string EnumDisplayNameFor(Enum item)
        {
            var type = item.GetType();
            var member = type.GetMember(item.ToString());
            DisplayAttribute displayname = (DisplayAttribute)member[0].GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();

            if (displayname != null)
            {
                return displayname.Name;
            }

            return item.ToString();
        }

        public static MvcHtmlString TamanhoDocumento(this HtmlHelper html, string arquivo)
        {
            var path = HttpContext.Current.Server.MapPath("~/Content/Documentos/");
            if (arquivo == null)
            {
                return new MvcHtmlString("0 kb");
            }
            path = Path.Combine(path, arquivo);
            if (!File.Exists(path)) return new MvcHtmlString("0 kb");
            var file = new System.IO.FileInfo(path);

            return new MvcHtmlString((file.Length / 1024) + " kb");
        }

        public static MvcHtmlString TamanhoDocumento2(this HtmlHelper html, string arquivo)
        {
            var path = HttpContext.Current.Server.MapPath("~/Content/Comunicados/");
            if (arquivo == null)
            {
                return new MvcHtmlString("0 kb");
            }
            path = Path.Combine(path, arquivo);
            if (!File.Exists(path)) return new MvcHtmlString("0 kb");
            var file = new System.IO.FileInfo(path);

            return new MvcHtmlString((file.Length / 1024) + " kb");
        }

        public static MvcHtmlString TamanhoAnexo(this HtmlHelper html, string arquivo)
        {
            var path = HttpContext.Current.Server.MapPath("~/Content/Contato/");
            path = Path.Combine(path, arquivo);
            if (!File.Exists(path)) return new MvcHtmlString("0 kb");
            var file = new System.IO.FileInfo(path);

            return new MvcHtmlString((file.Length / 1024) + " kb");
        }

        public static MvcHtmlString TamanhoAnexoComunicado(this HtmlHelper html, string arquivo)
        {
            var path = HttpContext.Current.Server.MapPath("~/Content/Comunicados/");
            path = Path.Combine(path, arquivo);
            if (!File.Exists(path)) return new MvcHtmlString("0 kb");
            var file = new System.IO.FileInfo(path);

            return new MvcHtmlString((file.Length / 1024) + " kb");
        }

        public static MvcHtmlString TamanhoAnexoNoticia(this HtmlHelper html, string arquivo)
        {
            var path = HttpContext.Current.Server.MapPath("~/Content/Noticias/");
            path = Path.Combine(path, arquivo);
            if (!File.Exists(path)) return new MvcHtmlString("0 kb");
            var file = new System.IO.FileInfo(path);

            return new MvcHtmlString((file.Length / 1024) + " kb");
        }

        public static MvcHtmlString IconeArquivo(this HtmlHelper html, string arquivo)
        {
            string extensao = "";

            if (arquivo != null)
            {
                extensao = arquivo.ToLower().Split('.').Last();
            }

            if (".jpg, .jpeg, .png, .gif, .ai, .eps".Contains(extensao))
            {
                return new MvcHtmlString("<i class='fa fa-file-image-o' aria-hidden='true'></i>");
            }

            if (".pdf, .doc, .docx, .txt, .odt, .rtf".Contains(extensao))
            {
                return new MvcHtmlString("<i class='fa fa-file-text-o' aria-hidden='true'></i>");
            }

            if (".xls, .xlsx".Contains(extensao))
            {
                return new MvcHtmlString("<i class='fa fa-file-excel-o' aria-hidden='true'></i>");
            }

            if (".mp3, .aac, .ogg".Contains(extensao))
            {
                return new MvcHtmlString("<i class='fa fa-file-audio-o' aria-hidden='true'></i>");
            }

            return new MvcHtmlString("<i class='fa fa-file' aria-hidden='true'></i>");
        }
    }
}
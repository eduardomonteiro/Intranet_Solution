using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
    
namespace Intranet.UI.Util
{
    public static class Email
    {
        public static void EnviaEmail(string from, string to, string assunto, string body, string bcc = "")
        {
            body = body.Replace("~/", "http://191.252.61.42/");
            body = body.Replace("##data##", DateTime.Now.ToString());

            var objEmail = new MailMessage { From = new MailAddress("Shopping <informatica@partage.com.br>") };

            objEmail.ReplyToList.Add(new MailAddress(from));
            if (!string.IsNullOrEmpty(bcc))
            {
                objEmail.Bcc.Add(new MailAddress(bcc));
            }
            objEmail.To.Add(to);
            objEmail.Priority = MailPriority.Normal;
            objEmail.IsBodyHtml = true;
            objEmail.Subject = assunto;
            objEmail.Body = body;
            objEmail.SubjectEncoding = Encoding.GetEncoding("ISO-8859-1");
            objEmail.BodyEncoding = Encoding.GetEncoding("ISO-8859-1");

            var objSmtp = new SmtpClient
            {
                Host = "smtp-cluster.idc2.mandic.com.br",
                Port = 587,
                Credentials = new NetworkCredential("informatica@partage.com.br", "@part0000")
            };

            objSmtp.Send(objEmail);
        }

        public static bool ValidaEmail(string email)
        {
            var rg = new Regex(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");

            return rg.IsMatch(email);
        }
    }
}
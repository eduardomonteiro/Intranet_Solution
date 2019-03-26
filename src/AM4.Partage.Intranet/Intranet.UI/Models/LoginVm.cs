using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Intranet.UI.Models
{
    public class LoginVm
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email *")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha *")]
        public string Password { get; set; }

        [HiddenInput]
        public string ReturnUrl { get; set; }

        public bool LembrarMe { get; set; }
    }
}
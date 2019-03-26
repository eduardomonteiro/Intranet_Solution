using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intranet.Data.Entities
{
    public class Usuario
    {
        public Usuario()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public virtual bool EmailConfirmed { get; set; }

        public virtual string PasswordHash { get; set; }

        public virtual string SecurityStamp { get; set; }

        public virtual string PhoneNumber { get; set; }

        public virtual bool PhoneNumberConfirmed { get; set; }

        public virtual bool TwoFactorEnabled { get; set; }

        public virtual DateTime? LockoutEndDateUtc { get; set; }

        public virtual bool LockoutEnabled { get; set; }

        public virtual int AccessFailedCount { get; set; }

        public virtual string UserName { get; set; }

        [StringLength(250)]
        public string Avatar { get; set; }

        [StringLength(250)]
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [StringLength(250)]
        [Required]
        public string Nome { get; set; }

        [StringLength(250)]
        [Required]
        public string Cargo { get; set; }

        [StringLength(30)]        
        public string Sexo { get; set; }

        [StringLength(250)]        
        public string Telefone { get; set; }

        [StringLength(250)]
        public string Curriculo { get; set; }

        [Display(Name = "Empreendimento")]
        [Required]
        public int IdEmpreendimento { get; set; }

        [Display(Name = "Data de Nascimento *")]
        [Required]
        public DateTime DataNascimento { get; set; }

        [Display(Name = "Tipo Sanguíneo e Fator RH")]
        [StringLength(100)]
        public string TipoSangue { get; set; }

        [Display(Name = "Alergias")]
        [StringLength(100)]
        public string Alergias { get; set; }

        [Display(Name = "Telefone Residencial")]
        [StringLength(100)]
        public string TelefoneResidencial { get; set; }

        [Display(Name = "Telefone para Emergência")]
        [StringLength(100)]
        public string TelefoneEmergencia { get; set; }

        [Display(Name = "Telefone Comercial")]
        [StringLength(100)]
        public string TelefoneComercial { get; set; }

        [StringLength(250)]
        [EmailAddress]
        [Display(Name = "Email Particular")]
        public string EmailParticular { get; set; }

        [Display(Name = "Skype")]
        [StringLength(250)]
        public string Skype { get; set; }

        [Display(Name = "Linkedin")]
        [StringLength(250)]
        public string Linkedin { get; set; }

        [Display(Name = "Local de Nascimento")]
        [StringLength(250)]
        public string LocalNascimento { get; set; }

        [Display(Name = "Hobbies")]
        [StringLength(250)]
        public string Hobbies { get; set; }

        [Display(Name = "Interesses")]
        [StringLength(250)]
        public string Interesses { get; set; }

        [Display(Name = "Atividades Voluntárias")]
        [StringLength(250)]
        public string AtividadesVoluntarias { get; set; }

        [Display(Name = "Esporte")]
        [StringLength(250)]
        public string Esporte { get; set; }

        public DateTime DataCadastro { get; set; }

        [ForeignKey("IdEmpreendimento")]
        public virtual Empreendimento Empreendimento { get; set; }
    }
}
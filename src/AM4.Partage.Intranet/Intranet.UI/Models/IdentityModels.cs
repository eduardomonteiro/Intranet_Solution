using Intranet.Data.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Intranet.UI.Models
{
    // You will not likely need to customize there, but it is necessary/easier to create our own
    // project-specific implementations, so here they are:
    public class ApplicationUserLogin : IdentityUserLogin<string> { }

    public class ApplicationUserClaim : IdentityUserClaim<string> { }

    public class ApplicationUserRole : IdentityUserRole<string> { }

    // Must be expressed in terms of our custom Role and other types:
    public class ApplicationUser
        : IdentityUser<string, ApplicationUserLogin,
        ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();

            // Add any custom User properties/code here
        }

        [StringLength(250)]
        [Display(Name = "Foto")]
        public string Avatar { get; set; }

        [StringLength(250)]
        [EmailAddress]
        [Required]
        [Display(Name = "Email *")]
        public override string Email { get; set; }

        [StringLength(250)]
        [Required]
        [Display(Name = "Nome *")]
        public string Nome { get; set; }

        [StringLength(250)]
        [Required]
        [Display(Name = "Cargo *")]
        public string Cargo { get; set; }

        [StringLength(30)]
        //[Required]
        [Display(Name = "Sexo *")]
        public string Sexo { get; set; }

        [StringLength(250)]
        //[Required]
        [Display(Name = "Telefone Pessoal *")]
        public string Telefone { get; set; }

        [StringLength(250)]
        public string Curriculo { get; set; }

        [Display(Name = "Local de trabalho *")]
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

        [Display(Name = "Hobbies/Atividades de Lazer")]
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

        public async Task<ClaimsIdentity>
            GenerateUserIdentityAsync(ApplicationUserManager manager)
        {
            var userIdentity = await manager
                .CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }

    // Must be expressed in terms of our custom UserRole:
    public class ApplicationRole : IdentityRole<string, ApplicationUserRole>
    {
        public ApplicationRole()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public ApplicationRole(string name)
            : this()
        {
            this.Name = name;
        }

        // Add any custom Role properties/code here
    }

    // Must be expressed in terms of our custom types:
    public class ApplicationDbContext
        : IdentityDbContext<ApplicationUser, ApplicationRole,
        string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationDbContext()
            : base("DataContext")
        {
        }

        //static ApplicationDbContext()
        //{
        //    Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        //}

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        // Add the ApplicationGroups property:
        public virtual IDbSet<ApplicationGroup> ApplicationGroups { get; set; }

        // Override OnModelsCreating:
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationGroup>()
                .HasMany<ApplicationUserGroup>((ApplicationGroup g) => g.ApplicationUsers)
                .WithRequired().HasForeignKey<string>((ApplicationUserGroup ag) => ag.ApplicationGroupId);
            modelBuilder.Entity<ApplicationUserGroup>()
                .HasKey((ApplicationUserGroup r) =>
                    new
                    {
                        ApplicationUserId = r.ApplicationUserId,
                        ApplicationGroupId = r.ApplicationGroupId
                    }).ToTable("ApplicationUserGroups");

            modelBuilder.Entity<ApplicationGroup>()
                .HasMany<ApplicationGroupRole>((ApplicationGroup g) => g.ApplicationRoles)
                .WithRequired().HasForeignKey<string>((ApplicationGroupRole ap) => ap.ApplicationGroupId);
            modelBuilder.Entity<ApplicationGroupRole>().HasKey((ApplicationGroupRole gr) =>
                new
                {
                    ApplicationRoleId = gr.ApplicationRoleId,
                    ApplicationGroupId = gr.ApplicationGroupId
                }).ToTable("ApplicationGroupRoles");
        }
    }

    // Most likely won't need to customize these either, but they were needed because we implemented
    // custom versions of all the other types:
    public class ApplicationUserStore
        : UserStore<ApplicationUser, ApplicationRole, string,
            ApplicationUserLogin, ApplicationUserRole,
            ApplicationUserClaim>, IUserStore<ApplicationUser, string>,
        IDisposable
    {
        public ApplicationUserStore()
            : this(new IdentityDbContext())
        {
            base.DisposeContext = true;
        }

        public ApplicationUserStore(DbContext context)
            : base(context)
        {
        }
    }

    public class ApplicationRoleStore
    : RoleStore<ApplicationRole, string, ApplicationUserRole>,
    IQueryableRoleStore<ApplicationRole, string>,
    IRoleStore<ApplicationRole, string>, IDisposable
    {
        public ApplicationRoleStore()
            : base(new IdentityDbContext())
        {
            base.DisposeContext = true;
        }

        public ApplicationRoleStore(DbContext context)
            : base(context)
        {
        }
    }

    public class ApplicationGroup
    {
        public ApplicationGroup()
        {
            this.Id = Guid.NewGuid().ToString();
            this.ApplicationRoles = new List<ApplicationGroupRole>();
            this.ApplicationUsers = new List<ApplicationUserGroup>();
        }

        public ApplicationGroup(string name)
            : this()
        {
            this.Name = name;
        }

        public ApplicationGroup(string name, string description)
            : this(name)
        {
            this.Description = description;
        }

        [Key]
        public string Id { get; set; }

        [Display(Name = "Nome")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Descrição")]
        public string Description { get; set; }

        public DateTime Data { get; set; }
        public virtual ICollection<ApplicationGroupRole> ApplicationRoles { get; set; }
        public virtual ICollection<ApplicationUserGroup> ApplicationUsers { get; set; }
    }

    public class ApplicationUserGroup
    {
        public string ApplicationUserId { get; set; }
        public string ApplicationGroupId { get; set; }
    }

    public class ApplicationGroupRole
    {
        public string ApplicationGroupId { get; set; }
        public string ApplicationRoleId { get; set; }
    }
}
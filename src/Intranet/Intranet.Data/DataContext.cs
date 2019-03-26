using Intranet.Data.Entities;
using System.Data.Entity;

namespace Intranet.Data
{
    public class DataContext : DbContext
    {
        public DataContext()
            : base("name=DataContext")
        {
        }

        public virtual DbSet<Institucional> Institucionais { get; set; }
        public virtual DbSet<Diretoria> Diretorias { get; set; }
        public virtual DbSet<Empreendimento> Empreendimentos { get; set; }
        public virtual DbSet<LinkUtil> LinksUteis { get; set; }
        public virtual DbSet<Noticia> Noticias { get; set; }
        public virtual DbSet<NoticiaCategoria> NoticiasCategorias { get; set; }
        public virtual DbSet<Evento> Eventos { get; set; }
        public virtual DbSet<Album> Albuns { get; set; }
        public virtual DbSet<AlbumFoto> AlbumFotos { get; set; }
        public virtual DbSet<Documento> Documentos { get; set; }
        public virtual DbSet<Curso> Cursos { get; set; }
        public virtual DbSet<CursoParticipacao> CursoParticipacoes { get; set; }
        public virtual DbSet<Contato> Contato { get; set; }
        public virtual DbSet<AssuntoContato> AssuntoContato { get; set; }
        public virtual DbSet<Assuntos> Assuntos { get; set; }
        public virtual DbSet<ContatoInteracao> ContatoInteracao { get; set; }
        public virtual DbSet<ContatoAnexo> ContatoAnexo { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<Comunicado> Comunicados { get; set; }
        public virtual DbSet<ComunicadoAnexo> ComunicadoAnexos { get; set; }
        public virtual DbSet<DocumentoCategoria> DocumentosCategorias { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Id)
                .IsRequired()
                .HasMaxLength(128)
                .HasColumnType("nvarchar");

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256);

            modelBuilder.Entity<Usuario>()
                .Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(256);

            modelBuilder.Entity<Usuario>()
                .ToTable("AspNetUsers");

            modelBuilder.Entity<Contato>()
                .Property(u => u.IdUsuario)
                .IsRequired()
                .HasMaxLength(128)
                .HasColumnType("nvarchar");

            modelBuilder.Entity<ContatoInteracao>()
                .Property(u => u.IdUsuario)
                .IsRequired()
                .HasMaxLength(128)
                .HasColumnType("nvarchar");

            modelBuilder.Properties<string>().Configure(c => c.HasColumnType("varchar"));
        }
    }
}
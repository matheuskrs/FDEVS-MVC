using Microsoft.EntityFrameworkCore;
using FDevs.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FDevs.Data
{

    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Alternativa> Alternativas { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Modulo> Modulos { get; set; }
        public DbSet<Prova> Provas { get; set; }
        public DbSet<Questao> Questoes { get; set; }
        public DbSet<Resposta> Respostas { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Trilha> Trilhas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<UsuarioCurso> UsuarioCursos { get; set; }
        public DbSet<UsuarioEstadoVideo> UsuarioEstadoVideos { get; set; }
        public DbSet<UsuarioEstadoModulo> UsuarioEstadoModulos { get; set; }
        public DbSet<UsuarioEstadoCurso> UsuarioEstadoCursos { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Progresso> Progressos { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            AppDbSeed seed = new(modelBuilder);
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.SetTableName(entityType.DisplayName());

                entityType.GetForeignKeys()
                    .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade)
                    .ToList()
                    .ForEach(fk => fk.DeleteBehavior = DeleteBehavior.Restrict);
            }
            modelBuilder.Entity<Progresso>().HasNoKey(); // DTO de progresso

            #region Configuração do Muitos para Muitos - UsuarioCurso
            // Definindo Chave Primária
            modelBuilder.Entity<UsuarioCurso>()
                .HasKey(uc => new { uc.UsuarioId, uc.CursoId });
            #endregion

            #region Configuração do Muitos para Muitos - UsuarioEstadoVideo
            // Definindo Chave Primária
            modelBuilder.Entity<UsuarioEstadoVideo>()
                .HasKey(uev => new { uev.UsuarioId, uev.EstadoId, uev.VideoId });
            #endregion

            #region Configuração do Muitos para Muitos - UsuarioEstadoModulo
            // Definindo Chave Primária
            modelBuilder.Entity<UsuarioEstadoModulo>()
                .HasKey(uec => new { uec.UsuarioId, uec.EstadoId, uec.ModuloId });
            #endregion

            #region Configuração do Muitos para Muitos - UsuarioEstadoCurso
            // Definindo Chave Primária
            modelBuilder.Entity<UsuarioEstadoCurso>()
                .HasKey(uec => new { uec.UsuarioId, uec.EstadoId, uec.CursoId });
            #endregion
        }
    }
}
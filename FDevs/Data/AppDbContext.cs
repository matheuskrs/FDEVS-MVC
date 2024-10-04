using Microsoft.EntityFrameworkCore;
using FDevs.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FDevs.Data;

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
    public DbSet<Video> Videos { get; set; }
    

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
        
        #region Configuração do Muitos para Muitos - JogoGenero
        // Definindo Chave Primária
        modelBuilder.Entity<UsuarioCurso>()
            .HasKey(uc => new { uc.UsuarioId, uc.CursoId });

        #endregion

    }

}
using FDevs.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FDevs.Data
{


    public class AppDbSeed
    {
        public AppDbSeed(ModelBuilder builder)
        {

            #region Populate Roles - Perfis de Usuário
            List<IdentityRole> roles = new()
        {
            new IdentityRole() {
               Id = "0b44ca04-f6b0-4a8f-a953-1f2330d30894",
               Name = "Administrador",
               NormalizedName = "ADMINISTRADOR"
            },
            new IdentityRole() {
               Id = "bec71b05-8f3d-4849-88bb-0e8d518d2de8",
               Name = "Usuário",
               NormalizedName = "USUÁRIO"
            },
            new IdentityRole() {
               Id = "ddf093a6-6cb5-4ff7-9a64-83da34aee005",
               Name = "Moderador",
               NormalizedName = "MODERADOR"
            },
        };
            builder.Entity<IdentityRole>().HasData(roles);
            #endregion

            #region Populate IdentityUser
            List<IdentityUser> users = new() {
            new IdentityUser(){
                Id = "ddf093a6-6cb5-4ff7-9a64-83da34aee005",
                Email = "admin@fdevs.com",
                NormalizedEmail = "ADMIN@FDEVS.COM",
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                LockoutEnabled = false,
                EmailConfirmed = true,
            }
        };
            foreach (var user in users)
            {
                PasswordHasher<IdentityUser> pass = new();
                user.PasswordHash = pass.HashPassword(user, "@Admin123");
            }
            builder.Entity<IdentityUser>().HasData(users);

            List<Usuario> usuarios = new(){
            new Usuario(){
                UsuarioId = users[0].Id,
                Nome = "Matheus Kauan Rodrigues de Souza",
                DataNascimento = DateTime.Parse("05/08/1981"),
                Foto = "/img/usuarios/avatar.png"
            }
        };
            builder.Entity<Usuario>().HasData(usuarios);
            #endregion

            #region Populate UserRole - Usuário com Perfil
            List<IdentityUserRole<string>> userRoles = new()
        {
            new IdentityUserRole<string>() {
                UserId = users[0].Id,
                RoleId = roles[0].Id
            },
            new IdentityUserRole<string>() {
                UserId = users[0].Id,
                RoleId = roles[1].Id
            },
            new IdentityUserRole<string>() {
                UserId = users[0].Id,
                RoleId = roles[2].Id
            }
        };
            builder.Entity<IdentityUserRole<string>>().HasData(userRoles);
            #endregion

            #region Populate Estado
            List<Estado> estados = new()
        {
            new Estado { Id = 1, Nome = "Em andamento", Cor = "rgb(255, 255, 0)" }, // Amarelo
            new Estado { Id = 2, Nome = "Concluído", Cor = "rgb(0, 255, 0)" },    // Verde
            new Estado { Id = 3, Nome = "Não iniciado", Cor = "rgb(255, 0, 0)" }  // Vermelho
        };
            builder.Entity<Estado>().HasData(estados);
            #endregion

            #region Populate Trilha
            List<Trilha> trilhas = new()
        {
            new Trilha { Id = 1, Nome = "Trilha de Backend", Foto = "https://bipper-treinamentos-qa.s3.amazonaws.com/BipperDocs/CapasTrilha/1.png" }
        };
            builder.Entity<Trilha>().HasData(trilhas);
            #endregion

            #region Populate Curso
            List<Curso> cursos = new(){
            new Curso { Id = 1, Nome = "Lógica de programação", Foto = "https://bipper-treinamentos-qa.s3.amazonaws.com/BipperDocs/Capas/18.png", TrilhaId = 1 },
            new Curso { Id = 2, Nome = "Banco de Dados", Foto = "https://bipper-treinamentos-qa.s3.amazonaws.com/BipperDocs/Capas/19.png", TrilhaId = 1 }
        };
            builder.Entity<Curso>().HasData(cursos);
            #endregion

            #region Populate Prova
            List<Prova> provas = new()
        {
            new Prova { Id = 1, Nome = "Prova de Lógica da Programação", CursoId = 1 },
            new Prova { Id = 2, Nome = "Prova de Banco de Dados", CursoId = 2 }
        };
            builder.Entity<Prova>().HasData(provas);
            #endregion

            #region Populate Modulo
            List<Modulo> modulos = new()
        {
            new Modulo { Id = 1, Nome = "Módulo 1 - Iniciante", CursoId = 1 },
            new Modulo { Id = 2, Nome = "Módulo 1 - Iniciante", CursoId = 2 },
            new Modulo { Id = 3, Nome = "Módulo 2 - Intermediário", CursoId = 2 }
        };
            builder.Entity<Modulo>().HasData(modulos);
            #endregion

            #region Populate Video
            List<Video> videos = new()
        {
            new Video { Id = 1, Titulo = "Introdução a Algoritmos", URL = "https://www.youtube.com/embed/8mei6uVttho?si=gn2VgTONcmRet24o", ModuloId = 1 },
            new Video { Id = 2, Titulo = "Primeiro algoritmo", URL = "https://www.youtube.com/embed/M2Af7gkbbro?si=yx5Yy6dgQYy_1Y8f", ModuloId = 1 },
            new Video { Id = 3, Titulo = "Comando de Entrada e Operadores", URL = "https://www.youtube.com/embed/RDrfZ-7WE8c?si=JP0LvntY7_cxuWUB", ModuloId = 1 },
            new Video { Id = 4, Titulo = "Operadores lógicos e relacionais", URL = "https://www.youtube.com/embed/Ig4QZNpVZYs?si=Eaes88_HwJc28Vp2", ModuloId = 1 },
            new Video { Id = 5, Titulo = "Introdução ao Scratch", URL = "https://www.youtube.com/embed/GrPkuk1ezyo?si=QoDgOp2ZVSgM_CTM", ModuloId = 1 },
            new Video { Id = 6, Titulo = "Exercícios de Algoritmo", URL = "https://www.youtube.com/embed/v2nCgGSVCeE?si=_-lFdQVYxv_1uJVB", ModuloId = 1 },
            new Video { Id = 7, Titulo = "Estruturas Condicionais 1", URL = "https://www.youtube.com/embed/_g05aHdBAEY?si=YHLhKkoo8Cnaieub", ModuloId = 1 },
            new Video { Id = 8, Titulo = "SQL Server - Instalando no seu computador", URL = "https://www.youtube.com/embed/OKqpZ6zbZwQ?si=PR8tj46glLT1VUyD", ModuloId = 2 },
            new Video { Id = 9, Titulo = "Orientações", URL = "https://www.youtube.com/embed/qEitmEuXG1I?si=71gXL6ykXdoTHoxk", ModuloId = 2 },
            new Video { Id = 10, Titulo = "Conceitos Essenciais e Modelagem", URL = "https://www.youtube.com/embed/N_0ujgVRrdI?si=kmYxFk0v6jv0SXSc", ModuloId = 2 },
            new Video { Id = 11, Titulo = "Relacionamento entre tabelas", URL = "https://www.youtube.com/embed/HmFUrlQcCJ0?si=-E4k0khkUdH9ABS3", ModuloId = 3 }
        };
            builder.Entity<Video>().HasData(videos);
            #endregion

            #region Populate Questao
            List<Questao> questoes = new()
        {
            new Questao { Id = 1, Texto = "O que é uma função na programação?", ProvaId = 1 },
            new Questao { Id = 2, Texto = "Qual a diferença entre um loop while e um loop for?", ProvaId = 1 },
            new Questao { Id = 3, Texto = "O que é um banco de dados?", ProvaId = 2 }
        };
            builder.Entity<Questao>().HasData(questoes);
            #endregion

            #region Populate Alternativa
            List<Alternativa> alternativas = new()
        {
            new Alternativa { Id = 1, Texto = "É um meio de armazenar dados", Correta = false, QuestaoId = 1 },
            new Alternativa { Id = 2, Texto = "É um bloco de código que pode ser chamado várias vezes", Correta = true, QuestaoId = 1 },
            new Alternativa { Id = 3, Texto = "Um é repetido infinitamente, e o outro até que um valor seja verdadeiro", Correta = false, QuestaoId = 2 },
            new Alternativa { Id = 4, Texto = "O loop while se repete até que um valor seja verdadeiro, e o loop for até que a iteração seja concluída uma certa quantidade de vezes.", Correta = true, QuestaoId = 2 },
            new Alternativa { Id = 5, Texto = "É uma sequência de comandos", Correta = false, QuestaoId = 3 },
            new Alternativa { Id = 6, Texto = "É uma estrutura de armazenamento de dados", Correta = true, QuestaoId = 3 }
        };
            builder.Entity<Alternativa>().HasData(alternativas);
            #endregion

            #region Populate Resposta
            List<Resposta> respostas = new()
        {
            new Resposta { Id = 1, UsuarioId = "ddf093a6-6cb5-4ff7-9a64-83da34aee005", QuestaoId = 1, AlternativaId = 1 },
            new Resposta { Id = 2, UsuarioId = "ddf093a6-6cb5-4ff7-9a64-83da34aee005", QuestaoId = 2, AlternativaId = 4 },
            new Resposta { Id = 3, UsuarioId = "ddf093a6-6cb5-4ff7-9a64-83da34aee005", QuestaoId = 3, AlternativaId = 5 }
        };
            builder.Entity<Resposta>().HasData(respostas);
            #endregion

            #region Populate UsuarioEstadoVideo
            List<UsuarioEstadoVideo> usuarioEstadoVideos = new()
        {
            new UsuarioEstadoVideo { UsuarioId = "ddf093a6-6cb5-4ff7-9a64-83da34aee005", EstadoId = 2, VideoId = 1 },
            new UsuarioEstadoVideo { UsuarioId = "ddf093a6-6cb5-4ff7-9a64-83da34aee005", EstadoId = 3, VideoId = 2 },
            new UsuarioEstadoVideo { UsuarioId = "ddf093a6-6cb5-4ff7-9a64-83da34aee005", EstadoId = 3, VideoId = 3 },
            new UsuarioEstadoVideo { UsuarioId = "ddf093a6-6cb5-4ff7-9a64-83da34aee005", EstadoId = 3, VideoId = 4 },
            new UsuarioEstadoVideo { UsuarioId = "ddf093a6-6cb5-4ff7-9a64-83da34aee005", EstadoId = 3, VideoId = 5 },
            new UsuarioEstadoVideo { UsuarioId = "ddf093a6-6cb5-4ff7-9a64-83da34aee005", EstadoId = 3, VideoId = 6 },
            new UsuarioEstadoVideo { UsuarioId = "ddf093a6-6cb5-4ff7-9a64-83da34aee005", EstadoId = 2, VideoId = 7 },
            new UsuarioEstadoVideo { UsuarioId = "ddf093a6-6cb5-4ff7-9a64-83da34aee005", EstadoId = 2, VideoId = 8 },
            new UsuarioEstadoVideo { UsuarioId = "ddf093a6-6cb5-4ff7-9a64-83da34aee005", EstadoId = 2, VideoId = 9 },
            new UsuarioEstadoVideo { UsuarioId = "ddf093a6-6cb5-4ff7-9a64-83da34aee005", EstadoId = 2, VideoId = 10 },
            new UsuarioEstadoVideo { UsuarioId = "ddf093a6-6cb5-4ff7-9a64-83da34aee005", EstadoId = 2, VideoId = 11 }
        };
            builder.Entity<UsuarioEstadoVideo>().HasData(usuarioEstadoVideos);
            #endregion

            #region Populate UsuarioEstadoModulo
            List<UsuarioEstadoModulo> usuarioEstadoModulos = new(){
            new UsuarioEstadoModulo { UsuarioId = "ddf093a6-6cb5-4ff7-9a64-83da34aee005", ModuloId = 1, EstadoId = 1 },
            new UsuarioEstadoModulo { UsuarioId = "ddf093a6-6cb5-4ff7-9a64-83da34aee005", ModuloId = 2, EstadoId = 2 },
            new UsuarioEstadoModulo { UsuarioId = "ddf093a6-6cb5-4ff7-9a64-83da34aee005", ModuloId = 3, EstadoId = 2 },
        };
            builder.Entity<UsuarioEstadoModulo>().HasData(usuarioEstadoModulos);
            #endregion

            #region Populate UsuarioEstadoCurso
            List<UsuarioEstadoCurso> usuarioEstadoCursos = new(){
            new UsuarioEstadoCurso { UsuarioId = "ddf093a6-6cb5-4ff7-9a64-83da34aee005", EstadoId = 1, CursoId = 1 },
            new UsuarioEstadoCurso { UsuarioId = "ddf093a6-6cb5-4ff7-9a64-83da34aee005", EstadoId = 2, CursoId = 2 }
        };
            builder.Entity<UsuarioEstadoCurso>().HasData(usuarioEstadoCursos);
            #endregion

            #region Populate UsuarioCurso
            List<UsuarioCurso> usuarioCursos = new()
        {
            new UsuarioCurso { UsuarioId = "ddf093a6-6cb5-4ff7-9a64-83da34aee005", CursoId = 1 },
            new UsuarioCurso { UsuarioId = "ddf093a6-6cb5-4ff7-9a64-83da34aee005", CursoId = 2 }
        };
            builder.Entity<UsuarioCurso>().HasData(usuarioCursos);
            #endregion
        }
    }
}

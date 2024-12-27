using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FDevs.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Estado",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Cor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estado", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityRole",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Progresso",
                columns: table => new
                {
                    QtdCursos = table.Column<int>(type: "int", nullable: true),
                    QtdAndamento = table.Column<int>(type: "int", nullable: true),
                    QtdConcluido = table.Column<int>(type: "int", nullable: true),
                    QtdNaoIniciado = table.Column<int>(type: "int", nullable: true),
                    ProgressoVermelho = table.Column<double>(type: "float", nullable: true),
                    ProgressoAmarelo = table.Column<double>(type: "float", nullable: true),
                    ProgressoVerde = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Trilha",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Foto = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trilha", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityRoleClaim<string>",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityRoleClaim<string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdentityRoleClaim<string>_IdentityRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "IdentityRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IdentityUserClaim<string>",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUserClaim<string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdentityUserClaim<string>_IdentityUser_UserId",
                        column: x => x.UserId,
                        principalTable: "IdentityUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IdentityUserLogin<string>",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUserLogin<string>", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_IdentityUserLogin<string>_IdentityUser_UserId",
                        column: x => x.UserId,
                        principalTable: "IdentityUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IdentityUserRole<string>",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUserRole<string>", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_IdentityUserRole<string>_IdentityRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "IdentityRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IdentityUserRole<string>_IdentityUser_UserId",
                        column: x => x.UserId,
                        principalTable: "IdentityUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IdentityUserToken<string>",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUserToken<string>", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_IdentityUserToken<string>_IdentityUser_UserId",
                        column: x => x.UserId,
                        principalTable: "IdentityUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Foto = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.UsuarioId);
                    table.ForeignKey(
                        name: "FK_Usuario_IdentityUser_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "IdentityUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Curso",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Foto = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TrilhaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Curso", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Curso_Trilha_TrilhaId",
                        column: x => x.TrilhaId,
                        principalTable: "Trilha",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Modulo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CursoId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modulo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Modulo_Curso_CursoId",
                        column: x => x.CursoId,
                        principalTable: "Curso",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Modulo_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId");
                });

            migrationBuilder.CreateTable(
                name: "Prova",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CursoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prova", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prova_Curso_CursoId",
                        column: x => x.CursoId,
                        principalTable: "Curso",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioCurso",
                columns: table => new
                {
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CursoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioCurso", x => new { x.UsuarioId, x.CursoId });
                    table.ForeignKey(
                        name: "FK_UsuarioCurso_Curso_CursoId",
                        column: x => x.CursoId,
                        principalTable: "Curso",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsuarioCurso_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioEstadoCurso",
                columns: table => new
                {
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EstadoId = table.Column<int>(type: "int", nullable: false),
                    CursoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioEstadoCurso", x => new { x.UsuarioId, x.EstadoId, x.CursoId });
                    table.ForeignKey(
                        name: "FK_UsuarioEstadoCurso_Curso_CursoId",
                        column: x => x.CursoId,
                        principalTable: "Curso",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsuarioEstadoCurso_Estado_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "Estado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsuarioEstadoCurso_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioEstadoModulo",
                columns: table => new
                {
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EstadoId = table.Column<int>(type: "int", nullable: false),
                    ModuloId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioEstadoModulo", x => new { x.UsuarioId, x.EstadoId, x.ModuloId });
                    table.ForeignKey(
                        name: "FK_UsuarioEstadoModulo_Estado_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "Estado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsuarioEstadoModulo_Modulo_ModuloId",
                        column: x => x.ModuloId,
                        principalTable: "Modulo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsuarioEstadoModulo_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Video",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    URL = table.Column<string>(type: "nvarchar(700)", maxLength: 700, nullable: false),
                    ModuloId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Video", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Video_Modulo_ModuloId",
                        column: x => x.ModuloId,
                        principalTable: "Modulo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Questao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Texto = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ProvaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questao_Prova_ProvaId",
                        column: x => x.ProvaId,
                        principalTable: "Prova",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioEstadoVideo",
                columns: table => new
                {
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EstadoId = table.Column<int>(type: "int", nullable: false),
                    VideoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioEstadoVideo", x => new { x.UsuarioId, x.EstadoId, x.VideoId });
                    table.ForeignKey(
                        name: "FK_UsuarioEstadoVideo_Estado_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "Estado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsuarioEstadoVideo_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsuarioEstadoVideo_Video_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Video",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Alternativa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Texto = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Correta = table.Column<bool>(type: "bit", nullable: false),
                    QuestaoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alternativa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alternativa_Questao_QuestaoId",
                        column: x => x.QuestaoId,
                        principalTable: "Questao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Resposta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    QuestaoId = table.Column<int>(type: "int", nullable: false),
                    AlternativaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resposta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resposta_Alternativa_AlternativaId",
                        column: x => x.AlternativaId,
                        principalTable: "Alternativa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Resposta_Questao_QuestaoId",
                        column: x => x.QuestaoId,
                        principalTable: "Questao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Resposta_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Estado",
                columns: new[] { "Id", "Cor", "Nome" },
                values: new object[,]
                {
                    { 1, "rgb(255, 255, 0)", "Em andamento" },
                    { 2, "rgb(0, 255, 0)", "Concluído" },
                    { 3, "rgb(255, 0, 0)", "Não iniciado" }
                });

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0b44ca04-f6b0-4a8f-a953-1f2330d30894", null, "Administrador", "ADMINISTRADOR" },
                    { "bec71b05-8f3d-4849-88bb-0e8d518d2de8", null, "Usuário", "USUÁRIO" },
                    { "ddf093a6-6cb5-4ff7-9a64-83da34aee005", null, "Moderador", "MODERADOR" }
                });

            migrationBuilder.InsertData(
                table: "IdentityUser",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "ddf093a6-6cb5-4ff7-9a64-83da34aee005", 0, "223c2b3a-d332-4ca8-9384-0f4cf378d170", "admin@fdevs.com", true, false, null, "ADMIN@FDEVS.COM", "ADMIN", "AQAAAAIAAYagAAAAEAyqNvUqLjOWxtG+HhCh+5AYP40QZYchKBDoadjTzzEno4nBwBl4RqmKan3SkmwNrA==", null, false, "d1f34fcb-4112-4b15-be7e-2b6090b373b1", false, "Admin" });

            migrationBuilder.InsertData(
                table: "Trilha",
                columns: new[] { "Id", "Foto", "Nome" },
                values: new object[] { 1, "https://bipper-treinamentos-qa.s3.amazonaws.com/BipperDocs/CapasTrilha/1.png", "Trilha de Backend" });

            migrationBuilder.InsertData(
                table: "Curso",
                columns: new[] { "Id", "Foto", "Nome", "TrilhaId" },
                values: new object[,]
                {
                    { 1, "https://bipper-treinamentos-qa.s3.amazonaws.com/BipperDocs/Capas/18.png", "Lógica de programação", 1 },
                    { 2, "https://bipper-treinamentos-qa.s3.amazonaws.com/BipperDocs/Capas/19.png", "Banco de Dados", 1 }
                });

            migrationBuilder.InsertData(
                table: "IdentityUserRole<string>",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "0b44ca04-f6b0-4a8f-a953-1f2330d30894", "ddf093a6-6cb5-4ff7-9a64-83da34aee005" },
                    { "bec71b05-8f3d-4849-88bb-0e8d518d2de8", "ddf093a6-6cb5-4ff7-9a64-83da34aee005" },
                    { "ddf093a6-6cb5-4ff7-9a64-83da34aee005", "ddf093a6-6cb5-4ff7-9a64-83da34aee005" }
                });

            migrationBuilder.InsertData(
                table: "Usuario",
                columns: new[] { "UsuarioId", "DataNascimento", "Foto", "IsAdmin", "Nome" },
                values: new object[] { "ddf093a6-6cb5-4ff7-9a64-83da34aee005", new DateTime(1981, 8, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "/img/usuarios/avatar.png", false, "Matheus Kauan Rodrigues de Souza" });

            migrationBuilder.InsertData(
                table: "Modulo",
                columns: new[] { "Id", "CursoId", "Nome", "UsuarioId" },
                values: new object[,]
                {
                    { 1, 1, "Módulo 1 - Iniciante", null },
                    { 2, 2, "Módulo 1 - Iniciante", null },
                    { 3, 2, "Módulo 2 - Intermediário", null }
                });

            migrationBuilder.InsertData(
                table: "Prova",
                columns: new[] { "Id", "CursoId", "Nome" },
                values: new object[,]
                {
                    { 1, 1, "Prova de Lógica da Programação" },
                    { 2, 2, "Prova de Banco de Dados" }
                });

            migrationBuilder.InsertData(
                table: "UsuarioCurso",
                columns: new[] { "CursoId", "UsuarioId" },
                values: new object[,]
                {
                    { 1, "ddf093a6-6cb5-4ff7-9a64-83da34aee005" },
                    { 2, "ddf093a6-6cb5-4ff7-9a64-83da34aee005" }
                });

            migrationBuilder.InsertData(
                table: "UsuarioEstadoCurso",
                columns: new[] { "CursoId", "EstadoId", "UsuarioId" },
                values: new object[,]
                {
                    { 1, 1, "ddf093a6-6cb5-4ff7-9a64-83da34aee005" },
                    { 2, 2, "ddf093a6-6cb5-4ff7-9a64-83da34aee005" }
                });

            migrationBuilder.InsertData(
                table: "Questao",
                columns: new[] { "Id", "ProvaId", "Texto" },
                values: new object[,]
                {
                    { 1, 1, "O que é uma função na programação?" },
                    { 2, 1, "Qual a diferença entre um loop while e um loop for?" },
                    { 3, 2, "O que é um banco de dados?" }
                });

            migrationBuilder.InsertData(
                table: "UsuarioEstadoModulo",
                columns: new[] { "EstadoId", "ModuloId", "UsuarioId" },
                values: new object[,]
                {
                    { 1, 1, "ddf093a6-6cb5-4ff7-9a64-83da34aee005" },
                    { 2, 2, "ddf093a6-6cb5-4ff7-9a64-83da34aee005" },
                    { 2, 3, "ddf093a6-6cb5-4ff7-9a64-83da34aee005" }
                });

            migrationBuilder.InsertData(
                table: "Video",
                columns: new[] { "Id", "ModuloId", "Titulo", "URL" },
                values: new object[,]
                {
                    { 1, 1, "Introdução a Algoritmos", "https://www.youtube.com/embed/8mei6uVttho?si=gn2VgTONcmRet24o" },
                    { 2, 1, "Primeiro algoritmo", "https://www.youtube.com/embed/M2Af7gkbbro?si=yx5Yy6dgQYy_1Y8f" },
                    { 3, 1, "Comando de Entrada e Operadores", "https://www.youtube.com/embed/RDrfZ-7WE8c?si=JP0LvntY7_cxuWUB" },
                    { 4, 1, "Operadores lógicos e relacionais", "https://www.youtube.com/embed/Ig4QZNpVZYs?si=Eaes88_HwJc28Vp2" },
                    { 5, 1, "Introdução ao Scratch", "https://www.youtube.com/embed/GrPkuk1ezyo?si=QoDgOp2ZVSgM_CTM" },
                    { 6, 1, "Exercícios de Algoritmo", "https://www.youtube.com/embed/v2nCgGSVCeE?si=_-lFdQVYxv_1uJVB" },
                    { 7, 1, "Estruturas Condicionais 1", "https://www.youtube.com/embed/_g05aHdBAEY?si=YHLhKkoo8Cnaieub" },
                    { 8, 2, "SQL Server - Instalando no seu computador", "https://www.youtube.com/embed/OKqpZ6zbZwQ?si=PR8tj46glLT1VUyD" },
                    { 9, 2, "Orientações", "https://www.youtube.com/embed/qEitmEuXG1I?si=71gXL6ykXdoTHoxk" },
                    { 10, 2, "Conceitos Essenciais e Modelagem", "https://www.youtube.com/embed/N_0ujgVRrdI?si=kmYxFk0v6jv0SXSc" },
                    { 11, 3, "Relacionamento entre tabelas", "https://www.youtube.com/embed/HmFUrlQcCJ0?si=-E4k0khkUdH9ABS3" }
                });

            migrationBuilder.InsertData(
                table: "Alternativa",
                columns: new[] { "Id", "Correta", "QuestaoId", "Texto" },
                values: new object[,]
                {
                    { 1, false, 1, "É um meio de armazenar dados" },
                    { 2, true, 1, "É um bloco de código que pode ser chamado várias vezes" },
                    { 3, false, 2, "Um é repetido infinitamente, e o outro até que um valor seja verdadeiro" },
                    { 4, true, 2, "O loop while se repete até que um valor seja verdadeiro, e o loop for até que a iteração seja concluída uma certa quantidade de vezes." },
                    { 5, false, 3, "É uma sequência de comandos" },
                    { 6, true, 3, "É uma estrutura de armazenamento de dados" }
                });

            migrationBuilder.InsertData(
                table: "UsuarioEstadoVideo",
                columns: new[] { "EstadoId", "UsuarioId", "VideoId" },
                values: new object[,]
                {
                    { 2, "ddf093a6-6cb5-4ff7-9a64-83da34aee005", 1 },
                    { 2, "ddf093a6-6cb5-4ff7-9a64-83da34aee005", 7 },
                    { 2, "ddf093a6-6cb5-4ff7-9a64-83da34aee005", 8 },
                    { 2, "ddf093a6-6cb5-4ff7-9a64-83da34aee005", 9 },
                    { 2, "ddf093a6-6cb5-4ff7-9a64-83da34aee005", 10 },
                    { 2, "ddf093a6-6cb5-4ff7-9a64-83da34aee005", 11 },
                    { 3, "ddf093a6-6cb5-4ff7-9a64-83da34aee005", 2 },
                    { 3, "ddf093a6-6cb5-4ff7-9a64-83da34aee005", 3 },
                    { 3, "ddf093a6-6cb5-4ff7-9a64-83da34aee005", 4 },
                    { 3, "ddf093a6-6cb5-4ff7-9a64-83da34aee005", 5 },
                    { 3, "ddf093a6-6cb5-4ff7-9a64-83da34aee005", 6 }
                });

            migrationBuilder.InsertData(
                table: "Resposta",
                columns: new[] { "Id", "AlternativaId", "QuestaoId", "UsuarioId" },
                values: new object[,]
                {
                    { 1, 1, 1, "ddf093a6-6cb5-4ff7-9a64-83da34aee005" },
                    { 2, 4, 2, "ddf093a6-6cb5-4ff7-9a64-83da34aee005" },
                    { 3, 5, 3, "ddf093a6-6cb5-4ff7-9a64-83da34aee005" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alternativa_QuestaoId",
                table: "Alternativa",
                column: "QuestaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Curso_TrilhaId",
                table: "Curso",
                column: "TrilhaId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "IdentityRole",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityRoleClaim<string>_RoleId",
                table: "IdentityRoleClaim<string>",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "IdentityUser",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "IdentityUser",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUserClaim<string>_UserId",
                table: "IdentityUserClaim<string>",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUserLogin<string>_UserId",
                table: "IdentityUserLogin<string>",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUserRole<string>_RoleId",
                table: "IdentityUserRole<string>",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Modulo_CursoId",
                table: "Modulo",
                column: "CursoId");

            migrationBuilder.CreateIndex(
                name: "IX_Modulo_UsuarioId",
                table: "Modulo",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Prova_CursoId",
                table: "Prova",
                column: "CursoId");

            migrationBuilder.CreateIndex(
                name: "IX_Questao_ProvaId",
                table: "Questao",
                column: "ProvaId");

            migrationBuilder.CreateIndex(
                name: "IX_Resposta_AlternativaId",
                table: "Resposta",
                column: "AlternativaId");

            migrationBuilder.CreateIndex(
                name: "IX_Resposta_QuestaoId",
                table: "Resposta",
                column: "QuestaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Resposta_UsuarioId",
                table: "Resposta",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioCurso_CursoId",
                table: "UsuarioCurso",
                column: "CursoId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioEstadoCurso_CursoId",
                table: "UsuarioEstadoCurso",
                column: "CursoId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioEstadoCurso_EstadoId",
                table: "UsuarioEstadoCurso",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioEstadoModulo_EstadoId",
                table: "UsuarioEstadoModulo",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioEstadoModulo_ModuloId",
                table: "UsuarioEstadoModulo",
                column: "ModuloId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioEstadoVideo_EstadoId",
                table: "UsuarioEstadoVideo",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioEstadoVideo_VideoId",
                table: "UsuarioEstadoVideo",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_Video_ModuloId",
                table: "Video",
                column: "ModuloId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentityRoleClaim<string>");

            migrationBuilder.DropTable(
                name: "IdentityUserClaim<string>");

            migrationBuilder.DropTable(
                name: "IdentityUserLogin<string>");

            migrationBuilder.DropTable(
                name: "IdentityUserRole<string>");

            migrationBuilder.DropTable(
                name: "IdentityUserToken<string>");

            migrationBuilder.DropTable(
                name: "Progresso");

            migrationBuilder.DropTable(
                name: "Resposta");

            migrationBuilder.DropTable(
                name: "UsuarioCurso");

            migrationBuilder.DropTable(
                name: "UsuarioEstadoCurso");

            migrationBuilder.DropTable(
                name: "UsuarioEstadoModulo");

            migrationBuilder.DropTable(
                name: "UsuarioEstadoVideo");

            migrationBuilder.DropTable(
                name: "IdentityRole");

            migrationBuilder.DropTable(
                name: "Alternativa");

            migrationBuilder.DropTable(
                name: "Estado");

            migrationBuilder.DropTable(
                name: "Video");

            migrationBuilder.DropTable(
                name: "Questao");

            migrationBuilder.DropTable(
                name: "Modulo");

            migrationBuilder.DropTable(
                name: "Prova");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Curso");

            migrationBuilder.DropTable(
                name: "IdentityUser");

            migrationBuilder.DropTable(
                name: "Trilha");
        }
    }
}

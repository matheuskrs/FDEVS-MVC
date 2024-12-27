using FDevs.Data;
using FDevs.Models;
using FDevs.Services.EstadoCursoService;
using FDevs.Services.EstadoModuloService;
using FDevs.Services.EstadoVideoService;
using FDevs.Services.UsuarioCursoService;
using FDevs.Services.VideoService;
using FDevs.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FDevs.Services.ProgressoService
{
    public class ProgressoService : IProgressoService
    {
        private readonly IEstadoVideoService _estadoVideoService;
        private readonly IEstadoModuloService _estadoModuloService;
        private readonly IEstadoCursoService _estadoCursoService;
        private readonly IUsuarioCursoService _usuarioCursoService;
        private readonly AppDbContext _context;

        public ProgressoService(
            IEstadoVideoService estadoVideoService,
            IEstadoModuloService estadoModuloService,
            IEstadoCursoService estadoCursoService,
            IUsuarioCursoService usuarioCursoService,
            AppDbContext context)
        {
            _estadoVideoService = estadoVideoService;
            _estadoModuloService = estadoModuloService;
            _estadoCursoService = estadoCursoService;
            _usuarioCursoService = usuarioCursoService;
            _context = context;
        }

        public async Task<Progresso> CalcularProgressosEmPorcentagemAsync(Progresso progresso, string usuarioId)
        {
            var cursos = await _usuarioCursoService.GetCursosPorUsuarioAsync(usuarioId);
            if (cursos == null) return null;
            var qtdCursos = 0;
            var qtdAndamento = 0;
            var qtdNaoIniciado = 0;
            var qtdConcluido = 0;
            foreach (var usuarioCurso in cursos)
            {
                qtdCursos += 1;
                var estadoCurso = progresso.UsuarioEstadoCursos
                    .SingleOrDefault(uec => uec.CursoId == usuarioCurso.Curso.Id);
                switch (estadoCurso.EstadoId)
                {
                    case 1: qtdAndamento++; break;
                    case 2: qtdConcluido++; break;
                    case 3: qtdNaoIniciado++; break;
                }
            }

            progresso.QtdCursos = qtdCursos;
            progresso.QtdAndamento = qtdAndamento;
            progresso.QtdConcluido = qtdConcluido;
            progresso.QtdNaoIniciado = qtdNaoIniciado;

            if (qtdCursos > 0)
            {
                progresso.ProgressoAmarelo = Math.Round((double)qtdAndamento / qtdCursos * 100);
                progresso.ProgressoVermelho = Math.Round((double)qtdNaoIniciado / qtdCursos * 100);
                progresso.ProgressoVerde = 100 - (progresso.ProgressoAmarelo + progresso.ProgressoVermelho);
            }
            else
            {
                progresso.ProgressoAmarelo = 0;
                progresso.ProgressoVermelho = 0;
                progresso.ProgressoVerde = 0;
            }

            return progresso;
        }

        public async Task<Progresso> ObterProgressoAsync(string usuarioId)
        {
            return new Progresso
            {
                UsuarioEstadoVideos = await _estadoVideoService.GetByUsuarioIdAsync(usuarioId),
                UsuarioEstadoModulos = await _estadoModuloService.GetByUsuarioIdAsync(usuarioId),
                UsuarioEstadoCursos = await _estadoCursoService.GetByUsuarioIdAsync(usuarioId)
            };
        }

    }
}

using FDevs.Data;
using FDevs.Models;
using FDevs.Services.EstadoCursoService;
using FDevs.Services.EstadoModuloService;
using FDevs.Services.EstadoVideoService;
using Microsoft.EntityFrameworkCore;

namespace FDevs.Services.ProgressoService
{
    public class ProgressoService : IProgressoService
    {
        private readonly IEstadoVideoService _videoService;
        private readonly IEstadoModuloService _moduloService;
        private readonly IEstadoCursoService _cursoService;
        private readonly AppDbContext _context;

        public ProgressoService(
            IEstadoVideoService videoService,
            IEstadoModuloService moduloService,
            IEstadoCursoService cursoService,
            AppDbContext context)
        {
            _videoService = videoService;
            _moduloService = moduloService;
            _cursoService = cursoService;
            _context = context;
        }

        public async Task<Progresso> ObterProgressoAsync(string usuarioId)
        {
            return new Progresso
            {
                UsuarioEstadoVideos = await _videoService.GetByUsuarioIdAsync(usuarioId),
                UsuarioEstadoModulos = await _moduloService.GetByUsuarioIdAsync(usuarioId),
                UsuarioEstadoCursos = await _cursoService.GetByUsuarioIdAsync(usuarioId)
            };
        }

        public async Task<bool> UpdateProgressoVideoParaAndamentoAsync(UsuarioEstadoVideo estadoVideo)
        {
            if (estadoVideo.EstadoId == 3)
            {

                var novoUsuarioEstadoVideo = new UsuarioEstadoVideo
                {
                    UsuarioId = estadoVideo.UsuarioId,
                    VideoId = estadoVideo.VideoId,
                    EstadoId = 1
                };
                _context.Remove(estadoVideo);
                await _context.SaveChangesAsync();
                _context.Add(novoUsuarioEstadoVideo);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}

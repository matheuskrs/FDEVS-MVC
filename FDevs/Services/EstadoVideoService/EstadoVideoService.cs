using FDevs.Data;
using FDevs.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Runtime.CompilerServices;

namespace FDevs.Services.EstadoVideoService
{
    public class EstadoVideoService : IEstadoVideoService
    {
        private readonly AppDbContext _context;

        public EstadoVideoService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<List<UsuarioEstadoVideo>> GetUsuarioEstadoVideosByVideoId(int videoId)
        {
            var usuarioEstadoVideos = await _context.UsuarioEstadoVideos
                .Where(uev => uev.VideoId == videoId)
                .ToListAsync();

            return usuarioEstadoVideos;
        }


        public async Task<List<UsuarioEstadoVideo>> GetUsuarioEstadoVideosAsync()
        {
            List<UsuarioEstadoVideo> usuarioEstadoVideos = await _context.UsuarioEstadoVideos
                .Include(uev => uev.Estado)
                .Include(uev => uev.Video)
                .Include(uev => uev.Usuario)
                .ToListAsync();
            return usuarioEstadoVideos;
        }

        public async Task<UsuarioEstadoVideo> GetUsuarioEstadoVideosByIdAsync(string usuarioId, int videoId)
        {
            UsuarioEstadoVideo usuarioEstadoVideo = await _context.UsuarioEstadoVideos
                .Include(uem => uem.Usuario)
                .Include(uem => uem.Estado)
                .Include(uem => uem.Video)
                .FirstOrDefaultAsync(uem =>
                    uem.UsuarioId == usuarioId &&
                    uem.VideoId == videoId);

            return usuarioEstadoVideo;
        }

        public async Task<List<UsuarioEstadoVideo>> GetByUsuarioIdAsync(string usuarioId)
        {
            List<UsuarioEstadoVideo> usuarioEstadoVideos = await _context.UsuarioEstadoVideos
                .Include(uev => uev.Usuario)
                .Include(uev => uev.Estado)
                .Include(uev => uev.Video)
                .Where(uev => uev.UsuarioId == usuarioId)
                .ToListAsync();

            return usuarioEstadoVideos;
        }


        public async Task<int> ObterQuantidadeVideosConcluidos(string usuarioId, Video video)
        {
            return await _context.UsuarioEstadoVideos
                    .Where(uev => uev.UsuarioId == usuarioId
                        && uev.EstadoId == 2
                        && _context.Videos.Any(v => v.Id == uev.VideoId && v.ModuloId == video.ModuloId))
                    .CountAsync();
        }

        public async Task<int> ObterQuantidadeVideos(Video video)
        {
            return await _context.Videos
                .Where(v => v.ModuloId == video.ModuloId)
                .CountAsync();
        }

        public async Task<bool> AtualizarEstadoVideoParaAndamentoAsync(UsuarioEstadoVideo estadoVideo)
        {
            if (estadoVideo.EstadoId != 3) return false;

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

        public async Task<bool> AtualizarEstadoVideoParaConcluidoAsync(UsuarioEstadoVideo estadoVideo)
        {
            _context.Remove(estadoVideo);
            await _context.SaveChangesAsync();

            var novoUsuarioEstadoVideo = new UsuarioEstadoVideo
            {
                UsuarioId = estadoVideo.UsuarioId,
                VideoId = estadoVideo.VideoId,
                EstadoId = 2
            };

            _context.Add(novoUsuarioEstadoVideo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<UsuarioEstadoVideo> CreateUsuarioEstadoVideo(UsuarioEstadoVideo novoUsuarioEstadoVideo)
        {
            _context.Add(novoUsuarioEstadoVideo);
            await _context.SaveChangesAsync();
            return novoUsuarioEstadoVideo;
        }

        public async Task<bool> Delete(UsuarioEstadoVideo usuarioEstadoVideo)
        {
            _context.Remove(usuarioEstadoVideo);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

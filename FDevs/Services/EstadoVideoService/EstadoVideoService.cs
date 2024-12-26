using FDevs.Data;
using FDevs.Models;
using Microsoft.EntityFrameworkCore;

namespace FDevs.Services.EstadoVideoService
{
    public class EstadoVideoService : IEstadoVideoService
    {
        private readonly AppDbContext _context;

        public EstadoVideoService(AppDbContext context)
        {
            _context = context;
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
    }
}
